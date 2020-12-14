using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DisableTrackedVisuals : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Disables spawned planes and ARPlaneManager")]
    bool _DisablePlaneRendering;

    public bool disablePlaneRendering
    {
        get => _DisablePlaneRendering;
        set => _DisablePlaneRendering = value;
    }

    [SerializeField]
    ARPlaneManager _Planemanager;

    public ARPlaneManager planeManager
    {
        get => _Planemanager;
        set => _Planemanager = value;
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        TablePlacer.onTablePlaced += OnPlacedObject;
    }

    void OnDisable() {
        TablePlacer.onTablePlaced -=OnPlacedObject;
    }

    // Update is called once per frame
    void OnPlacedObject()
    {
        if(_DisablePlaneRendering){
            _Planemanager.SetTrackablesActive(false);
            _Planemanager.enabled = false;
        }
    }
}
