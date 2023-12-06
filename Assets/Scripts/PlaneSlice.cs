using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;

public class PlaneSlice : MonoBehaviour
{
    // public Transform planeDebug;
    public Transform firstPlane;
    public Transform secondPlane;
    public GameObject target;
    public Material crossSectionMaterial;

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        SlicedHull hull = target.Slice(firstPlane.position, firstPlane.up);

        if(hull != null) 
        {
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            // GameObject upperHull1 = hull.CreateUpperHull(target, crossSectionMaterial);
            // Destroy(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            Destroy(target);

            if(lowerHull != null)
            {
                GameObject middleHull = hull.CreateUpperHull(target, crossSectionMaterial);
                GameObject finalLowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
                Destroy(target);
            }
        }
    }
}
