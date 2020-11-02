using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Light))]
public class LightEstimation : MonoBehaviour
{
    // <summary>
    // Component for basic light estimation based on camera data
    // Based on https://github.com/Unity-Technologies/arfoundation-samples/blob/latest-preview/Assets/Scripts/BasicLightEstimation.cs 
    // </summary>
    // Start is called before the first frame update
    [SerializeField]
    ARCameraManager p_CameraManager;

    public ARCameraManager cameraManager
    {
        get { return p_CameraManager; }
        set
        {
            if (p_CameraManager == value)
                return;
            if (p_CameraManager != null)
                p_CameraManager.frameReceived -= FrameChanged;
            p_CameraManager = value;

            if (p_CameraManager != null & enabled)
                p_CameraManager.frameReceived += FrameChanged;
        }
    }

    public float? brightness { get; private set; } //estimated brightness
    public float? colorTemp { get; private set; } //light temp
    public Color? colorCorrection { get; private set; }

    public Vector3? mainLightDir {get; private set;}
    public Color? mainLightColor {get; private set;}
    public float? mainLightIntensity {get; private set;}

    void Awake()
    {
        m_Light = GetComponent<Light>();
    }

    void OnEnable()
    {
        if (p_CameraManager != null)
            p_CameraManager.frameReceived += FrameChanged;
    }
    void OnDisable() {
        if(p_CameraManager != null)
            p_CameraManager.frameReceived -= FrameChanged;    
    }

    void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            brightness = args.lightEstimation.averageBrightness.Value;
            m_Light.intensity = brightness.Value;
        }
        else
        {
            brightness = null;
        }
        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            colorTemp = args.lightEstimation.averageColorTemperature.Value;
            m_Light.colorTemperature = colorTemp.Value;
        }
        else
        {
            colorTemp = null;
        }
        if (args.lightEstimation.colorCorrection.HasValue)
        {
            colorCorrection = args.lightEstimation.colorCorrection.Value;
            m_Light.color = colorCorrection.Value;
        }
        else
        {
            colorCorrection = null;
        }
        if(args.lightEstimation.mainLightDirection.HasValue) {
            mainLightDir = args.lightEstimation.mainLightDirection.Value;
            m_Light.transform.rotation = Quaternion.LookRotation(mainLightDir.Value);
        }
        else {
            mainLightDir = null;
        }
        if(args.lightEstimation.mainLightColor.HasValue) {
            mainLightColor = args.lightEstimation.mainLightColor;
            m_Light.color = mainLightColor.Value;
        }
        else {
            mainLightColor = null;
        }
        if(args.lightEstimation.mainLightIntensityLumens.HasValue) {
            mainLightIntensity = args.lightEstimation.mainLightIntensityLumens.Value;
            m_Light.intensity = mainLightIntensity.Value;
        }
        else {
            mainLightIntensity = null;
        }
    }

    Light m_Light;
}
