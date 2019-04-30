using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HazardObject))]
public class LockerItem : MonoBehaviour
{
    public GameObject m_highlight;

    private HazardObject m_hazardObject;
    private bool m_turnedOn = false;
    public bool turnOn{
        get{return m_turnedOn;}
    }

    void Awake(){
        m_hazardObject = gameObject.GetComponent<HazardObject>();
    }

    public void ToggleLockerItem(){
        m_turnedOn = !m_turnedOn;
        m_highlight.SetActive(m_turnedOn);
    }

    public void ChangeScore(){
        m_hazardObject.m_scoreValue = m_hazardObject.m_scoreValue * -1;
    }
}
