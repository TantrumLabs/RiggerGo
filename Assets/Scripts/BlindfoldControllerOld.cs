using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VideoControl;

public class BlindfoldControllerOld : MonoBehaviour
{
    public VideoPlayerController m_controller;
    public float m_waitTimer = 5;
    private VideoEventHolder m_holder;
    public UnityEngine.UI.Image blindfold, m_loadingLogo;
    public TMPro.TextMeshProUGUI m_loading;
    public float m_revealTime;
    public float m_fadeTime;
    public UnityEvent m_doneEvent;
    private float m_percentageDonePreparing;
    private bool canReveal{
        get{
            // int preparedCount = 0;
            // var list = VideoManager.instance.m_prepareThese;
            // foreach(VideoControl.VideoPlayerController2 vp in list){
            //     if(vp.m_videoPlayer.isPrepared == false){
            //         return false;
            //     }
            //     preparedCount++;
            //     m_percentageDonePreparing = (float)preparedCount / (float)list.Count;
            // }
            // return true;

            return m_controller.m_videoPlayer.isPrepared;
        }
    }

    public void SetController(VideoPlayerController p_new){
        m_controller = p_new;
    }

    public void SetHolder(VideoEventHolder p_veh){
        m_holder = p_veh;
    }

    public void StartFading(){
        StartCoroutine(FadeInOut());
    }

    public IEnumerator FadeInOut()
    {
        //var m_lazers = FindObjectsOfType<TantrumInteractionLaserPointerPico>();

        //foreach(TantrumInteractionLaserPointerPico lazer in m_lazers){
        //    lazer.enabled = false;
        //}

        if (blindfold != null)
        {
            Color c = blindfold.color;
            Color loadingText = m_loading.color;
            Color loadingLogo = m_loadingLogo.color;

            //print(blindfold.color);
            loadingLogo.a = 1f;
            loadingText.a = 1f;
            c.a = 1f;
            blindfold.color = c;
            m_loadingLogo.color = loadingLogo;
            m_loading.color = loadingText;

            StartCoroutine(ProgressBar());
            yield return new WaitUntil(() => canReveal);

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

        m_doneEvent.Invoke();

        //foreach(TantrumInteractionLaserPointerPico lazer in m_lazers){
        //    lazer.enabled = true;
        //}

        if(m_holder != null){
            m_holder.SetManagerPlay();
        }
    }

    IEnumerator ProgressBar(){
        if(m_loadingLogo.type != UnityEngine.UI.Image.Type.Filled){
            yield break;
        }
        
        while(canReveal == false){
            m_loadingLogo.fillAmount = m_percentageDonePreparing;
            yield return null;
        }

        m_loadingLogo.fillAmount = 1;
        m_loadingLogo.GetComponent<RotateGameObjectOnAxis>().enabled = true;
    }
}
