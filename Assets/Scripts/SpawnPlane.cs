using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class SpawnPlane : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public GameObject pointPrefab;
    private Transform point1, point2;
    private bool isPoint1Spawned = false;
    private bool isPoint2Spawned = false;

    public GameObject planePrefab;
    private bool isPlaneSpawned = false;

    private PokeInteractor pokeInteractor;

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
        pokeInteractor = GetComponent<PokeInteractor>();

        if(pokeInteractor == null)
        {
            Debug.LogError("Poke Interactor not found. Make sure it's attached to this GameObject.");
        }

        Oculus.Interaction.InteractableUnityEventWrapper._whenSelect(SpawnPoints);
    }

    public void SpawnPoints()
    {
        // Get the poke position
        Vector3 pokePosition = PokePosition();

        // Spawn the point at the poke position
        if (!isPoint1Spawned)
        {
            point1 = Instantiate(pointPrefab, pokePosition, Quaternion.identity).transform;
            isPoint1Spawned = true;
        }
        else if (!isPoint2Spawned)
        {
            point2 = Instantiate(pointPrefab, pokePosition, Quaternion.identity).transform;
            isPoint2Spawned = true;
        }
    }

    private Vector3 PokePosition()
    {
        // Get the current hand position
        Vector3 pokePosition = pokeInteractor.transform.position;

        // Convert the hand position from local space to world space
        pokePosition = pokeInteractor.transform.TransformPoint(pokePosition);

        if(pokePosition == null)
        {
            Debug.Log("No Poke");
        }

        return pokePosition;
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