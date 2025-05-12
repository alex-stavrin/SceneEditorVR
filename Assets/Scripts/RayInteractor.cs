using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RayInteractor : MonoBehaviour
{
    LineRenderer lineRenderer;

    [SerializeField] float rayRange;
    [SerializeField] Transform rayStart;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // reset any changes made in the editor
        lineRenderer.positionCount = 0;

        // line start
        lineRenderer.positionCount = 2;

    }

    void Update()
    {
        lineRenderer.SetPosition(0, rayStart.position);

        RaycastHit hit;
        if(Physics.Raycast(rayStart.position, rayStart.forward, out hit, rayRange))
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            Vector3 rayEnd = rayStart.position + rayStart.forward * rayRange;
            lineRenderer.SetPosition(1, rayEnd);
        }
    }
}