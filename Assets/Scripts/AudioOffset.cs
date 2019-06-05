using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOffset : MonoBehaviour
{
    public static bool m_UKVersion = true;

    public bool m_ignoreRegion = false;
    public AudioSource m_audioSource;
    private float m_playBackTime;
    public List<MonoBehaviour> m_turnOff;
    public List<AudioClip> UKAudioClip;

    private int m_index = -1;

    void Start(){
        if(!m_UKVersion || m_ignoreRegion)
            return;
        
        if(UKAudioClip.Count == 0)
        {
            m_audioSource.clip = null;
            return;
        }

        m_audioSource.clip = UKAudioClip[0];
    }

    void Update()
    {
        if(m_UKVersion && !m_ignoreRegion)
            return;

        if(m_audioSource.isPlaying)
        {
            if(m_audioSource.time >= m_playBackTime)
            {
                m_audioSource.Stop();
            }
        }
    }

    public void ReplaceClip(AudioClip ac){
        m_audioSource.clip = ac;
        m_ignoreRegion = true;
    }

    public void StartAudioAt(float sec)
    {
        if(m_UKVersion && !m_ignoreRegion)
        {
            StartCoroutine(PlayVoiceOversOneAtATime());
        }

        else{
            m_audioSource.Stop();
            m_audioSource.time = sec;
            m_audioSource.Play();
        }
    }

    public void SetAudioOffset(float timeOffset)
    {
        m_playBackTime = m_audioSource.time + timeOffset;
    }

    public void TurnOffItems(){
        foreach(MonoBehaviour c in m_turnOff)
            Destroy(c);
    }

    public IEnumerator PlayVoiceOversOneAtATime(){
        foreach(AudioClip ac in UKAudioClip){
            m_audioSource.clip = ac;
            m_audioSource.Stop();
            m_audioSource.Play();

            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => m_audioSource.isPlaying == false);
        }
    }

    private void OnDisable() {
        StopCoroutine(PlayVoiceOversOneAtATime());
        m_audioSource.Stop();
    }

    private void OnEnable() {
        StopCoroutine(PlayVoiceOversOneAtATime());
        m_audioSource.Stop();
    }
}
