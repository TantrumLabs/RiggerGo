using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HazardAnalysisBlackboard : MonoBehaviour
{
    private static HazardAnalysisBlackboard s_instance;
    public static HazardAnalysisBlackboard instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<HazardAnalysisBlackboard>();
                DontDestroyOnLoad(s_instance.gameObject);
            }

            return s_instance;
        }
    }
    public List<HazardUpdateInfo> hazardCollection = new List<HazardUpdateInfo>();

    private void Start()
    {
        if (instance != null && instance != this) Destroy(gameObject);
    }

    public void AddHazardAssesment(HazardUpdateInfo info)
    {
        hazardCollection.Add(info);

        string update = $"Item Name:{info.m_itemName}|||Actions Taken:{info.m_actionsTaken}|||Item Score:{info.m_itemScore}";

        MironDB.MironDB_Manager.UpdateTest(2, update, /*Convert.ToInt32(info.m_questionNumber) + */6001);
    }

    public void ClearCollection()
    {
        hazardCollection = new List<HazardUpdateInfo>();
    }
}

[Serializable]
public struct HazardUpdateInfo
{
    public string m_itemName { get; private set; }
    public string m_itemScore { get; private set; }
    public string m_actionsTaken { get; private set; }
    public string m_ratingResults { get; private set; }

    public HazardUpdateInfo(string p_iN, string p_iS, string p_aT, string p_rR)
    {
        m_actionsTaken = p_aT;
        m_itemName = p_iN;
        m_itemScore = p_iS;
        m_ratingResults = p_rR;
    }
}