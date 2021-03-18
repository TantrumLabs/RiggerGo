using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaretShower : MonoBehaviour
{
    public float m_caretBlinkRate;

    private TMP_InputField m_currentField;
    private bool m_caretOn = false;

    Mouledoux.Components.Mediator.Subscriptions m_subs = new Mouledoux.Components.Mediator.Subscriptions();

    private void Start() {
        StartCoroutine(CaretShowerEnumerator());
    }

    public void ChangeCurrentField(TMP_InputField p_newField){
        if(m_currentField != null){
            if(m_currentField.text.Length >= 1 && m_currentField.text[m_currentField.text.Length - 1] == '|'){
                m_currentField.text = m_currentField.text.Remove(m_currentField.text.Length -1);
            }

            //If used in company section redesign this line
            m_currentField.onValueChanged.RemoveAllListeners();
        }

        m_currentField = p_newField;

        m_currentField.onValueChanged.AddListener(delegate{OnValueChange();});
    }

    private IEnumerator CaretShowerEnumerator(){
        while(true){
            if(m_currentField == null){
                yield return null;
            }

            else{
                if(m_caretOn){
                    m_currentField.text = m_currentField.text.Remove(m_currentField.text.Length - 1);
                    m_caretOn = false;
                }

                else{
                    m_currentField.text += "|";
                    m_caretOn = true;
                }

                yield return new WaitForSeconds(m_caretBlinkRate);
            }
        }
    }

    private void OnValueChange(){
        string text = m_currentField.text;

        if(text.Length >= 1){
            if(text[text.Length - 1] == '|' || text[text.Length - 2] == '|'){
                text = text.Remove(text.Length - 1);
            }
        }
        

        text += "|";
        m_caretOn = true;

        m_currentField.text = text;
    }
}
