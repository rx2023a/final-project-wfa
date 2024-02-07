using UnityEngine;

public class SpawnCubeOnSelect : MonoBehaviour
{
    public GameObject pointPrefab; // Prefab of the point object
    public GameObject lineRendererPrefab; // Prefab of the line renderer object
    public GameObject planePrefab; // Prefab of the plane object

    private Transform point1;
    private Transform point2;
    private LineRenderer lineRenderer;

    private void OnSelect()
    {
        // If the first point hasn't been spawned yet, spawn it
        if (point1 == null)
        {
            point1 = Instantiate(pointPrefab, transform.position, Quaternion.identity).transform;
        }
        // If the second point hasn't been spawned yet, spawn it and create the line renderer
        else if (point2 == null)
        {
            point2 = Instantiate(pointPrefab, transform.position, Quaternion.identity).transform;
            lineRenderer = Instantiate(lineRendererPrefab, transform.position, Quaternion.identity).GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, point1.position);
            lineRenderer.SetPosition(1, point2.position);
        }
        // If both points have been spawned, spawn the plane
        else
        {
            Instantiate(planePrefab, (point1.position + point2.position) / 2, Quaternion.identity);
            Destroy(point1.gameObject);
            Destroy(point2.gameObject);
            Destroy(lineRenderer.gameObject);
            point1 = null;
            point2 = null;
            lineRenderer = null;
        }
    }
}