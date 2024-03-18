using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;

public class PlaneSlice : MonoBehaviour
{
    public Transform firstPlane;
    public Transform secondPlane;
    public GameObject target;
    public Material crossSectionMaterial;

    public GameObject firstPlaneObject;
    public GameObject secondPlaneObject;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        // Store references to the parent GameObject and the sliced parts
        target.transform.parent = null;
        SlicedHull firstSlice = target.Slice(firstPlane.position, firstPlane.up);

        if (firstSlice != null)
        {
            GameObject upperHull = firstSlice.CreateUpperHull(target, crossSectionMaterial);
            GameObject lowerHull = firstSlice.CreateLowerHull(target, crossSectionMaterial);

            SlicedHull secondSlice = lowerHull.Slice(secondPlane.position, secondPlane.up);

            if (secondSlice != null)
            {
                GameObject middleHull = secondSlice.CreateUpperHull(target, crossSectionMaterial);
                GameObject finalLowerHull = secondSlice.CreateLowerHull(target, crossSectionMaterial);

                firstPlaneObject.SetActive(false);
                secondPlaneObject.SetActive(false);

                Destroy(lowerHull);
                Destroy(finalLowerHull);
            }

            Destroy(target);
        }
    }
}