using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VideoControl
{
    public class VideoPreparerBatch : MonoBehaviour
    {
        public List<VideoPlayerController> m_prepareBatch;

        public void SendBatchToPreparer()
        {
            VideoPreparer.instance.PrepareController(m_prepareBatch);
        }
    }
}