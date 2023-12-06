using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;

public class PlaneSlice1 : MonoBehaviour
{
    public Transform planeDebug;
    // public Transform firstPlane;
    // public Transform secondPlane;
    public GameObject target;
    public Material crossSectionMaterial;

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.xKey.wasPressedThisFrame)
        {
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        SlicedHull hull = target.Slice(planeDebug.position, planeDebug.up);

        if(hull != null) 
        {
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            Destroy(target);
        }
    }
}
