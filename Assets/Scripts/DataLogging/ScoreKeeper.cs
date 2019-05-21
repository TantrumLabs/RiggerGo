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

    private QuestionHazardData data = new QuestionHazardData();

    public LockerManager m_lockerManager;

    public int Score{
        get{ return data.m_score;}
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
            data.m_score += addition;
    }

    public void AppendQuestion(TMPro.TextMeshProUGUI text){
        data.m_questionsMissed += "Z" + m_forceTeleport.currentPoint + " " + text.text + ",";
        data.m_questionCount++;
    }

    public void AppendHazard(GameObject hazard){
        data.m_hazardsMissed += "Z" + m_forceTeleport.currentPoint + " " + hazard.name + ",";
        data.m_hazardCount++;
    }

    public string ReturnResults(){
        string result = "";

        result += "Final Score" + data.m_score + "\n";
        result += "Questions Missed: " + data.m_questionsMissed + "\n";
        result += "Hazards Missed: " + data.m_hazardsMissed;

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
                if(go.name != "Locker Identification")
                    data.m_maxScore++;
            }
        }

        //Hazards
        foreach(SearchAndFindManager saf in hazardManagers){
            foreach(GameObject go in saf.m_hazards)
                data.m_maxScore++;
        }

        //Locker
        foreach(GameObject go in lockerManager.m_lockerItems){
            if(!go.GetComponent<HazardObject>())
                continue;

            var ho = go.GetComponent<HazardObject>();

            if(ho.m_scoreValue > 0)
                data.m_maxScore++;
        }
    }

    public void PacketRecieve(object[] obj)
    {
        var packet = obj[0] as Mouledoux.Callback.Packet;

        AddToScore((int)packet.floats[0]);
    }

    public void SetText(){
        string result = "";
        var inst = TransitionDataHolder.instance;
        result += "Congrats " + inst.m_firstName + " " + inst.m_lastName + "!\n";
        result += "Your score is: " + data.m_score + "/" + data.m_maxScore + "\n";
        result += "You missed " + data.m_questionCount + " questions and " + data.m_hazardCount +
            " hazards.";

        m_resultsScreen.text = result;
    }

    public void GetQuestionAndGivenAnswer(GameObject go){
        var question = go.transform.Find("Text Field").Find("Text")
            .GetComponent<TMPro.TextMeshProUGUI>().text;

        string answerGiven = "";
        foreach(Transform t in go.transform){
            if(t.gameObject.activeSelf && (t.name.Contains("Wrong") || t.name.Contains(" Wrong "))){
                TMPro.TextMeshProUGUI text = t.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                answerGiven = text.text;
            }
        }

        var questionSplit = question.Split('\n');

        question = "";
        foreach(string s in questionSplit){
            question += s + " ";
        }

        var answerSplit = answerGiven.Split(' ');
        if(answerSplit[1] == null)
            answerGiven = answerSplit[0];
        else
            answerGiven = answerSplit[1];

        data.m_questionsMissed += "Z" + m_forceTeleport.currentPoint + " " + question + "Answer Given: " + answerGiven + "\n";
        data.m_questionCount++;
    }

    [ContextMenu("Save Data")]
    public void SaveScore(){
        SaveLocally.SaveScoreData(data, TransitionDataHolder.instance.m_emailData);
    }

    [ContextMenu("Load Data")]
    public void LoadData(){
        QuestionHazardData d = SaveLocally.LoadScoreData("0001_anthonyjtouchet@gmail.com.xml");
        Debug.Log(d.m_questionCount);
    }

    public void CallSceneEnd(string name){
        TransitionDataHolder.instance.GoToScene(name);
    }
}
