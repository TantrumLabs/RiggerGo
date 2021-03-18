﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockerManager : MonoBehaviour
{
    public List<GameObject> m_lockerItems = new List<GameObject>();
    public TMPro.TextMeshProUGUI m_displayText;

    private float m_scoreValue;

    public float scoreValue{
        get{return m_scoreValue;}
    }

    public void AddToScore(HazardObject hazard){
        m_scoreValue += hazard.m_scoreValue;
    }

    public void SendScore(){
        var packet = new Mouledoux.Callback.Packet();
        packet.floats = new float[]{m_scoreValue};
        packet.bools = new bool[]{false};
        object[] data = {packet};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("incrementcurrentscore", data);
    }

    public void LockerFinished(){
        string result = "";

        int correctScore = 0;
        int wrongScore = 0;
        foreach(GameObject go in m_lockerItems){
            var lockerItem = go.GetComponent<LockerItem>();
            var hazardObject = go.GetComponent<HazardObject>();

            TurnOffChild(go, "Toggle Highlight");

            if(lockerItem.turnOn){
                if(hazardObject.m_scoreValue < 0)
                {
                    TurnOnChild(go, "Correct highlight");
                    correctScore++;
                    continue;
                }

                else{
                    TurnOnChild(go, "Wrong Highlight");
                    wrongScore++;
                }
            }
        }

        foreach(GameObject go in m_lockerItems){
            var lockerItem = go.GetComponent<LockerItem>();
            var hazardObject = go.GetComponent<HazardObject>();

            if(lockerItem.turnOn == false && hazardObject.m_scoreValue > 0){
                result += go.name+", ";
                TurnOnChild(go, "Highlight");
            }
        }
        
        result.Trim();
        var resultSplit = result.Split(',');
        result = "";
        for(int i = 0; i < resultSplit.Length; i++){
            result += (i == resultSplit.Length - 1)? $"{resultSplit[i]}." : $"{resultSplit[i]}, ";
        }

        string message = $"PPE Locker-- ";
        if(correctScore - wrongScore >= 6){
            
            MironDB_TestManager.instance.UpdateTest(7001, message + $"Message:All proper PPE was selected.");
        }

        else{
            MironDB_TestManager.instance.UpdateTest(7001, message + $"Correct PPE selected:{correctScore}|||6. Wrong PPE selected:{wrongScore}|||This is the PPE the user missed: {result.Trim()}");
        }

        result += "Correct: " + correctScore + "/6";
        result += "\n";
        result += "Wrong: " + wrongScore;
        m_displayText.text = result;
        SendScore();
    }

    private void TurnOnChild(GameObject go, string name){
        foreach(Transform t in go.transform){
            if(t.name == name){
                t.gameObject.SetActive(true);
            }
        }
    }

    private void TurnOffChild(GameObject go, string name){
        foreach(Transform t in go.transform){
            if(t.name == name){
                t.gameObject.SetActive(false);
            }
        }
    }
}
