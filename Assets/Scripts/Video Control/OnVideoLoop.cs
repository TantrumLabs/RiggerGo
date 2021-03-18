using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class OnVideoLoop : MonoBehaviour
{
    public UnityEvent m_onLoopEvent;

    // Start is called before the first frame update
    void Start()
    {
        var vp = GetComponent<VideoPlayer>();
        vp.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer p_vp)
    {
        m_onLoopEvent.Invoke();
    }
}
