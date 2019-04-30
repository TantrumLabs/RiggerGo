using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionWallRandomizer : MonoBehaviour
{
    public bool m_includeFinalAnswer = false;
    public List<GameObject> m_answers = new List<GameObject>();
    
    void Start(){
        RearrangeAnswers();
    }

    [ContextMenu("Rearrange Answers")]
    public void RearrangeAnswers(){
        var tempList = new List<GameObject>();
        foreach(GameObject go in m_answers)
            tempList.Add(go);

        if(!m_includeFinalAnswer)
            tempList.Remove(tempList[tempList.Count-1]);

        var positionList = (!m_includeFinalAnswer) ? new List<int>{0,1,2} : 
            new List<int>{0,1,2,3};

        foreach(GameObject go in tempList){
            int index = (positionList.Count - 1 > 0)?Random.Range(
                0, positionList.Count): 0;
            int position = positionList[index];
            positionList.Remove(positionList[index]);

            RectTransform goRectTransform = go.GetComponent<RectTransform>();
            TMPro.TextMeshProUGUI goText = go.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            var tempText = goText.text;
            //var parsedString = tempText.Split(new char[]{'.'});
            //string result = "";

            switch(position){
                case 0:
                    goRectTransform.anchorMax = new Vector2(1, .65f);
                    goRectTransform.anchorMin = new Vector2(0, .55f);
                    goText.text = "A. " + goText.text;
                    break;

                case 1:
                    goRectTransform.anchorMax = new Vector2(1, .5f);
                    goRectTransform.anchorMin = new Vector2(0, .4f);
                    goText.text = "B. " + goText.text;
                    break;

                case 2:
                    goRectTransform.anchorMax = new Vector2(1, .35f);
                    goRectTransform.anchorMin = new Vector2(0, .25f);
                    goText.text = "C. " + goText.text;
                    break;

                case 3:
                    goRectTransform.anchorMax = new Vector2(1, .2f);
                    goRectTransform.anchorMin = new Vector2(0, .1f);
                    goText.text = "D. " + goText.text;
                    break;

                default:
                    break;
            }

            //goText.text = result;

        }
    }
}
