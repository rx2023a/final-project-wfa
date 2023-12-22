using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughManager : MonoBehaviour
{
    public OVRPassthroughLayer passthrough;
    public OVRInput.Button button;
    public OVRInput.Touch touch;
    public OVRInput.Controller controller;

    private void Start()
    {
        if (OVRManager.IsPassthroughRecommended())
        {
            passthrough.enabled = true;

            // Set camera background to transparent
            OVRCameraRig ovrCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
            var centerCamera = ovrCameraRig.centerEyeAnchor.GetComponent<Camera>();
            centerCamera.clearFlags = CameraClearFlags.SolidColor;
            centerCamera.backgroundColor = Color.clear;

            // Ensure your VR background elements are disabled
        }
        else
        {
            passthrough.enabled = false;

            // Ensure your VR background elements are enabled
        }
    }
    void Update()
    {
        if(OVRInput.GetDown(button, controller)) 
        {
            passthrough.hidden = !passthrough.hidden;
        }
    }
}
