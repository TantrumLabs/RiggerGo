using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CompanyCode : MonoBehaviour
{
    public float m_speed;
    public TMPro.TextMeshProUGUI m_errorText;

    [SerializeField]
    private Image m_background;

    [SerializeField]
    private Image m_logo;

    [SerializeField]
    private TMPro.TextMeshProUGUI m_text;

    private float m_opacity = 0;
    private float m_previoustime;

    private HazardObject m_button;

    private static CompanyCode s_instance;
    public static CompanyCode instance{
        get{
            if(s_instance == null)
                s_instance = FindObjectOfType<CompanyCode>();
            return s_instance;
        }
    }

    public UnityEvent m_afterLoginEvent;

    private void Awake() {
        if(instance != this)Destroy(this);

        DontDestroyOnLoad(gameObject);
    }

    public void SetCode(UnityEngine.UI.InputField field){
        StartCoroutine(CheckCompanyCode(field));
    }

    public IEnumerator Transition(){
        m_previoustime = Time.time;
        yield return null;

        while(m_opacity < 1){
            m_opacity += (Time.time - m_previoustime)* m_speed;
            
            m_previoustime = Time.time;

            var back = m_background.color;
            var logo = m_logo.color;
            var text = m_text.color;

            back.a += m_opacity;
            logo.a += m_opacity;
            text.a += m_opacity;

            m_background.color = back;
            m_logo.color = logo;
            m_text.color = text;

            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("main_test");
    }

    private IEnumerator CheckCompanyCode(UnityEngine.UI.InputField key){

        string comapyID = key.text;
        MironDB.MironDB_Manager.statusReturn = null;
        
        MironDB.MironDB_Manager.CheckKey(comapyID);

        m_errorText.text = "Validating...";

        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => MironDB.MironDB_Manager.m_operating == false);

        if(MironDB.MironDB_Manager.statusReturn.status == "ok"){
            MironDB.MironDB_Manager.machineID = comapyID;
            m_afterLoginEvent.Invoke();
        }

        else{
            m_button.enabled = true;
            key.text = "";
            m_errorText.text = "Error! Invalid Company code!";
        }
    }

    public void SetButton(HazardObject ho){
        m_button = ho;
    }

    public void StartTransition()
    {
        StartCoroutine(Transition());
    }
}
