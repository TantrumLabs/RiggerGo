using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VideoControl;

public class BlindfoldControllerTrainerFeedback : MonoBehaviour
{
    public VideoPlayerController m_controller;
    public UnityEngine.UI.Image blindfold, m_loadingLogo;
    public TMPro.TextMeshProUGUI m_loading;
    public float m_waitTimer = 5;
    public float m_revealTime;
    public float m_fadeTime;

    public void SetController(VideoPlayerController p_vpc){
        m_controller = p_vpc;
    }

    public void StartFading(){
        StartCoroutine(FadeInOut());
    }

    public IEnumerator FadeInOut()
    {
        var m_lazers = FindObjectsOfType<TantrumInteractionLaserPointerPico>();

        m_controller.m_videoPlayer.Pause();

        foreach(TantrumInteractionLaserPointerPico lazer in m_lazers){
            lazer.enabled = false;
        }

        if (blindfold != null)
        {
            Color c = blindfold.color;
            Color loadingText = m_loading.color;
            Color loadingLogo = m_loadingLogo.color;

            print(blindfold.color);
            loadingLogo.a = 1f;
            loadingText.a = 1f;
            c.a = 1f;
            blindfold.color = c;
            m_loadingLogo.color = loadingLogo;
            m_loading.color = loadingText;

            StartCoroutine(ProgressBar());

            yield return new WaitForSeconds(m_waitTimer);

            while (blindfold.color.a > 0.001f)
            {
                var delta = (Time.deltaTime / m_fadeTime);

                c.a -= delta;
                loadingText.a -= delta;
                loadingLogo.a -= delta;

                m_loadingLogo.color = loadingLogo;
                m_loading.color = loadingText;
                blindfold.color = c;
                yield return null;
            }

            loadingLogo.a = 0f;
            loadingText.a = 0f;
            c.a = 0f;
            blindfold.color = c;
            m_loading.color = loadingText;
            m_loadingLogo.color = loadingLogo;
        }

        m_controller.m_videoPlayer.Play();

        foreach(TantrumInteractionLaserPointerPico lazer in m_lazers){
            lazer.enabled = true;
        }
    }

    IEnumerator ProgressBar(){
        if(m_loadingLogo.type != UnityEngine.UI.Image.Type.Filled){
            yield break;
        }

        m_loadingLogo.fillAmount = 1;
        m_loadingLogo.GetComponent<RotateGameObjectOnAxis>().enabled = true;
    }
}
