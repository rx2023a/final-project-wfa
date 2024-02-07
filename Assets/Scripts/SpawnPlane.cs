using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class SpawnPlane : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private PokeInteractable eventWrapper;

    public GameObject pointPrefab;
    private Transform point1, point2;
    private bool isPoint1Spawned = false;
    private bool isPoint2Spawned = false;

    public GameObject planePrefab;
    private bool isPlaneSpawned = false;

    private void Start()
    {
        // Initialize LineRenderer if not already assigned
        lineRenderer = GetComponent<LineRenderer>();

        if(lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }

        // Get Poke Interactable component
        eventWrapper = GetComponent<PokeInteractable>();

        if(eventWrapper == null)
        {
            Debug.LogError("Poke Interactable not found");
        }

        //eventWrapper.OnSelect.AddListener(OnPokeStart);
    }

    void OnPokeStart(Vector3 pokePosition)
    {
        if(!isPoint1Spawned) 
        {
            OnPokeSpawnFirstPoint(pokePosition);      
        }
        else if(!isPoint2Spawned) 
        {
            SpawnSecondPointAndPlane(pokePosition);
        }
    }

    void OnPokeSpawnFirstPoint(Vector3 position)
    {
        point1 = Instantiate(pointPrefab, position, Quaternion.identity).transform;
        isPoint1Spawned = true;
    }

    void SpawnSecondPointAndPlane(Vector3 position)
    {
        point2 = Instantiate(pointPrefab, position, Quaternion.identity).transform;
        isPoint2Spawned = true;
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, point1.position);
        lineRenderer.SetPosition(1, point2.position);
        SpawnPlaneFromLine();
    }

    void SpawnPlaneFromLine()
    {
        Vector3 midPoint = (point1.position + point2.position) / 2;
        GameObject plane = Instantiate(planePrefab, midPoint, Quaternion.identity);

        Vector3 direction = (point2.position - point1.position).normalized;
        plane.transform.rotation = Quaternion.LookRotation(direction);
        plane.transform.Rotate(180, 0, 90, Space.Self);

        float lineLength = Vector3.Distance(point1.position, point2.position);

        foreach(Transform child in plane.transform)
        {
            child.localScale = new Vector3(lineLength, child.localScale.y, child.localScale.z);
        }

        lineRenderer.enabled = false;
        point1.gameObject.SetActive(false);
        point2.gameObject.SetActive(false);

        isPlaneSpawned = true;
    }
}


/*
Hey, can you make me a script for my VR Unity projects? I'm using Oculus Interactions SDK as a plugin. So what I want is I want to poke once on some 3D mesh, and then it spawned a point/pin. Then, I poke the second time, then it spawned the second point/pin. After both points are spawned, it create a line renderer. And from that line renderer, it spawned a plane from my plane prefab. After the plane is spawned, destroy both points and line renderer. I already make a script for it but I don't think it's right. Can you help me ? Here's the script 
*/