using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace VideoControl{
    public class VideoPlayerController : MonoBehaviour
    {
        [HideInInspector]public VideoPlayer m_videoPlayer;
        [HideInInspector]public AudioSource m_audioSource;
        [HideInInspector]public bool m_hasPlayed = false;

        private void Awake() {
            m_videoPlayer = GetComponent<VideoPlayer>();

            m_audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        private IEnumerator PrepareCorutine() {
            m_videoPlayer.Prepare();
            
            yield return new WaitUntil(() => m_videoPlayer.isPrepared);

            Debug.Log("Video Prepared");
        }

        [ContextMenu("Restart Video Player")]
        public void Restart(){
            m_videoPlayer.Stop();

            StartCoroutine(PrepareCorutine());
        }

        [ContextMenu("Play Video")]
        public void PlayVideo(){
            m_videoPlayer.Play();
        }

        private void OnDisable() {

            if(m_videoPlayer != null)
                Destroy(m_videoPlayer.gameObject);
        }
    }
}