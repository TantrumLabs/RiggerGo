using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace VideoControl{
    public class VideoEventHolder : MonoBehaviour
    {
        [SerializeField]private List<VideoEventHolder> m_nextClips = new List<VideoEventHolder>();
        [SerializeField]private List<VideoWithEvents> m_videoEvents = new List<VideoWithEvents>();

        public List<VideoWithEvents> videoEvents{
            get{
                return m_videoEvents;
            }
        }

        private void Start() {
            PrepareNextClips();
        }

        public void SetManagerPlay(){
            //gameObject.SetActive(true);
            VideoManager.instance.SetList(m_videoEvents);
            VideoManager.instance.StartVideo();
        }

        public void SetManagersList(){
            VideoManager.instance.SetList(m_videoEvents);
        }

        public void PrepareClips(){
            VideoPreparer.instance.PrepareClips(m_videoEvents);
        }

        public void PrepareNextClips(){
            if(m_nextClips.Count == 0){
                return;
            }

            Debug.Log("Loading Next Clips");
            VideoPreparer.instance.PrepareHolders(m_nextClips);
        }

        public bool IsPrepared(){
            if (m_videoEvents.Count == 0)
                return true;

            foreach(VideoWithEvents vwe in m_videoEvents){
                if(!vwe.m_videoController.m_videoPlayer.isPrepared){
                    return false;
                }
            }

            return true;
        }

        private void OnDestroy() {
            foreach(VideoWithEvents ve in m_videoEvents){
                Destroy(ve.m_videoController);
            }

            var saveList = new List<VideoPlayer>();

            foreach(VideoEventHolder veh in m_nextClips){
                bool save = false;
                foreach(VideoWithEvents vwe in veh.m_videoEvents){
                    if (vwe.m_videoController.m_videoPlayer.isPlaying && save == false)
                    {
                        saveList.Add(vwe.m_videoController.m_videoPlayer);
                        Debug.Log($"{vwe.m_videoController.m_videoPlayer.gameObject.name} is being saved");
                        save = true;
                    }

                    else if (save == true)
                    {
                        saveList.Add(vwe.m_videoController.m_videoPlayer);
                        Debug.Log($"{vwe.m_videoController.m_videoPlayer.gameObject.name} is being saved");
                    }
                    
                }
            }

            foreach(VideoPlayer vp in FindObjectsOfType<VideoPlayer>()){
                Debug.Log($"{vp.gameObject.name} is being checked... Prepared: {vp.isPrepared}");

                if (vp.isPrepared && !saveList.Contains(vp)){
                    Debug.Log($"{vp.gameObject.name} is being destroyed");
                    Destroy(vp);
                }
            }
        }
    }
}