using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOffset : MonoBehaviour
{
    public Transform m_target;

    public bool m_lockX, m_lockY, m_lockZ;

    private Vector3 m_offset;

    void Start()
    {
        m_offset = transform.position - m_target.position;
    }

    void Update()
    {
        var newPos = transform.position;
        if(!m_lockX)
            newPos.x = m_target.position.x + m_offset.x;
        if(!m_lockY)
            newPos.y = m_target.position.y + m_offset.y;
        if(!m_lockZ)
            newPos.z = m_target.position.z + m_offset.z;

        transform.position = newPos;
    }
}
