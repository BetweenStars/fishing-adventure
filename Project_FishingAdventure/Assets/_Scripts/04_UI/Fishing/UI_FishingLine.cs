using UnityEngine;

public class UI_FishingLine : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private Transform startTransform;
    private Vector3 endPos;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (lineRenderer.enabled == true && startTransform != null)
        {
            lineRenderer.SetPosition(0, startTransform.position);
            lineRenderer.SetPosition(1, endPos);
        }
    }

    public void StartLine(Transform start, Vector3 end)
    {
        startTransform = start;
        endPos = end;
        lineRenderer.enabled = true;
    }
    public void EndLine()
    {
        lineRenderer.enabled = false;
        startTransform = null;
    }
}
