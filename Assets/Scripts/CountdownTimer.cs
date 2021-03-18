using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    public float m_waitTime;
    public bool m_canStop;

    public UnityEvent m_failEvents;

    private float m_maxTime;

    [ContextMenu("Start Timer")]
    public void StartTimer(){
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown(){
        m_maxTime = Time.time + m_waitTime;
        while(Time.time < m_maxTime){
            if(m_canStop){
                m_maxTime = 0;
                yield break;
            }
            yield return null;
        }

        m_failEvents.Invoke();
    }
}
