using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableEvent : MonoBehaviour
{
    [SerializeField]
    private float delay = 0f;
    [SerializeField]
    private bool selfDestruct;
    public UnityEngine.Events.UnityEvent doOnEnable;

    private void OnEnable()
    {
        StartCoroutine(OnEnableRoutine());
    }

    IEnumerator OnEnableRoutine()
    {
        yield return new WaitForSeconds(delay);

        doOnEnable.Invoke();

        if (selfDestruct) Destroy(this);
    }
}
