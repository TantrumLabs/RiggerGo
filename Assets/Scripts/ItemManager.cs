using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [SerializeField]private string m_itemName;
    private string m_givenAnswer;
    [SerializeField] private string m_expectedRating;
    private string m_itemScore;
    private string m_actions = string.Empty;



    public GameObject m_decisonRoot;
    public GameObject m_priorityRoot;
    
    [SerializeField] private ToggleGroup m_toggleGroup;
    
    private int m_decision = 0; //1 means delegate, 2 means solve now, 0 means not chosen
    private List<GameObject> m_toggles = new List<GameObject>();

    private void Start()
    {
        foreach (Toggle to in GetComponentsInChildren<Toggle>())
        {
            m_toggles.Add(to.gameObject);
        }
    }

    public void AddToActions(string s)
    {
        m_actions += s + ", ";
    }

    public void CallOutClicked()
    {
        Mouledoux.Components.Mediator.instance.NotifySubscribers($"{m_itemName}clicked");
    }

    public void MakeDesision(int i)
    {
        m_decision = i;
        AddToActions("Decided");
        Mouledoux.Components.Mediator.instance.NotifySubscribers($"{m_itemName}decided");

        if (m_decision == 1)
        {
            Mouledoux.Components.Mediator.instance.NotifySubscribers($"{m_itemName}delegated");
        }

        else if(m_decision == 2)
        {
            Mouledoux.Components.Mediator.instance.NotifySubscribers($"{m_itemName}solvenow");
        }
    }
 
    public void GetActiveToggle()
    {
        //Check to see if anything is active
        if (!m_toggleGroup.AnyTogglesOn())
        {
            Debug.Log("There are no toggles on.");
            //m_submitButton.SetActive(true);

            return;
        }

        //Get active toggle
        Toggle activeToggle = m_toggleGroup.ActiveToggles().ToList()[0];
        m_givenAnswer = activeToggle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;

        //Fix up the toggles
        foreach (GameObject go in m_toggles)
        {
            go.GetComponent<Collider>().enabled = false;
            if (go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text == m_expectedRating)
            {
                go.GetComponent<InteractableObject>().m_onHighnight.Invoke();
            }
        }

        Mouledoux.Components.Mediator.instance.NotifySubscribers($"{m_itemName}rated");
    }

    private void GatherAndSendData()
    {
        var info = new HazardUpdateInfo(m_itemName, m_itemScore, m_actions, $"{m_givenAnswer}/{m_expectedRating}");
    }
}
