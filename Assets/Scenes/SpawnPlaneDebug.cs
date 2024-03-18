using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlaneDebug : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public GameObject pointPrefab;
    private Transform point1, point2;
    private bool isPoint1Spawned = false;
    private bool isPoint2Spawned = false;

    public GameObject planePrefab;
    private bool isPlaneSpawned = false;

    void Start()
    {
        // Initialize LineRenderer if not already assigned
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Ray origin: " + ray.origin + ", Ray direction: " + ray.direction);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast hit at point: " + hit.point + ", hit object: " + hit.collider.gameObject.name);

                if (!isPoint1Spawned)
                {
                    Debug.Log("Spawning point 1 at: " + hit.point);
                    point1 = Instantiate(pointPrefab, hit.point, Quaternion.identity).transform;
                    isPoint1Spawned = true;
                }
                else if (!isPoint2Spawned)
                {
                    Debug.Log("Spawning point 2 at: " + hit.point);
                    point2 = Instantiate(pointPrefab, hit.point, Quaternion.identity).transform;
                    isPoint2Spawned = true;
                    lineRenderer.enabled = true;
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, point1.position);
                    lineRenderer.SetPosition(1, point2.position);
                    SpawnPlaneFromLine();
                }
                else
                {
                    Debug.Log("Both points are already spawned.");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
    }

    void SpawnPlaneFromLine()
    {
        Vector3 midPoint = (point1.position + point2.position) / 2;
        Debug.Log("Spawning plane at midpoint: " + midPoint);
        GameObject plane = Instantiate(planePrefab, midPoint, Quaternion.identity);

        Vector3 direction = (point2.position - point1.position).normalized;
        plane.transform.rotation = Quaternion.LookRotation(direction);
        plane.transform.Rotate(180, 0, 90, Space.Self);

        float lineLength = Vector3.Distance(point1.position, point2.position);

        foreach (Transform child in plane.transform)
        {
            child.localScale = new Vector3(lineLength, child.localScale.y, child.localScale.z);
        }

        lineRenderer.enabled = false;
        point1.gameObject.SetActive(false);
        point2.gameObject.SetActive(false);

        isPlaneSpawned = true;
    }
}
