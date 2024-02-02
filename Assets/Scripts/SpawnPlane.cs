using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlane : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public GameObject pointPrefab;
    private Transform point1, point2;
    private bool isPoint1Spawned = false;
    private bool isPoint2Spawned = false;

    public GameObject planePrefab;
    private bool isPlaneSpawned = false;

    void Start()
    {
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isPoint1Spawned)
        {
            point1 = Instantiate(pointPrefab).transform;
            isPoint1Spawned = true;
        }

        if (Input.GetKeyDown(KeyCode.X) && isPoint1Spawned && !isPoint2Spawned)
        {
            point2 = Instantiate(pointPrefab).transform;
            isPoint2Spawned = true;
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, point1.position);
            lineRenderer.SetPosition(1, point2.position);
            SpawnPlaneFromLine();
        }
    }
    void SpawnPlaneFromLine()
    {
        Vector3 midPoint = (point1.position + point2.position) / 2;
        GameObject plane = Instantiate(planePrefab, midPoint, Quaternion.identity);

        Vector3 direction = (point2.position - point1.position).normalized;
        plane.transform.rotation = Quaternion.LookRotation(direction);
        plane.transform.Rotate(180, 0, 90, Space.Self);

        float lineLength = Vector3.Distance(point1.position, point2.position);

        foreach (Transform child in plane.transform)
        {
            child.localScale = new Vector3(lineLength, child.localScale.y, child.localScale.z);
        }

        Destroy(lineRenderer.gameObject);
        Destroy(point1.gameObject);
        Destroy(point2.gameObject);

        isPlaneSpawned = true;
    }

}
