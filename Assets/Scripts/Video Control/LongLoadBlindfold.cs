using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Renderer))]
public class LongLoadBlindfold : MonoBehaviour
{
    private Renderer m_renderer;
    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = new Mouledoux.Components.Mediator.Subscriptions();

    public float m_fadeOutDuriation;
    public TMPro.TextMeshProUGUI m_titleText;
    public Image m_image;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetAlpha(0);
        m_subscriptions.Subscribe("startpreparing", delegate { SetAlpha(1); });
        m_subscriptions.Subscribe("donepreparing", delegate { StartCoroutine(FadeAway()); });
    }

    private void SetAlpha(float f)
    {
        m_renderer.material.color = new Color(m_renderer.material.color.r, m_renderer.material.color.g, m_renderer.material.color.b, f);
        m_titleText.color = new Color(m_titleText.color.a, m_titleText.color.g, m_titleText.color.b, f);
        m_image.color = new Color(m_image.color.a, m_image.color.g, m_image.color.b, f);
    }

    private IEnumerator FadeAway()
    {
        var material = m_renderer.material;
        Color c = material.color;
        Color a = m_image.color;
        Color b = m_titleText.color;

        while (material.color.a > 0.001f)
        {
            var delta = (Time.deltaTime / m_fadeOutDuriation);

            c.a -= delta;
            a.a -= delta;
            b.a -= delta;
            material.color = c;
            m_image.color = a;
            m_titleText.color = b;
            yield return null;
        }

        c.a = 0f;
    }

    [ContextMenu("Done")]
    private void StartSystem()
    {
        Mouledoux.Components.Mediator.instance.NotifySubscribers("donepreparing");
    }
}
