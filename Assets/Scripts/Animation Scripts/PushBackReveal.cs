using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBackReveal : MonoBehaviour
{
    public float m_newValue;

    public ForceTeleport m_forceTeleport;

    void OnEnable()
    {
        if("Zone (" +m_forceTeleport.currentPoint+")" != name)
            return;

        m_forceTeleport.m_revealTime = m_newValue;
    }

    public void ChangeValue(float f){
        m_newValue = f;
    }

    public void PushBack(){
        m_forceTeleport.m_revealTime = m_newValue;
    }
}
