using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public GameObject m_target;
    public bool m_lookAway;
    public bool m_constant;

    // Start is called before the first frame update
    void Start()
    {
        if (m_lookAway)
        {
            transform.LookAt((transform.position - m_target.transform.position) * 2);
        }
        else
        {
            transform.LookAt(m_target.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_constant == false)
            return;

        if (m_lookAway)
        {
            transform.LookAt((transform.position - m_target.transform.position) * 2);
        }
        else
        {
            transform.LookAt(m_target.transform);
        }
    }
}
