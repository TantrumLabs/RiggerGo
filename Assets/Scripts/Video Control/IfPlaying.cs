using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class IfPlaying : MonoBehaviour
{
    public UnityEvent m_onPlayEvent;
    private VideoPlayer m_videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        m_videoPlayer = gameObject.GetComponent<VideoPlayer>();
        StartWatch();
    }

    public void StartWatch()
    {
        StartCoroutine(WatchVideo());
    }

    public IEnumerator WatchVideo()
    {
        //Debug.Log(m_videoPlayer.isPlaying);

        yield return new WaitWhile(() => m_videoPlayer.isPlaying == false);



        m_onPlayEvent.Invoke();
    }
}
