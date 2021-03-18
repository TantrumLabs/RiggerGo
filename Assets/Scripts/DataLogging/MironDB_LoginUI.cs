using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MironDB
{
    public class MironDB_LoginUI : MonoBehaviour
    {
        [SerializeField]private bool m_webBuild = false;

        [SerializeField] private string m_demoUsername;
        [SerializeField] private string m_demoPassword;

		public TMP_InputField m_inputEmail;
		public TMP_InputField m_inputPassword;
        public TMP_InputField m_firstName;
		public TMP_InputField m_lastName;
        public TMP_InputField m_storeNumber;
        public TMP_InputField m_codeField;
        
        public UnityEngine.UI.Button m_buttonLogin;
        public UnityEngine.UI.Button m_buttonForgotPassword;
        public UnityEngine.UI.Button m_buttonRegister;

        [Space]
        public TextMeshProUGUI m_textReturnMessage;

        [Space]
        public UnityEngine.Events.UnityEvent OnSuccess;

        private Mouledoux.Components.Mediator.Subscriptions m_subs = new Mouledoux.Components.Mediator.Subscriptions();
        private bool m_tryingLogin = false;
        public DelayEventOnStart m_restart;
        private string m_emailcode;

        public UnityEngine.UI.Button m_codeButton;

        void Start()
        {
            m_emailcode = "";

            while(m_emailcode.Length < 6)
            {
                int asciiIndex = UnityEngine.Random.Range(48, 57);

                if(asciiIndex >= 58 && asciiIndex <= 96)
                {
                    continue;
                }

                m_emailcode += (char)asciiIndex;
            }

            InitializeButtons();
            m_subs.Subscribe("clearerrormessage", ClearMessage);
        }

        void FixedUpdate()
        {
            if(MironDB_Manager.statusReturn == null) return;

            if(MironDB_Manager.statusReturn.error_description == null)
            {
                return;
            }

            //m_textReturnMessage.text = (!MironDB_Manager.statusReturn.error_description.Contains("...")) ? MironDB_Manager.statusReturn.error_description : m_textReturnMessage.text;
        }

        public void InitializeButtons()
        {
            if(m_buttonLogin != null)
            {
                m_buttonLogin.onClick.AddListener(delegate()
                {
                    StartCoroutine(TryLoginRegister(m_textReturnMessage));
                });

                m_restart.m_action.AddListener(delegate ()
                {
                    StartCoroutine(TryLoginRegister(m_textReturnMessage));
                });
            }

            if(m_buttonForgotPassword != null)
            {
                m_buttonForgotPassword.onClick.AddListener(delegate()
                {
                    MironDB_Manager.ForgotPassword(m_inputEmail.text);
                });
            }

            if(m_codeButton != null)
            {
                m_codeButton.onClick.AddListener(delegate() 
                {
                    CheckCode();
                });
            }
            
        }

        IEnumerator LoginCheck()
        {
            m_tryingLogin = true;
            m_buttonLogin.interactable = false;

            //Offline code----
            if(m_inputEmail.text == m_demoUsername && m_inputPassword.text == m_demoPassword){
                OnSuccess.Invoke();
                yield break;
            }
            //----------------

            StartCoroutine(CheckOnKeyChecker());
            MironDB_Manager.statusReturn = null;

            MironDB_Manager.Login(
                m_inputEmail.text, m_inputPassword.text);
        
            yield return new WaitWhile(() => MironDB_Manager.statusReturn == null);

            

            if(MironDB_Manager.statusReturn.status.Contains("ok"))
            {
                OnSuccess.Invoke();
                MironDB_Manager.statusReturn = null;
                yield break;
            }

            // else
            // {
            //     MironDB_Manager.Logout();
            // }
            m_buttonLogin.interactable = true;
            m_tryingLogin = false;
        }

        private IEnumerator CheckOnKeyChecker(){
            yield return new WaitForSeconds(3f);

            if(MironDB.MironDB_Manager.statusReturn == null){
                m_textReturnMessage.text = "User/password is incorrect.";
                MironDB_Manager.instance.StopAllCoroutines();
                StopAllCoroutines();
            }
        }

        private void ClearMessage(object[] obj){
            m_textReturnMessage.text = "";
        }

        private IEnumerator TryLoginRegister(TMPro.TextMeshProUGUI m_textReturnMessage){
            m_textReturnMessage.text = "";
            m_buttonLogin.interactable = false;

            //Offline code----
            if(m_inputEmail.text == m_demoUsername && m_inputPassword.text == m_demoPassword){
                OnSuccess.Invoke();
                yield break;
            }
            //----------------

            if(!m_inputEmail.text.Contains("@"))
            {
                m_textReturnMessage.text = "Invalid Email!";
                m_buttonLogin.interactable = true;
                yield break;
            }

            else if(m_inputPassword.text.Length < 1){
                m_textReturnMessage.text = "Fill in a password!";
                m_buttonLogin.interactable = true;
                yield break;
            }

            MironDB.MironDB_Manager.statusReturn = null;
            MironDB.MironDB_Manager.Login(m_inputEmail.text, m_inputPassword.text);

            yield return new WaitForSeconds(.5f);
            yield return new WaitWhile(() => MironDB.MironDB_Manager.statusReturn == null);

            if(MironDB.MironDB_Manager.statusReturn.status == "ok"){
                if(m_webBuild)
                {
                    TryEmail();
                    SwitchToCode();
                }

                else
                {
                    OnSuccess.Invoke();
                }
                yield break;
            }

            else
            {
                m_textReturnMessage.text = "";
                //MironDB_Manager.Logout();
                yield return new WaitForSeconds(1f);

            }

            MironDB.MironDB_Manager.statusReturn = null;

            //If Login Failed, try to register

            if(m_firstName.text.Length < 1 || m_lastName.text.Length < 1){
                m_textReturnMessage.text = "Please fill in a first & last name!";
                m_buttonLogin.interactable = true;
                yield break;
            }

            else if(m_storeNumber.text.Length < 1){
                m_textReturnMessage.text = "Please Enter a store number!";
                m_buttonLogin.interactable = true;
                yield break;
            }

            else
            {
                if(m_firstName.text[0] >= 97 && m_firstName.text[0] <= 122){
                    string newFirstName = string.Empty;

                    for(int i = 0; i < m_firstName.text.Length; i++){
                        if(i == 0){
                            newFirstName += (char)((int)m_firstName.text[0] - 32);
                        }

                        else{
                            newFirstName += m_firstName.text[i];
                        }
                    }

                    m_firstName.text = newFirstName;
                }

                if(m_lastName.text[0] >= 97 && m_lastName.text[0] <= 122){
                    string newFirstName = string.Empty;

                    for(int i = 0; i < m_lastName.text.Length; i++){
                        if(i == 0){
                            newFirstName += (char)((int)m_lastName.text[0] - 32);
                        }

                        else{
                            newFirstName += m_lastName.text[i];
                        }
                    }

                    m_lastName.text = newFirstName;
                }

                Debug.Log("Register Start");

                MironDB.MironDB_Manager.statusReturn = null;
                MironDB.MironDB_Manager.Register(m_inputEmail.text, m_inputPassword.text, m_firstName.text, m_lastName.text, null, m_storeNumber.text);

                yield return new WaitUntil(() => MironDB.MironDB_Manager.m_operating == true);
                yield return new WaitUntil(() => MironDB.MironDB_Manager.m_operating == false);
                
                //If Register is successful
                if(MironDB.MironDB_Manager.statusReturn.status == "ok")
                {
                    Debug.Log("Register Complete");
                    StartCoroutine(TryLoginRegister(m_textReturnMessage));
                    yield break;
                }

                //If Register Fail?
                else
                {
                    m_textReturnMessage.text = MironDB.MironDB_Manager.statusReturn.error_description;
                    m_buttonLogin.interactable = true;
                }
            }
        }

        public void TryEmail()
        {
            ////Used for WebGL
            //string from, to, subject, body, smtp, user, password;

            //from = "donotreply@gmail.com";
            //to = /*m_inputEmail.text*/"tony@tantrumlab.com";
            //subject = "Application Entry Code";
            //body = $"This is your code to continue:\n {m_emailcode}";

            //smtp = "smtp.gmail.com";
            //user = "tantrumlab001@gmail.com";
            //password = "T@ntrum00187";

            //Email.SendEmail(from, to, subject, body, smtp, user, password);

            //Works on PC
            //try
            //{
            //    SmtpMail oMail = new SmtpMail("TryIt");

            //    // Set sender email address, please change it to yours
            //    oMail.From = "admin@tantrumlab.com";
            //    // Set recipient email address, please change it to yours
            //    oMail.To = "tony@tantrumlab.com";

            //    // Set email subject
            //    oMail.Subject = "test email from c# project";
            //    // Set email body
            //    oMail.TextBody = "this is a test email sent from c# project, do not reply";

            //    // SMTP server address
            //    SmtpServer oServer = new SmtpServer("smtp.gmail.com");

            //    // User and password for ESMTP authentication
            //    oServer.User = "tantrumlab001@gmail.com";
            //    oServer.Password = "T@ntrum00187";

            //    // Most mordern SMTP servers require SSL/TLS connection now.
            //    // ConnectTryTLS means if server supports SSL/TLS, SSL/TLS will be used automatically.
            //    oServer.ConnectType = SmtpConnectType.ConnectTryTLS;

            //    // If your SMTP server uses 587 port
            //    oServer.Port = 587;

            //    // If your SMTP server requires SSL/TLS connection on 25/587/465 port
            //    // oServer.Port = 25; // 25 or 587 or 465
            //    // oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

            //    Debug.Log("start to send email ...");

            //    SmtpClient oSmtp = new SmtpClient();
            //    oSmtp.SendMail(oServer, oMail);

            //    Debug.Log("email was sent successfully!");
            //}
            //catch (Exception ep)
            //{
            //    Debug.LogError(ep.Message);
            //}
        }

        public void SwitchToCode()
        {
            //Turn off old shit
            m_inputEmail.gameObject.SetActive(false);
            m_inputPassword.gameObject.SetActive(false);
            m_buttonLogin.gameObject.SetActive(false);
            m_storeNumber.gameObject.SetActive(false);
            m_firstName.gameObject.SetActive(false);
            m_lastName.gameObject.SetActive(false);

            //Turn on new shit
            m_codeField.gameObject.SetActive(true);
            m_codeButton.gameObject.SetActive(true);
        }

        public void CheckCode()
        {
            if(m_codeField.text == m_emailcode.ToString())
            {
                Debug.Log("Code is the same");
                OnSuccess.Invoke();
            }

            else
            {
                m_textReturnMessage.text = "Code not the same.";
            }
        }
    }
}