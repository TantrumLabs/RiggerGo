using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private static ScoreKeeper _instance;
    public static ScoreKeeper instance{
        get{
            if(_instance == null)
                _instance = FindObjectOfType<ScoreKeeper>();
            return _instance;
        }
    }

    public ForceTeleport m_forceTeleport;
    public TMPro.TextMeshProUGUI m_resultsScreen;

    private int m_score;
    private int m_maxScore;
    private string m_questionsMissed, m_hazardsMissed;
    private int m_questionCount = 0, m_hazardCount = 0;
    public LockerManager m_lockerManager;

    public int Score{
        get{ return m_score;}
    }

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = new Mouledoux.Components.Mediator.Subscriptions();

    private Mouledoux.Callback.Callback onScored = null;

    // Start is called before the first frame update
    void Awake()
    {
        onScored += PacketRecieve;

        m_subscriptions.Subscribe("incrementcurrentscore", onScored);
    }

    void Start(){
        SetMaxScore();
    }

    public void AddToScore(int addition){
        if(addition > 0)
            m_score += addition;
    }

    public void AppendQuestion(TMPro.TextMeshProUGUI text){
        m_questionsMissed += "Z" + m_forceTeleport.currentPoint + " " + text.text + ",";
        m_questionCount++;
    }

    public void AppendHazard(GameObject hazard){
        m_hazardsMissed += "Z" + m_forceTeleport.currentPoint + " " + hazard.name + ",";
        m_hazardCount++;
    }

    public string ReturnResults(){
        string result = "";

        result += "Final Score" + m_score + "\n";
        result += "Questions Missed: " + m_questionsMissed + "\n";
        result += "Hazards Missed: " + m_hazardsMissed;

        return result;
    }

    public void SetMaxScore(){
        var questionManagers = FindObjectsOfType<QuestionManager>();
        var hazardManagers = FindObjectsOfType<SearchAndFindManager>();
        var lockerManager = m_lockerManager;

        //Questions
        foreach(QuestionManager qm in questionManagers){
            if(qm.GetComponent<LockerManager>())
                continue;

            if(qm.m_questionGameObjects[0].name == "Sling Table")
                continue;
            
            foreach(GameObject go in qm.m_questionGameObjects){
                m_maxScore++;
            }
        }

        //Hazards
        foreach(SearchAndFindManager saf in hazardManagers){
            foreach(GameObject go in saf.m_hazards)
                m_maxScore++;
        }

        //Locker
        foreach(GameObject go in lockerManager.m_lockerItems){
            if(!go.GetComponent<HazardObject>())
                continue;

            var ho = go.GetComponent<HazardObject>();

            if(ho.m_scoreValue > 0)
                m_maxScore++;
        }
    }

    public void PacketRecieve(object[] obj)
    {
        var packet = obj[0] as Mouledoux.Callback.Packet;

        AddToScore((int)packet.floats[0]);
    }

    [ContextMenu("Debug Wrong")]
    private void LogIt(){
        Debug.Log(m_questionsMissed);
        Debug.Log(m_hazardsMissed);
    }

    public void SetText(){
        string result = "";
        result += "Your score is: " + m_score + "/" + m_maxScore + "\n";
        result += "You missed " + m_questionCount + " questions and " + m_hazardCount +
            " hazards.";

        m_resultsScreen.text = result;
    }

    //Find a way to record both answer and Question.

    //for question: Canvas/Text Field/Text/TMPro.TextMeshProUGUI.text
    //for answer: 
    //  Canvas/Look at all active children/Find the one that has Wrong in name.
}
