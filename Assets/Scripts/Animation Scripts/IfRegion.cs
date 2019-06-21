using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfRegion : MonoBehaviour
{
    public List<GameObject> british = new List<GameObject>();

    public List<GameObject> merica = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        if(AudioOffset.m_UKVersion)
        {
            foreach(GameObject go in british)
                go.SetActive(true);

            foreach(GameObject go in merica)
                go.SetActive(false);
        }

        else
        {
            foreach(GameObject go in british)
                go.SetActive(false);

            foreach(GameObject go in merica)
                go.SetActive(true);
        }
    }
}
