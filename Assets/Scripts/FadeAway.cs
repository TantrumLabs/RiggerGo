using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FadeAway : MonoBehaviour
{
    public float m_fadeInDuration;
    public float m_waitTime;
    public float m_fadeOutDuration;
    private Renderer m_renderer;
    private bool m_firstFadeIn = true;

    private Mouledoux.Components.Mediator.Subscriptions m_subs = new Mouledoux.Components.Mediator.Subscriptions();

    private void Start()
    {
        m_subs.Subscribe("startfade", delegate { StartCoroutine(FadeCorutine()); });
    }

    private void OnDestroy()
    {
        m_subs.UnsubscribeAll();
    }

    // Start is called before the first frame update
    IEnumerator FadeCorutine()
    {
        m_renderer = gameObject.GetComponent<MeshRenderer>();
        var material = m_renderer.material;
        Color c = material.color;

        if (!m_firstFadeIn)
        {
            while (material.color.a < 0.999f)
            {
                var delta = (Time.deltaTime / m_fadeInDuration);

                c.a += delta;
                material.color = c;
                yield return null;
            }

            c.a = 1f;
            Mouledoux.Components.Mediator.instance.NotifySubscribers("fadeindone");
        }

        
        material.color = new Color(material.color.r, material.color.g, material.color.b, 1);

        c = material.color;

        yield return new WaitForSeconds(m_waitTime);

        while (material.color.a > 0.001f)
        {
            var delta = (Time.deltaTime / m_fadeOutDuration);

            c.a -= delta;
            material.color = c;
            yield return null;
        }

        c.a = 0f;
        m_firstFadeIn = false;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeCorutine());
    }
}
