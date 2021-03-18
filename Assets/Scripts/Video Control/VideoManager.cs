using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VideoControl{
    using UnityEngine.Events;
    
    public class VideoManager : MonoBehaviour
    {
        static VideoManager s_instance;
        public static VideoManager instance{
            get{
                if(s_instance == null){
                    var obj = FindObjectOfType<VideoManager>();
                    if(obj == null){
                        s_instance = new VideoManager();
                    }
                    else{
                        s_instance = obj;
                    }
                }
                return s_instance;
            }
        }
        
        public Transform m_starPoint;
        public float m_endClipDelay;
        public float m_prepareDelay = 0;

        [Space]
        private List<VideoWithEvents> m_videoEvents;
        
        private VideoPlayerController m_current;
        
        private Vector3 m_originalPosition;

        private bool m_isPlaying;
        public bool isPlaying{ get {return m_isPlaying;}}

        public List<VideoPlayerController> m_prepareThese = new List<VideoPlayerController>();

        private Mouledoux.Components.Mediator.Subscriptions m_subs = new Mouledoux.Components.Mediator.Subscriptions();

        private void Awake() {
            if(instance != this){
                Destroy(this);
                return;
            }

            m_originalPosition = m_starPoint.position;
        }

        private void OnDestroy() {
            m_subs.UnsubscribeAll();
        }

        public void SetList(List<VideoWithEvents> newEvents){
            m_videoEvents = newEvents;
        }

        // Start is called before the first frame update
        IEnumerator Start()
        {
            m_subs.Subscribe("startreel", StartVideo);
            yield return new WaitForSeconds(m_prepareDelay);
            StartCoroutine(PrepareVideos());
        }

        public void StartVideo(object[] obj){
            StartVideo();
        }

        [ContextMenu("Start Video")]
        public void StartVideo(){
            if(m_isPlaying){
                StopVideo();
            }
            StartCoroutine(PlayVideos());
            Debug.Log("Starting reel.");
        }

        [ContextMenu("Stop Video")]
        public void StopVideo(){
            m_current.m_videoPlayer.Pause();
            StopAllCoroutines();
            m_isPlaying = false;
        }

        private IEnumerator PlayVideos(){
            m_isPlaying = true;
            foreach (VideoWithEvents ve in m_videoEvents)
            {
                if(m_current != null){
                    //Push old Back
                    m_current.transform.position = new Vector3(1000, 1000, 1000);
                }

                //Set new
                m_current = ve.m_videoController;
                m_current.transform.position = m_originalPosition;

                m_current.PlayVideo();

                yield return new WaitUntil(() => m_current.m_videoPlayer.isPlaying);
                yield return new WaitWhile(() => m_current.m_videoPlayer.isPlaying);
                yield return new WaitForSeconds(0.3f);
                if(m_endClipDelay > 0){
                    yield return new WaitForSeconds(m_endClipDelay);
                }

                ve.m_event.Invoke();
                m_current.m_hasPlayed = true;
            }
            m_isPlaying = false;
        } 
    
        private IEnumerator PrepareVideos(){
            foreach(VideoPlayerController vpc in m_prepareThese){
                vpc.Restart();
                yield return new WaitUntil(() => vpc.m_videoPlayer.isPrepared);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }



    [System.Serializable]
    public struct VideoWithEvents{
        public string m_name;
        public VideoPlayerController m_videoController;
        public UnityEvent m_event;
    }
}