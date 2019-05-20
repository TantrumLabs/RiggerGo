using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceTeleport : MonoBehaviour
{
    public UnityEngine.UI.Image blindfold, m_loadingLogo;
    public TMPro.TextMeshProUGUI m_loading, m_areaTeller;
    public ViveHandInteractionLaserPointer m_lazer;
    public float fadeTime, delayTime;
    private Vector3 originalPos;
    private Quaternion originalRot;
    public GameObject objectRef;
    public float m_revealTime;


    public List<Transform> railPoints;
    public int currentPoint = 0;

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = 
    new Mouledoux.Components.Mediator.Subscriptions();

    Mouledoux.Callback.Callback nextRailHandler = null;

    // ---------- ---------- ---------- ---------- ---------- 
    private void Start()
    {
        if (objectRef == null)
        { objectRef = gameObject; }

        objectRef.transform.position = railPoints[currentPoint].position;
        originalPos = objectRef.transform.localPosition;
        originalRot = objectRef.transform.localRotation;

        StartCoroutine(FadeInOut());
        TurnOffOtherZones();

        nextRailHandler += NextRailPoint;
        m_subscriptions.Subscribe("MoveToNextRailPoint", nextRailHandler);
    }


    private void OnDestroy()
    {
        m_subscriptions.UnsubscribeAll();
    }

    /// <summary>
    ///     Used to call via the messenger
    /// </summary>
    /// <param name="pos"></param>
    public void StartTeleport(Transform pos)
    {
        StartCoroutine(TeleportPlayerToTransform(pos));
    }

    /// <summary>
    ///     Teleport the player to the set location with a delay and a blindfold
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    // ---------- ---------- ---------- ---------- ---------- 
    public IEnumerator TeleportPlayerToTransform(Transform pos)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(FadeInOut());
        TurnOffOtherZones();
        yield return new WaitForSeconds(.2f);
        print("Teleported to: " + pos.gameObject.name);
        pos.gameObject.SetActive(true);
        objectRef.transform.position = pos.position;
        objectRef.transform.rotation = pos.rotation;
    }

    public void StartNextZone(Transform pos)
    {
        StartCoroutine(NextZone(pos));
    }

    private IEnumerator NextZone(Transform pos)
    {
        yield return new WaitForSeconds(delayTime);
        pos.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Used to call via the messenger
    /// </summary>
    public void StartLoadScene(string scene)
    {
        StartCoroutine(LoadScene(scene));
    }

    /// <summary>
    ///     Load the scene at the end of the game
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadScene(string scene)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadSceneAsync(scene);
    }


    // ---------- ---------- ---------- ---------- ---------- 
    public void ResetToOriginalPosIn(float time)
    {
        StartCoroutine(TimeToTeleportBack(time));
    }


    // ---------- ---------- ---------- ---------- ---------- 
    public void SetOriginalValuesToCurrent()
    {
        originalPos = objectRef.transform.localPosition;
        originalRot = objectRef.transform.localRotation;
    }

    // ---------- ---------- ---------- ---------- ---------- 
    public void SetOriginalValuesTo(Transform newValues)
    {
        originalPos = newValues.localPosition;
        originalRot = newValues.localRotation;
    }



    // ---------- ---------- ---------- ---------- ---------- 
    public IEnumerator FadeInOut()
    {
        m_lazer.enabled = false;
        if (blindfold != null)
        {
            Color c = blindfold.color;
            Color loadingText = m_loading.color;
            Color loadingLogo = m_loadingLogo.color;

            print(blindfold.color);
            loadingLogo.a = 1f;
            loadingText.a = 1f;
            c.a = 1f;
            blindfold.color = c;
            m_loadingLogo.color = loadingLogo;
            m_loading.color = loadingText;
            m_areaTeller.color = loadingText;

            yield return new WaitForSeconds(m_revealTime);

            while (blindfold.color.a > 0.001f)
            {
                var delta = (Time.deltaTime / fadeTime);

                c.a -= delta;
                loadingText.a -= delta;
                loadingLogo.a -= delta;

                m_loadingLogo.color = loadingLogo;
                m_loading.color = loadingText;
                m_areaTeller.color = loadingText;
                blindfold.color = c;
                yield return null;
            }

            loadingLogo.a = 0f;
            loadingText.a = 0f;
            c.a = 0f;
            blindfold.color = c;
            m_loading.color = loadingText;
            m_loadingLogo.color = loadingLogo;
            m_areaTeller.color = loadingText;
        }

        m_lazer.enabled = true;
    }


    // ---------- ---------- ---------- ---------- ---------- 
    public IEnumerator TimeToTeleportBack(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(FadeInOut());
        objectRef.transform.localPosition = originalPos;
        objectRef.transform.localRotation = originalRot;
    }

    [ContextMenu("Next Point")]
    public void NextRailPoint()
    {
        currentPoint++;
        //TurnOffOtherZones();
        if(currentPoint >= railPoints.Count - 1)
        {
            currentPoint = railPoints.Count - 1;
            Mouledoux.Components.Mediator.instance.NotifySubscribers("setreview");
            StartTeleport(railPoints[currentPoint]);
            return;
        }

        var pack = new Mouledoux.Callback.Packet();
        pack.ints = new int[]{currentPoint};
        object[] data = {pack};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("teleporting", data);
        StartTeleport(railPoints[currentPoint]);
    }

    public void NextRailPoint(object[] args)
    {
        NextRailPoint();
    }

    //THIS FUNCTION IS PROJECT SPECIFIC PLEASE ERASE IF TRANSFERING OVER(Project: Rigger Go)
    public void TurnOffOtherZones(){
        for(int i=0; i < railPoints.Count; i++){
            if(i == currentPoint){
                railPoints[i].transform.parent.gameObject.SetActive(true);
                continue;
            }
                

            railPoints[i].transform.parent.gameObject.SetActive(false);
        }
    }
}