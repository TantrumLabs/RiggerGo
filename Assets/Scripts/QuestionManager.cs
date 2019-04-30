using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public bool m_turnGameobjectsOff = true;

    public List<GameObject> m_questionGameObjects =  new List<GameObject>();

    public UnityEngine.Events.UnityEvent m_onEmpty;


    void Start(){
        if(m_questionGameObjects.Count <= 0){
            m_onEmpty.Invoke();
            return;
        }

        if(m_turnGameobjectsOff){
            foreach(GameObject go in m_questionGameObjects)
                go.SetActive(false);
        
            m_questionGameObjects[0].SetActive(true);
        }
    }

    [ContextMenu("Next Question")]
    public void RemoveCurrentQuestion(){
        if(m_turnGameobjectsOff){
            m_questionGameObjects[0].SetActive(false);
        }
        m_questionGameObjects.Remove(m_questionGameObjects[0]);

        if(m_questionGameObjects.Count <= 0){
            m_onEmpty.Invoke();
            return;
        }

        if(m_turnGameobjectsOff){
            m_questionGameObjects[0].SetActive(true);
        }
    }
}
