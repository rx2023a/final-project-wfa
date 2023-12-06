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
        SlicedHull firstSlice = target.Slice(firstPlane.position, firstPlane.up);

        if(firstSlice != null) 
        {
            GameObject upperHull = firstSlice.CreateUpperHull(target, crossSectionMaterial);
            GameObject lowerHull = firstSlice.CreateLowerHull(target, crossSectionMaterial);
            // Destroy(target);

            SlicedHull secondSlice = lowerHull.Slice(secondPlane.position, secondPlane.up);
            
            if (secondSlice != null)
            {
                GameObject middleHull = secondSlice.CreateUpperHull(target, crossSectionMaterial);
                GameObject finalLowerHull = secondSlice.CreateLowerHull(target, crossSectionMaterial);
                Destroy(lowerHull);
            }
            Destroy(target);
        }
    }
}
