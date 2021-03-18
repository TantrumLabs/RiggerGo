using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldVerification : MonoBehaviour
{
    [SerializeField]
    protected List<InputField> m_fieldsToCheck = new List<InputField>();
    protected bool hasValidated = false;

    [Space]

    [SerializeField]
    protected UnityEngine.Events.UnityEvent onVerify, offVerify;



    protected void Start()
    {
        foreach (InputField i in m_fieldsToCheck)
        {
            i.onValueChanged.AddListener(delegate { VerifyFields(); });
        }
    }


    public void VerifyFields()
    {
        foreach (InputField i in m_fieldsToCheck)
        {
            if (i.text == "")
            {
                if (hasValidated) offVerify.Invoke();
                hasValidated = false;
                return;
            }
        }

        hasValidated = true;
        onVerify.Invoke();
    }
}