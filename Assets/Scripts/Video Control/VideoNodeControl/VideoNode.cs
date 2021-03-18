using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace VideoControl{
    [RequireComponent(typeof(VideoPlayerController))]
    public class VideoNode : MonoBehaviour
    {
        [SerializeField]private List<VideoNode> m_nextNodes = new List<VideoNode>();
        public List<VideoNode> nextNodes { get { return m_nextNodes; } }
        public VideoPlayerController nodeVideo { get; private set; }

        private void Awake()
        {
            nodeVideo = GetComponent<VideoPlayerController>();
        }

        public void PrepareNextClips(){
            if (m_nextNodes.Count == 0)
            {
                return;
            }

            Debug.Log("Loading Next Nodes");
            VideoPreparer.instance.StartPrepareNodes(m_nextNodes);
        }

        public bool IsPrepared(){
            return nodeVideo.m_videoPlayer.isPrepared;
        }
    }
}