using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTeller : MonoBehaviour
{
    public ForceTeleport m_forceTeleport;
    public TMPro.TextMeshProUGUI m_text;

    void Start()
    {
        m_text.text = "Zone " + m_forceTeleport.currentPoint;
    }

    void Update()
    {
        m_text.text = "Zone " + m_forceTeleport.currentPoint;
    }
}
