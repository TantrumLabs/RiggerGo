using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VideoControl{
    public class VideoPreparer : MonoBehaviour
    {
        static VideoPreparer s_instance;
        public static VideoPreparer instance{
            get{
                if(s_instance == null){
                    var obj = FindObjectOfType<VideoPreparer>();
                    if(obj == null){
                        s_instance = new VideoPreparer();
                    }
                    else{
                        s_instance = obj;
                    }
                }
                return s_instance;
            }
        }

        private bool m_isOperating;
        public static bool isOperating { get { return instance.m_isOperating; } }
    
        public void PrepareHolders(List<VideoEventHolder> veh){
            instance.StartCoroutine(EnumPrepare(veh));
        }

        public void StartPrepareNodes(List<VideoNode> nodes)
        {
            StartCoroutine(PrepareNodes(nodes));
        }

        private IEnumerator PrepareNodes(List<VideoNode> nodes)
        {
            m_isOperating = true;
            foreach(VideoNode vn in nodes)
            {
                vn.nodeVideo.Restart();

                yield return new WaitUntil(() => vn.nodeVideo.m_videoPlayer.isPrepared == true);
            }
            m_isOperating = false;
        }

        private IEnumerator EnumPrepare(List<VideoEventHolder> veh){
            m_isOperating = true;
            foreach (VideoEventHolder hold in veh){
                if(hold.IsPrepared()){
                    continue;
                }
                
                PrepareClips(hold.videoEvents);
                yield return new WaitUntil(() => hold.IsPrepared());
                //Debug.Log("Clips Loaded");
            }
            m_isOperating = false;
        }

        public void PrepareClips(List<VideoWithEvents> m_videoEvents){
            instance.StartCoroutine(PrepareClipsEnum(m_videoEvents));
        }

        private IEnumerator PrepareClipsEnum(List<VideoWithEvents> m_videoEvents){
            m_isOperating = true;
            foreach (VideoWithEvents ve in m_videoEvents){
                ve.m_videoController.Restart();
                yield return new WaitUntil(() => ve.m_videoController.m_videoPlayer.isPrepared);
            }
            m_isOperating = false;
        }

        public void PrepareController(List<VideoPlayerController> playerController){
            instance.StartCoroutine(EnumPrepareController(playerController));
        }

        private IEnumerator EnumPrepareController(List<VideoPlayerController> playerController){
            m_isOperating = true;
            foreach (VideoPlayerController vpc in playerController){
                vpc.Restart();
                yield return new WaitUntil(() => vpc.m_videoPlayer.isPrepared);
            }
            m_isOperating = false;
        }
    }
}
