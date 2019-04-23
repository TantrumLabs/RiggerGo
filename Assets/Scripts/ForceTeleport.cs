using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceTeleport : MonoBehaviour
{
    public UnityEngine.UI.Image blindfold;
    public float fadeTime, delayTime;
    private Vector3 originalPos;
    private Quaternion originalRot;
    public GameObject objectRef;

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

        nextRailHandler += NextRailPoint;
        m_subscriptions.Subscribe("MoveToNextRailPoint", nextRailHandler);
    }


    private void OnDestroy()
    {
        m_subscriptions.UnsubscribeAll();
    }

    public void SetoriginalPos()
    {

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
        yield return new WaitForSeconds(.1f);
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

        if (blindfold != null)
        {
            Color c = blindfold.color;
            print(blindfold.color);
            c.a = 1f;
            blindfold.color = c;

            while (blindfold.color.a > 0.01f)
            {
                c.a -= (Time.deltaTime / fadeTime);
                blindfold.color = c;
                yield return null;
            }

            c.a = 0f;
            blindfold.color = c;
        }
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
        if(currentPoint >= railPoints.Count - 1)
        {
            currentPoint = railPoints.Count - 1;
            Mouledoux.Components.Mediator.instance.NotifySubscribers("setreview");
            StartTeleport(railPoints[currentPoint]);
            return;
        }

        var pack = new Mouledoux.Callback.Packet();
        pack.ints = new int[]{currentPoint};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("teleporting", pack);
        StartTeleport(railPoints[currentPoint]);
    }

    public void NextRailPoint(Mouledoux.Callback.Packet data)
    {
        NextRailPoint();
    }
}