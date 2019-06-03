using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MironDB_TestManager : MonoBehaviour
{
    public int testScenarioID;
    public ScoreKeeper m_scoreKeeper;

    [SerializeField]
    bool testComplete = false;
    [SerializeField]
    bool passed = true;

    [SerializeField]
    bool isInstance = false;

    private static MironDB_TestManager _instance;
    public static MironDB_TestManager instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MironDB_TestManager>();
                DontDestroyOnLoad(_instance.gameObject);
                _instance.isInstance = true;
            }

            return _instance;
        }
    }

    void Awake()
    {
        //transform.parent = null;
        if(instance != this) Destroy(gameObject);
    }

    Mouledoux.Components.Mediator.Subscriptions subscriptions =
        new Mouledoux.Components.Mediator.Subscriptions();

    Mouledoux.Callback.Callback finishTest = null;

    void Start()
    {
        finishTest += FinishTest;
        subscriptions.Subscribe("endtest", finishTest);
        subscriptions.Subscribe("loginTry", StartLogin);
        
    }

    private void FailTest(object[] obj)
    {
        if(!passed) return;

        var data = (QuestionHazardData)obj[0];

        passed = false;
        testComplete = true;

        MironDB.MironDB_Manager.UpdateTest(testScenarioID, 1, 
            data.m_questionsMissed + " " + data.m_hazardsMissed);
    }


    private void DroppedObjectFail(object[] obj)
    {
        testComplete = true;
        MironDB.MironDB_Manager.UpdateTest(testScenarioID, 11, "Dropped Object");
    }

    public void StartLogin(object[] obj){
        StartCoroutine(TryLogin(obj));
    }

    public IEnumerator TryLogin(object[] obj){
        yield return null;

        DelayEventOnStart action = (DelayEventOnStart)obj[2];
        string email = (string)obj[0];
        string password = (string)obj[1];
        string output = "";

        MironDB.MironDB_Manager.Login(email, password, output);

        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => MironDB.MironDB_Manager.m_operating == false);

        if(MironDB.MironDB_Manager.statusReturn.status == "ok")
            action.m_action.Invoke();

        else{
            Debug.Log("Could not login!");
        }

    }

    public void FinishTest(object[] obj)
    {
        StartCoroutine(FinishTestRoutine());
    }

    public IEnumerator FinishTestRoutine()
    {
        print("ending test...");
        yield return null;

        if(!testComplete)
        {
            MironDB.MironDB_Manager.UpdateTest(testScenarioID, 0, "DNF");
        }

        else if(passed)
        {
            MironDB.MironDB_Manager.UpdateTest(testScenarioID, 2, 
                m_scoreKeeper.Data.m_questionsMissed + " " + m_scoreKeeper.Data.m_hazardsMissed);
        }

        yield return new WaitForSeconds(1f);

        MironDB.MironDB_Manager.FinishTest();
        subscriptions.UnsubscribeAll();
        Destroy(gameObject);
    }

    public void AssesScores(){
        if(m_scoreKeeper.Percentage < m_scoreKeeper.m_passingGrade)
            FailTest(new object[]{});

        FinishTest(new object[]{});
    }
}