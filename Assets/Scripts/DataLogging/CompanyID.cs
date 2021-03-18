using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompanyID : MonoBehaviour
{
    [SerializeField] private string m_demoCompanyCode;

    public UnityEngine.UI.Button submitButton;
    public TMP_InputField idInput;
    public TextMeshProUGUI returnMesage;

    public UnityEngine.Events.UnityEvent onAccept;

    private int m_notAuthorized = 0;

    private void Start()
    {
        submitButton.onClick.AddListener(delegate(){StartCoroutine(CheckCompanyKey());});
    }

    IEnumerator CheckCompanyKey()
    {
        submitButton.interactable = false;
        returnMesage.text = string.Empty;
        MironDB.MironDB_Manager.statusReturn = null;
        m_notAuthorized = 0;

        if(idInput.text == m_demoCompanyCode){
            SaveCompanyID();
            onAccept.Invoke();
            yield break;
        }

        StartCoroutine(CheckSerialNumber());

        yield return new WaitWhile(() => m_notAuthorized == 0);

        if(m_notAuthorized == 1)
        {
            returnMesage.text = "Headset is not authorized.";
            yield break;
        }
        MironDB.MironDB_Manager.statusReturn = null;
        StartCoroutine(InternetChecker());

        MironDB.MironDB_Manager.CheckKey(idInput.text);
        yield return new WaitWhile(() => MironDB.MironDB_Manager.statusReturn == null);

        if(MironDB.MironDB_Manager.statusReturn.status == "ok")
        {
            SaveCompanyID();
            onAccept.Invoke();
        }

        else
        {
            returnMesage.text = MironDB.MironDB_Manager.statusReturn.error_description.Split(new char[]{'.'})[0];
        }

        submitButton.interactable = true;

    }

    public void StartCheckKey(){
        StartCoroutine(CheckCompanyKey());
    }

    public void SaveCompanyID()
    {
        MironDB.MironDB_Manager.machineID = idInput.text;
    }

    private IEnumerator InternetChecker() //Time out Area. Will activate if it takes too long to connect to the internet
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No Internet");
            returnMesage.text = "Cannot connect to to internet. Check your headset's Wifi settings.";
            submitButton.interactable = true;
            yield break;
        }

        else
            Debug.Log("Internet");

        yield return new WaitForSeconds(4f);

        if(MironDB.MironDB_Manager.statusReturn == null){
            returnMesage.text = "Cannot connect to the Database.";
            MironDB.MironDB_Manager.instance.StopAllCoroutines();
            this.StopAllCoroutines();
            submitButton.interactable = true;
        }
    }

    private string m_seriaNumber;

    IEnumerator CheckSerialNumber()
    {
        //m_notAuthorized = 2;
        //MironDB.MironDB_Manager.statusReturn = null;
        //yield break;

        if (Application.isEditor)
        {
            m_notAuthorized = 2;
            MironDB.MironDB_Manager.statusReturn = null;
            yield break;
        }

        var jo = new AndroidJavaObject("android.os.Build");

        m_seriaNumber = jo.GetStatic<string>("SERIAL");

        MironDB.MironDB_Manager.statusReturn = null;
        MironDB.MironDB_Manager.CheckKey(m_seriaNumber);

        yield return new WaitWhile(() => MironDB.MironDB_Manager.statusReturn == null);

        if(MironDB.MironDB_Manager.statusReturn.status != "ok"){
            m_notAuthorized = 1;
            returnMesage.text = "Headset not an authorized Headset.";
            MironDB.MironDB_Manager.instance.StopAllCoroutines();
            this.StopAllCoroutines();
            submitButton.interactable = true;
        }

        else
        {
            m_notAuthorized = 2;
            MironDB.MironDB_Manager.statusReturn = null;
        }
    }
}