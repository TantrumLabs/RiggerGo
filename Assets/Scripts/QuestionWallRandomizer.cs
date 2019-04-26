using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionWallRandomizer : MonoBehaviour
{
    public List<GameObject> m_answers = new List<GameObject>();

    void Start(){
        RearrangeAnswers();
    }

    [ContextMenu("Rearrange Answers")]
    public void RearrangeAnswers(){
        var tempList = new List<GameObject>();
        foreach(GameObject go in m_answers)
            tempList.Add(go);

        tempList.Remove(tempList[tempList.Count-1]);

        var positionList = new List<int>{0,1,2};

        foreach(GameObject go in tempList){
            int index = (positionList.Count - 1 > 0)?Random.Range(
                0, positionList.Count): 0;
            int position = positionList[index];
            positionList.Remove(positionList[index]);

            RectTransform goRectTransform = go.GetComponent<RectTransform>();
            Text goText = go.GetComponentInChildren<Text>();
            var tempText = goText.text;
            var parsedString = tempText.Split(new char[]{'.'});
            string result = "";

            switch(position){
                case 0:
                    goRectTransform.anchorMax = new Vector2(1, .65f);
                    goRectTransform.anchorMin = new Vector2(0, .55f);
                    parsedString[0] = "A";
                    break;

                case 1:
                    goRectTransform.anchorMax = new Vector2(1, .5f);
                    goRectTransform.anchorMin = new Vector2(0, .4f);
                    parsedString[0] = "B";
                    break;

                case 2:
                    goRectTransform.anchorMax = new Vector2(1, .35f);
                    goRectTransform.anchorMin = new Vector2(0, .25f);
                    parsedString[0] = "C";
                    break;

                default:
                    break;
            }

            foreach(string s in parsedString)
            {
                 if(s!=" " && s!="")
                     result += s+ ".";
            }

            goText.text = result;

        }
    }
}
