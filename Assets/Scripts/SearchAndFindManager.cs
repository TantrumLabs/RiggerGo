using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAndFindManager : MonoBehaviour
{
    public List<GameObject> m_hazards= new List<GameObject>();

    public UnityEngine.Events.UnityEvent m_onEmpty;

    [ContextMenu("Highlight Missed Hazards")]
    public void HighlightHazards(){
        foreach(GameObject go in m_hazards){
            var goodOn = go.transform.FindChild("Good").gameObject;
            if(goodOn != null && goodOn.activeSelf == true)
                continue;
            
            var highLight = go.transform.FindChild("Notice Me");

            if(highLight == null)
                continue;

            highLight.gameObject.SetActive(true);
        }
    }

    [ContextMenu("Check Empty")]
    public void OnEmpty(){
        foreach(GameObject go in m_hazards){
            var goodOn = go.transform.FindChild("Good").gameObject;
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
}
