using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class HapticsManager : MonoBehaviour
{
    public static HapticsManager Instance { get; private set; }

    private InputDevice leftInputDevice;
    private InputDevice rightInputDevice;

    [Header("General")]

    [SerializeField]
    bool disableHaptics = false;

    [Header("Button Hover")]

    [SerializeField]
    float buttonHoverHapticAmplitude;

    [SerializeField]
    float buttonHoverHapticDuration;

    [Header("Gizmo Hover")]
    
    [SerializeField]
    float gizmoHoverHapticAmplitude;

    [SerializeField]
    float gizmoHoverHapticDuration;

    [Header("Radial Pick")]

    [SerializeField]
    float radialPickHapticAmplitude;

    [SerializeField]
    float radialPickHapticDuration;

    [Header("Button Pressed")]

    [SerializeField]
    float buttonClickHapticAmplitude;

    [SerializeField]
    float buttonClickHapticDuration;

    [Header("Actor Selected")]

    [SerializeField]
    float actorSelectedHapticAmplitude;

    [SerializeField]
    float actorSelectedHapticDuration;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        InitDevices();

        InputDevices.deviceConnected += OnDeviceConnected;
        InputDevices.deviceDisconnected += OnDeviceDisconnected;
    }

    void OnDisable()
    {
        InputDevices.deviceConnected -= OnDeviceConnected;
        InputDevices.deviceDisconnected -= OnDeviceDisconnected;
    }

    void OnDeviceConnected(InputDevice device)
    {
        DetectDevice(device);
    }

    void OnDeviceDisconnected(InputDevice device)
    {
        if(device == leftInputDevice)
        {
            leftInputDevice = default;
        }

        if(device == rightInputDevice)
        {
            rightInputDevice = default;
        }
    }

    void DetectDevice(InputDevice device)
    {
        if(!device.isValid) return;

        var characteristics = device.characteristics;

        bool isController = (characteristics & InputDeviceCharacteristics.HeldInHand) != 0 && (characteristics & InputDeviceCharacteristics.Controller) != 0;

        if(!isController) return;

        bool isLeft = (characteristics & InputDeviceCharacteristics.Left) != 0;
        bool isRight = (characteristics & InputDeviceCharacteristics.Right) != 0;

        if(isLeft)
        {
            leftInputDevice = device;
        }

        if(isRight)
        {   
            rightInputDevice = device;
        }
    }

    void InitDevices()
    {
        var leftHandDevices = new List<InputDevice>();
        var rightHandDevices = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);

        if(leftHandDevices.Count > 0)
        {
            leftInputDevice = leftHandDevices[0];
        }

        if(rightHandDevices.Count > 0)
        {
            rightInputDevice = rightHandDevices[0];
        }
    }

    public static void PlayHapticButtonHover(InputDeviceRole inputDeviceRole)
    {
        SendHapticToDevice(inputDeviceRole, Instance.buttonHoverHapticAmplitude, Instance.buttonHoverHapticDuration);
    }

    public static void PlayHapticGizmoHover(InputDeviceRole inputDeviceRole)
    {
        SendHapticToDevice(inputDeviceRole, Instance.gizmoHoverHapticAmplitude, Instance.gizmoHoverHapticDuration);
    }

    public static void PlayHapticRadialPick(InputDeviceRole inputDeviceRole)
    {
        SendHapticToDevice(inputDeviceRole, Instance.radialPickHapticAmplitude, Instance.radialPickHapticDuration);
    }

    public static void PlayHapticButtonClick(InputDeviceRole inputDeviceRole)
    {
        SendHapticToDevice(inputDeviceRole, Instance.buttonClickHapticAmplitude, Instance.buttonClickHapticDuration);
    }

    public static void PlayHapticActorSelected(InputDeviceRole inputDeviceRole)
    {
        SendHapticToDevice(inputDeviceRole, Instance.actorSelectedHapticAmplitude, Instance.actorSelectedHapticDuration);
    }

    private static void SendHapticToDevice(InputDeviceRole inputDeviceRole, float amplitude, float duration)
    {
        if(inputDeviceRole == InputDeviceRole.LeftHanded && Instance.leftInputDevice.isValid)
        {
            SendHaptic(Instance.leftInputDevice, amplitude, duration);
        }
        else if (inputDeviceRole == InputDeviceRole.RightHanded && Instance.rightInputDevice.isValid)
        {
            SendHaptic(Instance.rightInputDevice, amplitude, duration);
        }
    }

    private static void SendHaptic(InputDevice device, float amplitude, float duration)
    {
        if (!device.isValid) return;

        if (Instance.disableHaptics) return;

        HapticCapabilities hapticCapabilities;
        if(device.TryGetHapticCapabilities(out hapticCapabilities) && hapticCapabilities.supportsImpulse)
        {
            device.SendHapticImpulse(0, amplitude, duration);
        }
    }
}
