﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    // private static ScoreKeeper _instance;
    // public static ScoreKeeper instance{
    //     get{
    //         if(_instance == null)
    //             _instance = FindObjectOfType<ScoreKeeper>();
    //         return _instance;
    //     }
    // }

    public static bool m_demo = false;
    public ForceTeleport m_forceTeleport;
    public TMPro.TextMeshProUGUI m_resultsScreen;

    private QuestionHazardData data = new QuestionHazardData();

    public LockerManager m_lockerManager;

    public TransitionDataHolder m_dataHolder;

    public DelayEventOnStart m_wrongVoiceOver;
    public DelayEventOnStart m_rightVoiceOver;
    public AudioSource m_audioSource;
    public float m_passingGrade;

    public int Score{
        get{ return data.m_score;}
    }

    public float Percentage{get{ return data.m_score / data.m_maxScore;}}

    public QuestionHazardData Data => data;

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = new Mouledoux.Components.Mediator.Subscriptions();

    private Mouledoux.Callback.Callback onScored = null;

    // Start is called before the first frame update
    void Awake()
    {
        if(SceneManager.GetActiveScene().name == "main_Demo"){
            m_demo = true;
        }

        onScored += PacketRecieve;

        //m_subscriptions.Subscribe("newslingactive", TableShutUp);
        m_subscriptions.Subscribe("incrementcurrentscore", onScored);
    }

    void Start(){
        SetMaxScore();
    }

    public void AddToScore(int addition){
        if(addition > 0){
            data.m_score += addition;

            // if(!m_demo)
            //     MironDB_TestManager.instance.UpdateTest(DataBase.DBCodeAtlas.RIGHT, $"");
            m_rightVoiceOver.BeginCountdown();
        }
    }

    // Depricated!    
    // public void AppendQuestion(TMPro.TextMeshProUGUI text){
    //     data.m_questionsMissed += "Z" + m_forceTeleport.currentPoint + " " + text.text + ",";
    //     data.m_questionCount++;
    //     if(!m_demo)
    //         MironDB_TestManager.instance.UpdateTest(DataBase.DBCodeAtlas.WRONG, $"Wrong Question! Zone  {m_forceTeleport.currentPoint}");

    //     m_wrongVoiceOver.BeginCountdown();
    // }

    public void AppendHazard(List<string> names, int hazardCount){
        foreach(string s in names){
            data.m_hazardsMissed += "Z" + m_forceTeleport.currentPoint + " " + s + ",";
            data.m_hazardCount++;
        }

        if(!m_demo){
            string message = $"Hazard Area {m_forceTeleport.currentPoint - 4}-- ";

            if(names.Count > 0){
                message += $"Missed:{names.Count}/{hazardCount} //";
                foreach(string s in names){
                    message += (names[names.Count - 1] == s) ? s : s +", ";
                }
                MironDB_TestManager.instance.UpdateTest(7000 + (m_forceTeleport.currentPoint - 4), message);
            }

            else{
                message += "Missed: 0";
                MironDB_TestManager.instance.UpdateTest(7000 + (m_forceTeleport.currentPoint - 4), message);
            }

        }
            
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

        if(lockerManager == null)
            return;

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
        var inst = m_dataHolder;
        result += $"Congratulations!\n";
        result += "Your score is: " + data.m_score + "/" + data.m_maxScore + "\n";
        result += "You missed " + data.m_questionCount + " question(s) and " + data.m_hazardCount +
            " hazard(s).";

        m_resultsScreen.text = result;

        string message = $"Test Complete! Final Score: {data.m_score}/{data.m_maxScore}|||Question(s) missed:{data.m_questionCount}|||Hazard(s) missed:{data.m_hazardCount}";
        MironDB_TestManager.instance.FinalUpdate(message);

        MironDB_TestManager.instance.FinishTest(new object[]{});
    }

    public void GetQuestionAndGivenAnswer(GameObject go)
    {
        var question = go.transform.Find("Text Field").Find("Text")
            .GetComponent<TMPro.TextMeshProUGUI>().text;


        string answerGiven = "";
        string correctAnswer = "";
        Transform tTransform = null;

        foreach(Transform t in go.transform)
        {
            if(t.gameObject.activeSelf && (t.name.Contains("Wrong") || t.name.Contains(" Wrong ") || t.name.Contains("Wrong;")))
            {
                TMPro.TextMeshProUGUI text = t.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if(text != null)
                {
                    answerGiven = text.text;
                }

                else
                {
                    var split = t.name.Split(';');
                    answerGiven = split[1].Trim();
                }
                
            }

            else if(t.gameObject.activeSelf && (t.name.Contains("Correct") || t.name.Contains("Correct ") || t.name.Contains("Correct;")))
            {
                TMPro.TextMeshProUGUI text = t.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if(text != null)
                {
                    correctAnswer = text.text;
                }

                else
                {
                    var split = t.name.Split(';');
                    correctAnswer = split[1].Trim();
                }

                tTransform = t;
            }
        }

        if(answerGiven != "")
        {
            m_wrongVoiceOver.BeginCountdown();
            data.m_questionsMissed += "Z" + m_forceTeleport.currentPoint + " " + question + "Answer Given: " + answerGiven + "\n";
            data.m_questionCount++;
        }

        else
        {
            AddToScore(1);
        }

        var questionSplit = question.Split('\n');

        question = "";
        foreach(string s in questionSplit)
        {
            question += s + " ";
        }

        if(answerGiven != "")
        {
            var answerSplit = answerGiven.Split('.');
            if(answerSplit.Length <= 1)
                answerGiven = answerSplit[0];
            else
                answerGiven = answerSplit[1];
        }

        var correctAnswerSplit = correctAnswer.Split('.');
        if(correctAnswerSplit.Length <= 1)
            correctAnswer = correctAnswerSplit[0];
        else
            correctAnswer = correctAnswerSplit[1];


        if(!m_demo)
        {
            bool passed = answerGiven == "";
            string message = $"{(passed ? "Correct" : "Incorrect" )}:{question.Trim()}|||" +
                        $"Answer given:{(passed ? correctAnswer.Trim() : answerGiven.Trim())}||| " +
                        $"Expected answer:{correctAnswer.Trim()}";
            MironDB_TestManager.instance.UpdateTest(m_forceTeleport.currentPoint + 7000, message);
        }
    }

    [ContextMenu("Save Data")]
    public void SaveScore(){
        SaveLocally.SaveScoreData(data, m_dataHolder.m_emailData);
    }

    [ContextMenu("Load Data")]
    public void LoadData(){
        QuestionHazardData d = SaveLocally.LoadScoreData("0001_anthonyjtouchet@gmail.com.xml");
        Debug.Log(d.m_questionCount);
    }

    public void CallSceneEnd(string name){
        m_dataHolder.GoToScene(name);
    }

    public void TableShutUp(object[] obj){
        if(obj[0] != null)
        {
            m_audioSource.Stop();
        }
    }
}