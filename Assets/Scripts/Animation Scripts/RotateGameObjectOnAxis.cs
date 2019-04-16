using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObjectOnAxis : MonoBehaviour
{
    public Vector3 m_speed;
    public GameObject m_RotateHighlight;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(m_speed*Time.deltaTime);
        if(m_RotateHighlight != null)
            m_RotateHighlight.transform.Rotate(m_speed*Time.deltaTime);
    }
}
