using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionWallObjectsRandomizer : MonoBehaviour
{
    public List<GameObject> m_answers = new List<GameObject>();
    
    [ContextMenu("Rearrange Models")]
    public void RearrangeAnswers(){
        var tempList = new List<GameObject>();
        foreach(GameObject go in m_answers)
            tempList.Add(go);

        var positionList = new List<int>{0,1,2};

        foreach(GameObject go in tempList){
            int index = (positionList.Count - 1 > 0)?Random.Range(
                0, positionList.Count): 0;
            int position = positionList[index];
            positionList.Remove(positionList[index]);

            switch(position){
                case 0:
                    go.transform.localPosition = new Vector3(1,-0.8f, 0);
                    break;

                case 1:
                    go.transform.localPosition = new Vector3(0,-0.8f, 0);
                    break;

                case 2:
                    go.transform.localPosition = new Vector3(-1,-0.8f, 0);
                    break;

                default:
                    break;
            }

        }
    }
}
