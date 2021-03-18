using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionGame : MonoBehaviour
{
    public List<SelectionItem> m_selectionItems;
    public int m_currentScore { get; private set; } = 0;
    [Space]
    public UnityEvent m_onPass, m_onFail;

    public void CheckSelection()
    {
        var passed = true;

        foreach(SelectionItem item in m_selectionItems)
        {
            if (item.wasClicked)
            {
                m_currentScore += item.m_score;
            }

            if (passed && item.wasClicked == false)
            {
                passed = false;
            }
        }

        if (passed)
            m_onPass.Invoke();
        else
            m_onFail.Invoke();
    }

    public void AddToCurrentScore(int added)
    {
        m_currentScore += added;
    }
}
