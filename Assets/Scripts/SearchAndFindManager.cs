using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAndFindManager : MonoBehaviour
{
    public List<GameObject> m_hazards= new List<GameObject>();

    public UnityEngine.Events.UnityEvent m_onEmpty;

    [ContextMenu("Highlight Missed Hazards")]
    public void HighlightHazards(){
        List<string> hazardNames = new List<string>();

        foreach(GameObject go in m_hazards){
            var goodOn = go.transform.Find("Good").gameObject;
            if(goodOn != null && goodOn.activeSelf == true)
                continue;
            
            var highLight = go.transform.Find("Notice Me");

            if(highLight == null)
                continue;

            highLight.gameObject.SetActive(true);
            hazardNames.Add(go.name);
        }

        var scoreKeeper = FindObjectOfType<ScoreKeeper>();
        scoreKeeper.AppendHazard(hazardNames, m_hazards.Count);
    }

    [ContextMenu("Check Empty")]
    public void CheckEmpty(){
        foreach(GameObject go in m_hazards){
            var goodOn = go.transform.Find("Good").gameObject;
            if(goodOn != null && goodOn.activeSelf == true)
                continue;
            else{
                Debug.Log("Someone is still active.");
                HighlightHazards();
                return;
            }
        }

        m_onEmpty.Invoke();
    }

    public void DestroyHazardScripts(){
        foreach(GameObject go in m_hazards)
            Destroy(go.GetComponent<HazardObject>());
    }
}
