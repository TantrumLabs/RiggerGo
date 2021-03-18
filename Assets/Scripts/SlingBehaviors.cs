using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingBehaviors : MonoBehaviour
{
    public GameObject m_wrongHighlight;
    public GameObject m_rightHighlight;

    public Vector3 m_newPosition;
    public Vector3 m_newRotation;

    public GameObject m_questionGameObject;

    private Vector3 m_oldPosition;
    private Quaternion m_oldRotation;
    private Mouledoux.Components.Mediator.Subscriptions subscriptions = new Mouledoux.Components.Mediator.Subscriptions();

    void Awake(){
        m_oldPosition = transform.localPosition;
        m_oldRotation = transform.localRotation;
    }

    private void Start()
    {
        subscriptions.Subscribe("brandnewsling", DeactivateSelf);
    }

    public void Activate(){
        transform.localPosition = m_newPosition;
        transform.Rotate(m_newRotation);

        m_questionGameObject.SetActive(true);
        gameObject.GetComponent<HazardObject>().enabled = false;
        object[] objarray = {this};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("newslingactive", objarray);
        Mouledoux.Components.Mediator.instance.NotifySubscribers("brandnewsling", objarray);

    }

    public void SetBack(){
        transform.localPosition = m_oldPosition;
        transform.localRotation = m_oldRotation;

        m_questionGameObject.SetActive(false);
        gameObject.GetComponent<HazardObject>().enabled = true;
    }

    public void Deactivate(){
        transform.localPosition = m_oldPosition;
        transform.localRotation = m_oldRotation;

        m_questionGameObject.SetActive(false);
        gameObject.GetComponent<HazardObject>().enabled = true;
        object[] objarray = {null};
        //Mouledoux.Components.Mediator.instance.NotifySubscribers("newslingactive", objarray);
    }

    private void DeactivateSelf(object[] objArray)
    {
        Debug.Log("Deactivating...");
        var sling = objArray[0] as SlingBehaviors;
        if (sling != this)
        {
            Deactivate();
        }
    }

    public void PickHighlight(bool pass){
        if(pass == true){
            m_rightHighlight.SetActive(true);
            return;
        }
            
        else if(pass == false)
        {
            m_wrongHighlight.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        subscriptions.UnsubscribeAll();
    }
}
