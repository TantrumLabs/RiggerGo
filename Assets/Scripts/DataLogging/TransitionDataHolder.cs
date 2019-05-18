using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionDataHolder : MonoBehaviour
{
    private static TransitionDataHolder _instance;
    public static TransitionDataHolder instance{
        get{
            if(_instance == null)
                _instance = FindObjectOfType<TransitionDataHolder>();
            return _instance;
        }
    }

    public string m_emailData, m_firstName, m_lastName;

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = 
        new Mouledoux.Components.Mediator.Subscriptions();

    private Mouledoux.Callback.Callback onSetText, onSetFirstName, onSetLastName;

    void Awake(){
        DontDestroyOnLoad(gameObject);

        onSetText = SetEmail;
        onSetFirstName = SetFirstName;
        onSetLastName = SetLastName;

        m_subscriptions.Subscribe("getemail", onSetText);
        m_subscriptions.Subscribe("sendfirstname", onSetFirstName);
        m_subscriptions.Subscribe("sendlasrname", onSetLastName);
    }

    public void SetEmail(object[] obj){
        if(obj[0] as Text == null)
            return;

        var t = obj[0] as Text;
        instance.m_emailData = t.text; 
    }

    public void SetFirstName(object[] obj){
        if(obj[0] as Text == null)
            return;

        var t = obj[0] as Text;
        instance.m_firstName = t.text; 
    }

    public void SetLastName(object[] obj){
        if(obj[0] as Text == null)
            return;

        var t = obj[0] as Text;
        instance.m_lastName = t.text; 
    }

    public void GoToScene(int i){
        SceneManager.LoadSceneAsync(i);
    }

    public void GoToScene(string s){
        SceneManager.LoadSceneAsync(s);
    }
}
