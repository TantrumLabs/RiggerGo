using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.InputField))]
public class VRKeyboardInput : MonoBehaviour
{
    [SerializeField]
    private string m_keyboardID;
    public string keyboardID
    {
        get {return m_keyboardID;}

        set
        {
            m_keyboardID = value;
            m_subscriptions.UnsubscribeAll();

            AppendText = null;
            AppendText += AddText;
            m_subscriptions.Subscribe($"vrkeyboard:{m_keyboardID}", AppendText);
        }
    }

    private UnityEngine.UI.InputField m_inputField;
    private DelayEventOnStart m_delayEventOnStart;
    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions =
        new Mouledoux.Components.Mediator.Subscriptions();
    private Mouledoux.Callback.Callback AppendText;

    private string email = "";

    private string password = "";

    private string first = "";

    private string last = "";

    // Start is called before the first frame update
    void Start()
    {
        keyboardID = m_keyboardID;
        m_inputField = GetComponent<UnityEngine.UI.InputField>();
        m_delayEventOnStart = GetComponent<DelayEventOnStart>();
    }

    void Update()
    {
        //m_inputField.text = OVRManager.display.acceleration.ToString();
    }

    public void AddText(object[] args)
    {
        if(args[0].GetType() == typeof(VRKeyboardKey))
        {
            VRKeyboardKey key = (VRKeyboardKey)args[0];

            if(key.GetMainKey() == ".com"){
                m_inputField.text += ".com";
                return;
            }         

            switch(key.GetMainKey().ToLower())
            {
                case "bks":
                    m_inputField.text = m_inputField.text.Remove(m_inputField.text.Length - 1);
                    break;

                case "space":
                    m_inputField.text += " ";
                    break;

                case "shift":
                    break;

                default:
                    m_inputField.text += key.GetCurrentKey();
                    break;
            }
        }
    }

    public void ChangeAllKeyID(string s){
        var keys = FindObjectsOfType<VRKeyboardKey>();

        foreach(VRKeyboardKey k in keys)
        {
            k.Initialize(s, k.GetMainKey());
        }
    }

    public void SendInformation(string s){
        Mouledoux.Components.Mediator.instance.NotifySubscribers(s, new object[] {m_inputField.textComponent});
    }

    private void OnDestroy()
    {
        m_subscriptions.UnsubscribeAll();
    }

    public bool CheckEmailStructure(UnityEngine.UI.InputField otherInput){
        bool stop = true;


        foreach(char c in m_inputField.text)
        {
            if(c == '@'){
                stop = false;
                break;
            }
        }
        
        if(stop)
            return false;

        if(otherInput.text == null)
            return false;
        
        return true;
    }

    public bool CheckToSeeEmpty(UnityEngine.UI.InputField otherInput){
        bool contains = false;

        if(m_inputField.text != "" && otherInput.text != "")
            contains = true;
        
        if(!contains)
            return false;

        return true;
    }

    public void CheckLoginStructure(UnityEngine.UI.InputField otherInput){

        if(CheckEmailStructure(otherInput))
            Mouledoux.Components.Mediator.instance.NotifySubscribers("loginTry", new object[]{m_inputField.text, otherInput.text, m_delayEventOnStart});
    }

    public void StartRegeristry(TMPro.TextMeshProUGUI board){
        StartCoroutine(CheckRegerstration(board));
    }

    public void StartLogin(TMPro.TextMeshProUGUI board){
        StartCoroutine(CheckLogin(board));
    }

    private IEnumerator CheckRegerstration(TMPro.TextMeshProUGUI board){
        if(email.Length < 1 || password.Length < 1 || name.Length < 1 || last.Length < 1){
            if(!email.Contains("@") || email.Length < 2)
            {
                board.text = "Invalid Email!";
            }

            else if(password.Length < 1){
                board.text = "Fill in a password!";
            }

            else if(first.Length < 1){
                board.text = "Fill in a first name!";
            }

            else if(last.Length < 1){
                board.text = "Fill in a last!";
            }

            yield break;
        }

        MironDB.MironDB_Manager.Register(email, password, first, last, null);

        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => MironDB.MironDB_Manager.m_operating == false);
        
        if(MironDB.MironDB_Manager.statusReturn.status == "ok")
            board.text = "Registration Complete! Go Login!";

        else{
            board.text = "Error:\n" + MironDB.MironDB_Manager.statusReturn.error_description;
        }
    }

    private IEnumerator CheckLogin(TMPro.TextMeshProUGUI board){
        if(email.Length < 1 || password.Length < 1){
            if(!email.Contains("@") || email.Length < 2)
            {
                board.text = "Invalid Email!";
            }

            else if(password.Length < 1){
                board.text = "Fill in a password!";
            }

            yield break;
        }

        MironDB.MironDB_Manager.Login(email, password, null);

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => MironDB.MironDB_Manager.m_operating == false);

        if(MironDB.MironDB_Manager.statusReturn.status == "ok"){
            m_delayEventOnStart.m_action.Invoke();
            MironDB.MironDB_Manager.StartTest(MironDB_TestManager.instance.testScenarioID);
        }

        else{
            board.text = "Error:\n" + MironDB.MironDB_Manager.statusReturn.error_description;
        }
    }

    public void SetEmail(UnityEngine.UI.InputField field){
        if(field.text.Contains("@"))
            email = field.text;
    }

    public void SetPassword(UnityEngine.UI.InputField field){
        if(field.text.Length >= 1)
            password = field.text;
    }

    public void SetFirstName(UnityEngine.UI.InputField field){
        if(field.text.Length >= 1)
            first = field.text;
    }

    public void SetLastName(UnityEngine.UI.InputField field){
        if(field.text.Length >= 1)
            last = field.text;
    }
}
