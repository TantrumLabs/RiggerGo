using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldDuplicator : MonoBehaviour
{
    public TMPro.TMP_InputField m_Show;
    public TMPro.TMP_InputField m_Keeper;
    public bool m_hideText = false;
    [Space]
    public bool m_textCursorOn;
    public float m_tickFrequencey = 1;
    private float m_timeLeft;
    Mouledoux.Components.Mediator.Subscriptions subscriptions = new Mouledoux.Components.Mediator.Subscriptions();

    private void Awake() {
        subscriptions.Subscribe("turnoffcarrot", delegate { m_textCursorOn = false; m_timeLeft = m_tickFrequencey; });
        m_Keeper.onValueChanged.AddListener(delegate{OnValueChange();});
        m_timeLeft = m_tickFrequencey;
    }

    public void TurnOnCarrot()
    {
        m_timeLeft = m_tickFrequencey;
        m_textCursorOn = true;
    }

    private void Update()
    {
        //If Selected
        if(m_textCursorOn)
        {
            if(m_timeLeft < 0)
            {
                if(m_Show.text.Contains("|"))
                {
                    GetRidOfCarret();
                }

                else
                {
                    m_Show.text += "|";
                }

                m_timeLeft = m_tickFrequencey;
            }

            else
            {
                m_timeLeft -= Time.deltaTime;
            }
        }

        //If not ticking check for carrot and rid it.
        else
        {
            GetRidOfCarret();
        }
    }

    private void OnValueChange(){
        m_Show.text = m_Keeper.text + "|";
    }

    private void GetRidOfCarret()
    {
        if (m_Show.text.Contains("|"))
        {
            var strings = m_Show.text.Split('|');
            string endResult = string.Empty;
            foreach (string s in strings)
            {
                endResult += s;
            }
            m_Show.text = endResult;
        }
    }
}
