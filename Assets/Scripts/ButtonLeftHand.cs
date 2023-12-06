using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class ButtonLeftHand : MonoBehaviour
{
    public XRController controller; // Reference to the VR controller that will interact with the button
    public UnityEvent onClick; // Event to trigger when the button is clicked
    public Material pressedMaterial;
    public Material defaultMaterial;

    private XRBaseInteractable interactable;

    private void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.onSelectEntered.AddListener(OnButtonPress); // Add listener for button press event
        //interactable.onSelectEnter.AddListener(OnSelectEnter);

        SetMaterial(defaultMaterial);
    }

    private void OnButtonPress(XRBaseInteractor interactor)
    {
        if (interactor.gameObject == controller.gameObject)
        {
            // Trigger onClick event when the button is pressed by the specified controller
            SetMaterial(pressedMaterial);
        }
    }

    private void SetMaterial(Material material)
    {
        Renderer buttonRenderer = GetComponent<Renderer>();
        if (buttonRenderer != null)
        {
            buttonRenderer.material = material;
        }
    }

    //private void OnSelectEnter(XRBaseInteractor interactor)
    //{
    //    SetMaterial(defaultMaterial);
    //}

    //private void OnSelectExit(XRBaseInteractor interactor)
    //{
    //    SetMaterial(pressedMaterial);
    //}
}
