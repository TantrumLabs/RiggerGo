using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldVerificationPassword : InputFieldVerification
{
	[SerializeField]
	private string m_password;

	new protected void Start()
	{
		foreach (UnityEngine.UI.InputField i in m_fieldsToCheck)
        {
            i.onValueChanged.AddListener(delegate { VerifyFields(); });
        }
	}

	new public void VerifyFields()
	{
		foreach (UnityEngine.UI.InputField i in m_fieldsToCheck)
        {
            if (i.text == m_password)
            {
                if (!hasValidated) onVerify.Invoke();
                hasValidated = true;
                return;
            }
        }

        hasValidated = false;
        offVerify.Invoke();
	}
}