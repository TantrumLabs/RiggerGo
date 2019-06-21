using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanyCode : MonoBehaviour
{
    public float m_speed;

    [SerializeField]
    private Image m_background;

    [SerializeField]
    private Image m_logo;

    [SerializeField]
    private TMPro.TextMeshProUGUI m_text;

    private float m_opacity = 0;
    private float m_previoustime;

    private static CompanyCode s_instance;
    public static CompanyCode instance{
        get{
            if(s_instance == null)
                s_instance = FindObjectOfType<CompanyCode>();
            return s_instance;
        }
    }

    private void Awake() {
        if(instance != this)Destroy(this);

        DontDestroyOnLoad(gameObject);
    }

    public void SetCode(UnityEngine.UI.InputField field){
        MironDB.MironDB_Manager.companyCode = field.text;
        StartCoroutine(Transition());
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
}
