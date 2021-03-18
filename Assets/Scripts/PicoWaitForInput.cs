using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicoWaitForInput : MonoBehaviour
{
    public Pvr_UnitySDKAPI.Pvr_KeyCode m_keyCode;
    public float m_waitTime;
    public UnityEngine.Events.UnityEvent m_onInput;
    
    private float m_tracker;

    private void Start()
    {
        m_tracker = m_waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Pvr_UnitySDKAPI.Controller.UPvr_GetKey(0, m_keyCode))
        {
            if(m_tracker > 0)
            {
                m_tracker -= Time.deltaTime;
            }

            else
            {
                m_onInput.Invoke();
            }
        }

        else
        {
            m_tracker = m_waitTime;
        }
    }
}
