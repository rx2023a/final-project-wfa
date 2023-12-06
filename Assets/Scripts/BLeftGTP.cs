using UnityEngine;
using Oculus;

public class BLeftGTP : MonoBehaviour
{
    private OVRGrabbable grabbable;
    private Renderer rend;
    private Color originalColor;
    public Color grabColor = Color.blue; // Change this to the desired grab color

    void Start()
    {
        Debug.Log("starting...");
        grabbable = GetComponent<OVRGrabbable>();
        rend = GetComponent<Renderer>();
        Debug.Log("done getting component...");
        if (grabbable != null && rend != null)
        {
            Debug.Log("reading material color.");
            originalColor = rend.material.color;

            grabbable.OnGrabBegin += OnGrabBegin;
            Debug.Log("ongrabbegin.");
            grabbable.OnGrabEnd += OnGrabEnd;
            Debug.Log("ongrabbend.");
        }
        else
        {
            Debug.LogWarning("Missing OVRGrabbable or Renderer component.");
        }
    }

    private void OnGrabBegin()
    {
        Debug.Log("amogus123");
        if (rend != null)
        {
            Debug.Log("amogus");
            rend.material.color = grabColor;
        }
    }

    private void OnGrabEnd()
    {
        if (rend != null)
        {
            rend.material.color = originalColor;
        }
    }
}