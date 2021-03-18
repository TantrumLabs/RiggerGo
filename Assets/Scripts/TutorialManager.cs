using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public List<GameObject> m_targets;
    [Space]
    public UnityEngine.Events.UnityEvent m_endingEvent;
    private int m_currentTarget = 0;

    private void OnEnable() {
        m_currentTarget = 0;
        m_targets[0].SetActive(true);
    }

    private void Start() {
        foreach(GameObject go in m_targets){
            go.SetActive(false);
        }

        m_targets[0].SetActive(true);
    }

    public void NextTarget(){
        m_targets[m_currentTarget].SetActive(false);
        m_currentTarget++;

        if(m_currentTarget >= m_targets.Count){
            m_endingEvent.Invoke();
            return;
        }

        m_targets[m_currentTarget].SetActive(true);
    }
}
