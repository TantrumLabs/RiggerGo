using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PasswordShower : MonoBehaviour
{
    private string m_password;

    public string password{
        get {return m_password;}
    }

    public TMPro.TMP_InputField m_passwordShow;
    public TMPro.TMP_InputField m_passwordKeeper;

    private void Awake() {
        m_passwordKeeper.onValueChanged.AddListener(delegate{OnValueChange();});
    }

    public void OnValueChange(){
        string showPass = "";

        for(int i = 0; i < m_passwordKeeper.text.Length; i++){
            if(m_passwordKeeper.text.Length == 1 || i == m_passwordKeeper.text.Length - 1){
                showPass += m_passwordKeeper.text[i];
            }

            else{
                showPass += "*";
            }
        }

        m_passwordShow.text = showPass;
    }


}
