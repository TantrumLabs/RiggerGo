using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SetupLineRenderer : MonoBehaviour
{
    public Transform m_originalStartTransform;
    public Transform m_originalEndTransform;
    private LineRenderer m_lineRenderer;

    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        ResetToOriginal();
    }

    private void OnEnable()
    {
        Start();
    }

    public void SetLinerendererStartPosition(Transform trans)
    {
        m_lineRenderer.SetPosition(0, trans.position);
    }

    public void SetLinerendererStartPosition(Vector3 vec)
    {
        m_lineRenderer.SetPosition(0, vec);
    }

    public void SetLinerendererEndPosition(Transform trans)
    {
        m_lineRenderer.SetPosition(1, trans.position);
    }

    public void SetLinerendererEndPosition(Vector3 vec)
    {
        m_lineRenderer.SetPosition(1, vec);
    }

    [ContextMenu("Reset To Origins")]
    public void ResetToOriginal()
    {
        m_lineRenderer.SetPositions(new Vector3[] { m_originalStartTransform.position, m_originalEndTransform.position });
    }
}
