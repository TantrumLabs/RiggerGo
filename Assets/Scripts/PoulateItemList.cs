using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PoulateItemList : MonoBehaviour
{
    public Transform m_populationRoot;
    public GameObject m_itemPrefab;
    [SerializeField]private List<ItemInformation> m_itemList = new List<ItemInformation>();

    private void Awake()
    {
        if(m_populationRoot == null)
        {
            m_populationRoot = transform;
        }
    }

    public void SetAndPopulate(List<ItemInformation> p_otherList)
    {
        SetListToOtherList(p_otherList);
        PopulateField();
    }

    public void SetListToOtherList(List<ItemInformation> p_otherList)
    {
        m_itemList.Clear();

        m_itemList = p_otherList;
    }

    [ContextMenu("Populate")]
    public void PopulateField()
    {

        foreach(Transform t in m_populationRoot)
        {
            Destroy(t.gameObject);
        }

        foreach(ItemInformation ii in m_itemList)
        {
            GameObject itemGO = Instantiate(m_itemPrefab, m_populationRoot);
            var quickSub = itemGO.GetComponent<QuickSubscribe>();

            itemGO.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ii.m_itemDescription;
            Toggle needsDelToggle = null;
            Toggle doneToggle = null;

            foreach(Toggle t in m_populationRoot.GetComponentsInChildren<Toggle>())
            {
                if (t.name == "Needs Action Toggle")
                {
                    needsDelToggle = t;
                }

                else if (t.name == "Done Toggle")
                {
                    doneToggle = t;
                }
            }

            UnityEvent needsEvent = new UnityEvent();
            UnityEvent doneEvent = new UnityEvent();

            needsEvent.AddListener(delegate{ needsDelToggle.isOn = true; });
            doneEvent.AddListener(delegate { doneToggle.isOn = true; });

            //quickSub.AddMessageToWatch($"{ii.m_itemName}delegated", needsEvent);
            //quickSub.AddMessageToWatch($"{ii.m_itemName}solvenow", doneEvent);
        }
    }
}

[Serializable]
public struct ItemInformation
{
    public string m_itemName;
    public string m_itemDescription;

    public ItemInformation(string p_name, string p_description)
    {
        m_itemName = p_name;
        m_itemDescription = p_description;
    }
}