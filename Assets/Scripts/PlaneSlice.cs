using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlaneSlice : MonoBehaviour
{
    public Transform firstPlane;
    public Transform secondPlane;
    public GameObject target;
    public Material crossSectionMaterial;

    public AudioClip crackingSound;
    private AudioSource audioSource;

    private Stack<GameObject> previousSlices = new Stack<GameObject>(); // Stack to store previous slices
    private GameObject clonedObject; // Reference to the cloned object
    private bool isClonedHidden = true; // Flag to track cloned object visibility

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource component if not already present
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = crackingSound;
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Slice(target);
            PlayCrackingSound(); // Play the cracking sound
        }

        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            UndoSlice();
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

            // Store the sliced object state into the stack
            GameObject slicedObject = Instantiate(target);
            previousSlices.Push(slicedObject);

            // Hide the cloned object
            clonedObject = slicedObject;
            SetObjectVisibility(clonedObject, false);
            isClonedHidden = true;
        }
    }

    void PlayCrackingSound()
    {
        if (crackingSound != null && audioSource != null)
        {
            audioSource.Play();
        }
    }

    void UndoSlice()
    {
        if (previousSlices.Count > 0)
        {
            DestroyHulls(); // Destroy the created hulls

            // Restore the previous state by popping from the stack and setting it as the new target
            GameObject previousState = previousSlices.Pop();
            Destroy(target); // Destroy the current sliced object
            target = previousState;

            // Show the cloned object
            SetObjectVisibility(clonedObject, true);
            isClonedHidden = false;
        }
    }
    void DestroyHulls()
    {
        // Your logic to destroy the hulls created after slicing
        // For example:
        // Destroy(upperHull);
        // Destroy(lowerHull);
        // Destroy(middleHull);
        // Destroy(finalLowerHull);
    }

    void SetObjectVisibility(GameObject obj, bool isVisible)
    {
        if (obj != null)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                rend.enabled = isVisible;
            }
        }
    }

}
