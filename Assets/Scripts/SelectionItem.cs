using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionItem : MonoBehaviour
{
    public bool wasClicked { get; private set; } = false;
    public int m_score = 0;

    public void OnClicked()
    {
        wasClicked = true;
    }
}
