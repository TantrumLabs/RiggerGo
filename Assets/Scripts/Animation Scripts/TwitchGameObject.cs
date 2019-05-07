using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchGameObject : MonoBehaviour
{
    public float m_twitchFactor;
    public float m_speed;

    private Vector3 origin;
    private bool m_increasing;
    // Start is called before the first frame update
    void Awake()
    {
        m_increasing = true;
        origin = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
        transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_increasing)
            transform.Rotate(0,0, m_speed*Time.deltaTime);
        else
            transform.Rotate(0,0, -(m_speed*Time.deltaTime));
        
        if(m_increasing == true && transform.eulerAngles.z <= 
            180 && transform.eulerAngles.z >= origin.z + m_twitchFactor)
            m_increasing=false;
        else if(m_increasing == false && transform.eulerAngles.z >= 
            180 && transform.eulerAngles.z <= 360 - m_twitchFactor)
            m_increasing=true;
    }

    public void OnDestroy(){
        transform.rotation = new Quaternion();
        Destroy(this);
    }
}
