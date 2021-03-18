using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public FlashType m_flashType;
    [Space]
    [SerializeField]private float m_frequency;
    [Space]
    
    [Range(0,0.1f)]
    [SerializeField]private float m_colorDifference;
    
    public enum FlashType{
        ONOFF,
        COLORALPHA,
    }

    private Renderer m_renderer;
    private void OnEnable() {
        m_renderer = GetComponent<Renderer>();
        switch(m_flashType){
            case FlashType.ONOFF:
                StartCoroutine(FlashEnumerator());
                break;

            case FlashType.COLORALPHA:
                StartCoroutine(PulseEnumerator());
                break;
        }
    }

    private IEnumerator FlashEnumerator(){
        while(true){
            yield return new WaitForSeconds(m_frequency);

            m_renderer.enabled = !m_renderer.enabled;
        }
    }

    private IEnumerator PulseEnumerator(){
        var originalColor = m_renderer.material.color.a;

        var tracker = originalColor;
        var decreasing = false;

        if(tracker >= 1){
            decreasing = true;
        }

        while(true){
            if(decreasing == false && tracker >= 1){ decreasing = true;}
            else if(decreasing == true && tracker <= 0){decreasing = false;}

            if(decreasing){
                tracker -= m_colorDifference;
            }

            else{
                tracker += m_colorDifference;
            }

            yield return null;
        }
    }

    private void OnDisable() {
        StopAllCoroutines();       
    }
}
