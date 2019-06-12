using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TextReplacerScript : MonoBehaviour
{
    private List<string> bannedWords = new List<string>()
        {" riggers", " Rigger ", " rigger ", "\nrigger", "Rigger "};
    private List<string> replacmentWords = new List<string>()
        {" banksmen", " Banksman ", " banksman ","\nbanksman", "Banksman "};

    void Awake()
    {
        if(!AudioOffset.m_UKVersion)
            return;

        if(gameObject.GetComponent<TMPro.TextMeshProUGUI>() != null){
            var tmp = gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            foreach(string s in bannedWords){
                string newString = "";

                if(tmp.text.Contains(s)){
                    var split = Regex.Split(tmp.text, s);
                    List<string> splitList = new List<string>();
                    foreach (string st in split)
                    {
                        splitList.Add(st);
                    }

                    foreach (string st in splitList)
                    {
                        newString += (splitList.IndexOf(st) != splitList.Count - 1)? st + replacmentWords[bannedWords.IndexOf(s)]
                            : st;
                    }

                    tmp.text = newString;
                }
            }
        }
    }

    public void CheckText(){
        if(!AudioOffset.m_UKVersion)
            return;

        if(gameObject.GetComponent<TMPro.TextMeshProUGUI>() != null){
            var tmp = gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            foreach(string s in bannedWords){
                string newString = "";

                if(tmp.text.Contains(s)){
                    var split = Regex.Split(tmp.text, s);
                    List<string> splitList = new List<string>();
                    foreach (string st in split)
                    {
                        splitList.Add(st);
                    }

                    foreach (string st in splitList)
                    {
                        newString += (splitList.IndexOf(st) != splitList.Count - 1)? st + replacmentWords[bannedWords.IndexOf(s)]
                            : st;
                    }

                    tmp.text = newString;
                }
            }
        }
    }
}
