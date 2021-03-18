using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTextInField : MonoBehaviour
{
    public bool m_forceToCompanyCode;
    public string m_forcedText;
    private TMPro.TMP_InputField m_textMeshProField;
    private UnityEngine.UI.InputField m_unityField;

    // Start is called before the first frame update
    void Awake()
    {
        m_textMeshProField = gameObject.GetComponent<TMPro.TMP_InputField>();
        m_unityField = gameObject.GetComponent<UnityEngine.UI.InputField>();
        if(m_forceToCompanyCode){
            if(m_textMeshProField != null){
                m_textMeshProField.text = MironDB.MironDB_Manager.machineID;
            }

            if(m_unityField != null){
                m_unityField.text = MironDB.MironDB_Manager.machineID;
            }
            return;
        }

        if(m_textMeshProField != null){
            m_textMeshProField.text = m_forcedText;
        }

        if(m_unityField != null){
            m_unityField.text = m_forcedText;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(m_forceToCompanyCode){
            if(m_textMeshProField != null){
                m_textMeshProField.text = MironDB.MironDB_Manager.machineID;
            }

            if(m_unityField != null){
                m_unityField.text = MironDB.MironDB_Manager.machineID;
            }
            return;
        }

        if(m_textMeshProField != null){
            m_textMeshProField.text = m_forcedText;
        }

        if(m_unityField != null){
            m_unityField.text = m_forcedText;
        }
    }
}
