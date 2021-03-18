using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VideoControl{
    public class VideoNodeManager : MonoBehaviour
    {
        static VideoNodeManager s_instance;
        public static VideoNodeManager instance
        {
            get
            {
                if (s_instance == null)
                {
                    var obj = FindObjectOfType<VideoNodeManager>();
                    if (obj == null)
                    {
                        s_instance = new VideoNodeManager();
                    }
                    else
                    {
                        s_instance = obj;
                    }
                }
                return s_instance;
            }
        }

        public Transform m_rootPosition;
        public float m_endClipDelay;
        public float m_prepareDelay = 0;
        

        private VideoNode m_current = null;
        private Vector3 m_originalPosition;

        //This is going to be a list of nodes
        public List<VideoNode> m_prepareThese = new List<VideoNode>();
        private Mouledoux.Components.Mediator.Subscriptions m_subs = new Mouledoux.Components.Mediator.Subscriptions();
        private bool m_doneFadingIn;

        private void Awake()
        {
            if(instance != this)
            {
                Destroy(this);
                return;
            }
            m_originalPosition = m_rootPosition.position;
        }

        IEnumerator Start()
        {
            m_subs.Subscribe("fadeindone", delegate { m_doneFadingIn = true; });
            yield return new WaitForSeconds(m_prepareDelay);
            StartCoroutine(PrepareVideos());
        }

        private IEnumerator PrepareVideos()
        {
            foreach (VideoNode vn in m_prepareThese)
            {
                vn.nodeVideo.Restart();
                yield return new WaitUntil(() => vn.nodeVideo.m_videoPlayer.isPrepared);
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void PlayThisNode(VideoNode toPlay)
        {
            StartCoroutine(PlayThisNodeEnum(toPlay));
        }

        public void StartLongLoad(VideoNode toPlay)
        {
            StartCoroutine(PlayThisNodeLongEnum(toPlay));
        }

        public IEnumerator PlayThisNodeEnum(VideoNode toPlay)
        {
            m_doneFadingIn = false;
            Mouledoux.Components.Mediator.instance.NotifySubscribers("startfade");

            var list = new List<VideoPlayerController>();
            foreach (VideoNode vn in FindObjectsOfType<VideoNode>())
            {
                bool isprepared = vn.IsPrepared();

                if (isprepared && !toPlay.nextNodes.Contains(vn) && vn != toPlay && vn != m_current)
                {
                    Debug.Log($"Stopping {vn.name}...");
                    vn.nodeVideo.m_videoPlayer.Stop();
                }

                else if (!isprepared && toPlay.nextNodes.Contains(vn))
                {
                    Debug.Log($"Preparing {vn.name}...");
                    list.Add(vn.nodeVideo);
                }
            }

            if (!toPlay.IsPrepared())
                list.Add(toPlay.nodeVideo);

            if (list.Count > 0)
                VideoPreparer.instance.PrepareController(list);

            yield return new WaitWhile(() => m_doneFadingIn == false);

            if (m_current != null)
            {
                m_current.transform.position = new Vector3(1000, 1000, 1000);
                m_current.nodeVideo.m_videoPlayer.Stop();
                m_current.nodeVideo.m_videoPlayer.time = 0;
            }

            m_current = toPlay;

            m_current.transform.position = m_originalPosition;

            m_current.nodeVideo.PlayVideo();
        }

        public IEnumerator PlayThisNodeLongEnum(VideoNode toPlay)
        {
            Mouledoux.Components.Mediator.instance.NotifySubscribers("startpreparing");

            var list = new List<VideoPlayerController>();
            foreach (VideoNode vn in FindObjectsOfType<VideoNode>())
            {
                bool isprepared = vn.IsPrepared();

                if (isprepared && !toPlay.nextNodes.Contains(vn) && vn != toPlay)
                {
                    Debug.Log($"Stopping {vn.name}...");
                    vn.nodeVideo.m_videoPlayer.Stop();
                }

                else if (!isprepared && toPlay.nextNodes.Contains(vn))
                {
                    Debug.Log($"Preparing {vn.name}...");
                    list.Add(vn.nodeVideo);
                }
            }

            if (!toPlay.IsPrepared())
                list.Add(toPlay.nodeVideo);

            if (list.Count > 0)
                VideoPreparer.instance.PrepareController(list);

            yield return new WaitWhile(() => VideoPreparer.isOperating == true);

            Mouledoux.Components.Mediator.instance.NotifySubscribers("donepreparing");

            if (m_current != null)
            {
                m_current.transform.position = new Vector3(1000, 1000, 1000);
                m_current.nodeVideo.m_videoPlayer.Stop();
                m_current.nodeVideo.m_videoPlayer.time = 0;
            }

            m_current = toPlay;

            m_current.transform.position = m_originalPosition;

            m_current.nodeVideo.PlayVideo();
        }
    }
}