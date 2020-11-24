using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultiplayerTablePlacer : MonoBehaviourPunCallbacks
{
    public GameObject table;
    public GameObject placementIndicator;
    public GameObject ball;

    [SerializeField] private GameObject cameraPrefab = null;
    private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;
    private ARPlane plane;
    private NetworkManager network;
    private bool validPlane
    {
        get
        {
            return plane != null && plane.alignment == PlaneAlignment.HorizontalUp;
        }
    }

    private Pose placementPose;
    private bool validPose = false;
    private ARPlane currentPlane;


    void Start()
    {
        InstantiateCameraPrefab();
        raycastManager = FindObjectOfType<ARRaycastManager>();
        planeManager = FindObjectOfType<ARPlaneManager>();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        
        if (validPose && validPlane && DidPress())
        {
            //If only host should be able to place table
            PlaceTable();
        }
    }

    private bool DidPress()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                return true;
            }
        }

        return false;
    }
    private void InstantiateCameraPrefab()
    {
        //if 1st player -> en kamera med en position på ena sidan
        //if (PhotonNetwork.PlayerList)
        var camera = PhotonNetwork.Instantiate("AR Camera", placementIndicator.transform.position, Quaternion.identity, 0);
        camera.transform.SetParent(gameObject.transform);
    }

    private void PlaceTable()
    {
        var cameraForward = Camera.current.transform.forward;
        var cameraBearing = new Vector3(cameraForward.x, 0.0f, cameraForward.z).normalized;
        var rotation = Quaternion.LookRotation(cameraBearing);
        if (PhotonNetwork.LocalPlayer.UserId.Equals(2001))
        {
            PhotonNetwork.Instantiate("PhysicsPongBall", placementPose.position, rotation);
            PhotonNetwork.Instantiate("TableWithCups", placementPose.position, rotation);
        }
        else
        {
            PhotonNetwork.Instantiate("PhysicsPongBall", placementPose.position, rotation);
        }


        Destroy(this);
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.2f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        validPose = hits.Count > 0;

        if (validPose)
        {
            plane = planeManager.GetPlane(hits[0].trackableId);
            placementPose = hits[0].pose;
        }
        else {
            plane = null;
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (validPose && validPlane)
        {
            placementIndicator.SetActive(true);

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0.0f, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void OnDestroy() {
        //set state on destruction
        GameManager.instance.tablePlaced = true;
    }
}
