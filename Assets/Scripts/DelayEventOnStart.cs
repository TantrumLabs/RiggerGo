using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayEventOnStart : MonoBehaviour
{
    public float m_delay;

    [SerializeField]
    private bool m_onStart, m_selfDestruct;

    public UnityEngine.Events.UnityEvent m_action;



    private void Start()
    {
        if(m_selfDestruct) m_action.AddListener(delegate{Destroy(this);});
        if(m_onStart) BeginCountDown();
    }

    public void BeginCountDown()
    {
        StartCoroutine(_BeginCountDown());
    }

	IEnumerator _BeginCountDown()
    {
		yield return new WaitForSeconds(m_delay);
        m_action.Invoke();
	}
}
