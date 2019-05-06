using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockerManager : MonoBehaviour
{
    public List<GameObject> m_lockerItems;
    public TMPro.TextMeshProUGUI m_displayText;

    private float m_scoreValue;

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
                //result += go.name+", ";
                TurnOnChild(go, "Highlight");
            }
        }

        result += "Correct: " + correctScore + "/7";
        result += "\n";
        result += "Wrong: " + wrongScore;
        m_displayText.text = result;
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
