using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FollowCameraSystem : SystemBase
{
    [SerializeField] private GameObject playerPrefab;
   protected override void OnUpdate()
    {

        Entities.WithoutBurst().ForEach((ref Translation translation, ref Rotation r, in ref Throwable throwable) =>
        {
            playerPrefab = Resources.Load("NonARCamera") as GameObject;
            if (!throwable.thrown && !playerPrefab.GetPhotonView().IsMine)
            {
             
                Camera camData = playerPrefab.GetComponent<Camera>(); //Camera camData = Camera.main;
             
                //Debug.Log(camData.transform.position);
                translation.Value = camData.transform.position + (camData.transform.forward * 0.2f) - (camData.transform.up * 0.03f);
                throwable.angle = math.radians(-camData.transform.localRotation.eulerAngles.x);
                r.Value = camData.transform.localRotation;
            }

        }).Run();
    }
}
