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

    public void CheckEmailStructure(UnityEngine.UI.InputField otherInput){
        bool stop = true;


        foreach(char c in m_inputField.text)
        {
            if(c == '@'){
                stop = false;
                break;
            }
        }
        
        if(stop)
            return;

        if(otherInput.text == null)
            return;


        Mouledoux.Components.Mediator.instance.NotifySubscribers("loginTry", 
            new object[]{m_inputField.text, otherInput.text, m_delayEventOnStart});
    }

    public void CheckToSeeEmpty(UnityEngine.UI.InputField otherInput){
        bool contains = false;

        if(m_inputField.text != "" && otherInput.text != "")
            contains = true;
        
        if(contains)
            m_delayEventOnStart.m_action.Invoke();
    }
}
