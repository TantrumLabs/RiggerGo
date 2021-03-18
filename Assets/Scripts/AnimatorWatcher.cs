using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorWatcher : MonoBehaviour
{

    public Animator m_animator;
    public UnityEngine.Events.UnityEvent m_endEvent;

    public void BeginWaitForAnimator(){
        StartCoroutine(WaitForAnimator(m_animator));
    }

    private IEnumerator WaitForAnimator(Animator a){
        
        yield return new WaitUntil(() => a.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        m_endEvent.Invoke();
    }
}
