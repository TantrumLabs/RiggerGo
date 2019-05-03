using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOffset : MonoBehaviour
{
    public AudioSource m_audioSource;
    private float m_playBackTime;


    void Update()
    {
        if(m_audioSource.isPlaying)
        {
            if(m_audioSource.time >= m_playBackTime)
            {
                m_audioSource.Stop();
            }
        }
    }


    public void StartAudioAt(float sec)
    {
        m_audioSource.Stop();
        m_audioSource.time = sec;
        m_audioSource.Play();
    }

    public void SetAudioOffset(float timeOffset)
    {
        m_playBackTime = m_audioSource.time + timeOffset;
    }

}
