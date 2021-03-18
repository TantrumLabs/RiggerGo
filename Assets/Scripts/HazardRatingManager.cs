using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HazardRatingManager : MonoBehaviour
{
    [SerializeField] private ToggleGroup m_toggleGroup;
    [SerializeField] private List<string> m_expectedResults = new List<string>();
    [SerializeField] private GameObject m_submitButton;
    [SerializeField] private GameObject m_closeButton;
    [SerializeField] private int m_questionNumber;
    [SerializeField] private List<string> m_choiceTextReplacements = new List<string>();

    private TMPro.TextMeshProUGUI m_question;
    private List<GameObject> m_toggles = new List<GameObject>();

    private void Start()
    {
        foreach(Toggle to in GetComponentsInChildren<Toggle>())
        {
            m_toggles.Add(to.gameObject);
        }

        m_question = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        Debug.Log($"{m_submitButton.name} is the submit button");
    }

    public void StartSubmission()
    {
        StartCoroutine(FindSelectedToggle());
    }


    public IEnumerator FindSelectedToggle()
    {
        m_submitButton.SetActive(false);

        //Check to see if anything is active
        if (!m_toggleGroup.AnyTogglesOn())
        {
            Debug.Log("There are no toggles on.");
            m_submitButton.SetActive(true);

            yield break;
        }

        //Get active toggle
        Toggle activeToggle = m_toggleGroup.ActiveToggles().ToList()[0];
        string toggleValue = activeToggle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;

        //Set up new text
        m_question.text = m_choiceTextReplacements[System.Convert.ToInt32(toggleValue) - 1];

        //String up expected results
        string expectedString = string.Empty;

        foreach (string i in m_expectedResults)
        {
            expectedString += (i == m_expectedResults[m_expectedResults.Count - 1]) ? i.ToString() : i.ToString() + ",";
        }

        //Fix up the toggles
        foreach(GameObject go in m_toggles)
        {
            go.GetComponent<Collider>().enabled = false;
            if (m_expectedResults.Contains(go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text))
            {
                go.GetComponent<InteractableObject>().m_onHighnight.Invoke();
            }
        }
        //Activate Close Button
        m_closeButton.SetActive(true);

        //Send the Strings where they need to go.
        //var info = new HazardUpdateInfo($"{m_questionNumber}", expectedString, toggleValue);
        //HazardAnalysisBlackboard.instance.AddHazardAssesment(info);
    }
}
