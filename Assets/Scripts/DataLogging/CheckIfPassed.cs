using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckIfPassed : MonoBehaviour
{
    public UnityEvent m_notPassed;
    public UnityEvent m_passed;

    private void Start()
    {
        CheckPassLogic();
    }

    public void CheckPassLogic()
    {
        //if (ScoreKeeper.instance.m_hasPassed)
        //    m_passed.Invoke();

        //else
        //    m_notPassed.Invoke();
    }
}
