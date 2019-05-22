using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingManager : MonoBehaviour
{
    public List<SlingBehaviors> m_slings = new List<SlingBehaviors>();

    private SlingBehaviors m_currentSling = null;

    private Mouledoux.Callback.Callback m_onChangeCurrent;

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions =
     new Mouledoux.Components.Mediator.Subscriptions();

    void Awake(){
        m_onChangeCurrent += ChangeCurrent;

        m_subscriptions.Subscribe("newslingactive", m_onChangeCurrent);
    }

    public void ChangeCurrent(object[] objArray){
        if(objArray[0] == null)
            m_currentSling = null;
        else{
            if(m_currentSling != null)
                m_currentSling.SetBack();

            if(m_currentSling == objArray[0] as SlingBehaviors)
                {
                    m_currentSling = null;
                    return;
                }
            m_currentSling = objArray[0] as SlingBehaviors;
        }
    }

    private void OnDestroy() {
        m_subscriptions.UnsubscribeAll();
    }
}
