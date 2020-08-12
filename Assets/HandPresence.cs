using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDeviceCharacteristics characteristics = InputDeviceCharacteristics.HandTracking;

        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);

        Debug.Log("start");
        foreach (InputDevice item in devices)
        {
            Debug.Log(item.name + " | " + item.characteristics);
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
