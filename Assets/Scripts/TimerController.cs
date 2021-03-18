using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TimerController : MonoBehaviour
{
    public AudioSource m_tickNoise;
    private bool m_forceStop = false;
    public float m_timeLimit = 30;
    private Image m_timerImage;
    public UnityEngine.Events.UnityEvent m_timesUpEvent;
    private float m_lastTickAt = 0.95f;

    Mouledoux.Components.Mediator.Subscriptions m_sub = new Mouledoux.Components.Mediator.Subscriptions();

    // Start is called before the first frame update
    void Awake()
    {
        m_timerImage = GetComponent<Image>();
    }

    private void Start() {
        m_sub.Subscribe("stoptimer", delegate{StopTimer();});
    }

    [ContextMenu("Start Timer")]
    public void StartTimer(){
        m_timerImage.fillAmount = 1;
        m_forceStop = false;
        StartCoroutine(TimerEnumerator());
    }

    [ContextMenu("Stop Timer")]
    public void StopTimer(){
        m_forceStop = true;
    }

    private IEnumerator TimerEnumerator(){
        float oldtime = Time.time;
        float timePassed = 0;
        while(m_forceStop == false){
            timePassed = Time.time - oldtime;
            
            if(timePassed >= m_timeLimit){
                m_forceStop = true;
                yield return null;
            }

            else{
                m_timerImage.fillAmount = 1 - (timePassed/m_timeLimit);

                if(m_timerImage.fillAmount <= m_lastTickAt){

                    if(m_timerImage.fillAmount < .15f){
                        m_lastTickAt = m_timerImage.fillAmount -.0175f;
                        m_tickNoise.Play();
                    }

                    else if(m_timerImage.fillAmount < .3f){
                        m_lastTickAt = m_timerImage.fillAmount -.025f;
                        m_tickNoise.Play();
                    }

                    else{
                        m_lastTickAt = m_timerImage.fillAmount -.05f;
                        m_tickNoise.Play();
                    }
                }
            }

            yield return null;
        }

        if(timePassed >= m_timeLimit && m_forceStop){
            m_timesUpEvent.Invoke();
        }
    }
}
