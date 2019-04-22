using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Vector3 m_positionOffset;
    public List<Transform> m_followObjects = new List<Transform>();
    private Transform m_target;

    IEnumerator Start()
    {
        while(Application.IsPlaying(this))
        {
            foreach(Transform t in m_followObjects)
            {
                if(t.gameObject.activeInHierarchy)
                {
                    m_target = t;
                    break;
                }
            }
            yield return new WaitWhile(() => m_target != null);
        }

    }

    void Update()
    {
        if(m_target != null)
        {
            transform.position = m_target.position + m_positionOffset;
        }
    }
}
