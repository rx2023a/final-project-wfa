using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck: MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject spherePrefab;


    private void Start()
    {
        mainCamera = Camera.main;
        print(mainCamera.name);
    }

    void Update()
    {
        // Draw Ray
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayOrigin = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(rayOrigin, out rayHit, 100)) 
            {
                Debug.Log(rayHit.transform.name); 
                Instantiate(spherePrefab, rayHit.point, Quaternion.identity);
            }
        }
    }
}
