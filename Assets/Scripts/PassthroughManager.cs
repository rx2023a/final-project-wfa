using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughManager : MonoBehaviour
{
    public OVRPassthroughLayer passtrough;
    public OVRInput.Button button;
    public OVRInput.Touch touch;
    public OVRInput.Controller controller;

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(button, controller)) 
        {
            passtrough.hidden = !passtrough.hidden;
        }
    }
}
