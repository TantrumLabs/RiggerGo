using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MironDB
{
    public class MironDB_RegisterUI : MonoBehaviour
    {
        public UnityEngine.UI.InputField m_inputEmail;
        public UnityEngine.UI.InputField m_inputPassword;
        public UnityEngine.UI.InputField m_inputFirstName;
        public UnityEngine.UI.InputField m_inputLastName, m_inputStoreNo;
        
        public UnityEngine.UI.Button m_buttonRegister;

        [Space]
        public UnityEngine.UI.Text m_textReturnMessage;

        void Start()
        {
            InitializeButtons();
        }

        public void InitializeButtons()
        {
             m_buttonRegister.onClick.AddListener(delegate()
            {
                MironDB_Manager.Register(
                m_inputEmail.text, m_inputPassword.text, m_inputFirstName.text, m_inputLastName.text, m_textReturnMessage, m_inputStoreNo.text);
            });
        }

    }
}