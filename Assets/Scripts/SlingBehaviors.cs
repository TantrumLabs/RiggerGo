using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingBehaviors : MonoBehaviour
{
    public Vector3 m_newPosition;
    public Vector3 m_newRotation;

    public GameObject m_questionGameObject;

    private Vector3 m_oldPosition;
    private Quaternion m_oldRotation;

    void Awake(){
        m_oldPosition = transform.localPosition;
        m_oldRotation = transform.localRotation;
    }

    public void Activate(){
        transform.localPosition = m_newPosition;
        transform.Rotate(m_newRotation);

        m_questionGameObject.SetActive(true);
    }

    public void Deactivate(){
        transform.localPosition = m_oldPosition;
        transform.localRotation = m_oldRotation;

        m_questionGameObject.SetActive(false);
        Destroy(gameObject.GetComponent<HazardObject>());
    }
}
