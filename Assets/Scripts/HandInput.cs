using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandInput : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public bool right = true;
    private PlayerMovementScript playerMovement;

    private InputDevice inputDevice;
    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
        this.playerMovement = GameObject.Find("First Person Player").GetComponent<PlayerMovementScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (right)
        {
            if (inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
            {
                this.playerMovement.primaryButtonDown();
            }
            if (inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
            {
                this.playerMovement.triggerXR(triggerValue);
            }
            if (inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
            {
                this.playerMovement.axis2DXR(primary2DAxisValue);
            }
        }
    }
}
