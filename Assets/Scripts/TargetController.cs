using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject fibula;
    public GameObject mandibula;
    public GameObject currentTarget;

    public Color highlightColor;
    public Color originalColor;

    void Start()
    {
        SetTarget(fibula);
    }
    
    public void SwitchTarget()
    {
        if (currentTarget == fibula)
        {
            SetTarget(mandibula);
        }
        else
        {
            SetTarget(fibula);
        }

        SetObjectColor(currentTarget, highlightColor);
    }

    public void SetTarget(GameObject newTarget)
    {
        Debug.Log("Setting target to: " + newTarget.name);
        if (currentTarget != null)
        {
            //Reset color of the previous target
            SetObjectColor(currentTarget, originalColor);
        }

        currentTarget = newTarget;

        if (currentTarget != null)
        {
            // Highlight the new target
            SetObjectColor(currentTarget, highlightColor);
        }
    }

    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }

    private void SetObjectColor(GameObject obj, Color color)
    {
        if (obj != null)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                Material material = rend.material;
                material.SetColor("_Color", color);
            }
        }
    }
}
