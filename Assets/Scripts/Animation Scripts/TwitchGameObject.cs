using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchGameObject : MonoBehaviour
{
    public float m_twitchFactor;
    public float m_speed;

    private Quaternion origin;
    private bool m_increasing;
    // Start is called before the first frame update
    void Awake()
    {
        m_increasing = true;
        origin = new Quaternion(transform.localRotation.x,transform.localRotation.y,transform.localRotation.z, transform.localRotation.w);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_increasing)
            transform.Rotate(0,0, m_speed*Time.deltaTime);
        else
            transform.Rotate(0,0, -(m_speed*Time.deltaTime));
        
        if(m_increasing == true && transform.localRotation.z >= origin.z + m_twitchFactor)
            m_increasing=false;
        else if(m_increasing == false && transform.localRotation.z <= origin.z - m_twitchFactor)
            m_increasing=true;
    }
}
