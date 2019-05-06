using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchGameObjectTranslate : MonoBehaviour
{
    public float m_twitchFactor;
    public Vector3 m_velocity;

    public enum MoveAxis{
        X,Y,Z,
    }

    public MoveAxis m_moveOn;
    private Vector3 origin;
    private bool m_increasing;
    // Start is called before the first frame update
    void Awake()
    {
        m_increasing = true;
        origin = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_increasing)
            transform.Translate(m_velocity.x*Time.deltaTime, m_velocity.y*Time.deltaTime, m_velocity.z*Time.deltaTime);
        else
            transform.Translate(-m_velocity.x*Time.deltaTime,-m_velocity.y*Time.deltaTime, -m_velocity.z*Time.deltaTime);
        
        switch(m_moveOn){
            case MoveAxis.X:

            
                if(m_increasing == true && transform.localPosition.x >= origin.x + m_twitchFactor)
                    m_increasing = false;
                else if(m_increasing == false && transform.localPosition.x <= origin.x - m_twitchFactor)
                    m_increasing = true;
                break;

            case MoveAxis.Y:
                if(m_increasing == true && transform.localPosition.y >= origin.y + m_twitchFactor)
                    m_increasing = false;
                else if(m_increasing == false && transform.localPosition.y <= origin.y - m_twitchFactor)
                    m_increasing = true;
                break;

            case MoveAxis.Z:
                if(m_increasing == true && transform.localPosition.z >= origin.z + m_twitchFactor)
                    m_increasing = false;
                else if(m_increasing == false && transform.localPosition.z <= origin.z - m_twitchFactor)
                    m_increasing = true;
                break;

            default:
                break;
        }
    }

    public void OnDestroy(){
        transform.position = origin;
        Destroy(this);
    }
}
