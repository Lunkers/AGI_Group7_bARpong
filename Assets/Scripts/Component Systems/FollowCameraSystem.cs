using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class FollowCameraSystem : SystemBase
{
   protected override void OnUpdate()
    {

        Entities.WithoutBurst().ForEach((ref Translation translation, ref Rotation r, in ref Throwable throwable) =>
        {
            if (!throwable.thrown)
            {
                Camera camData = Camera.main;

                //Debug.Log(camData.transform.position);
                translation.Value = camData.transform.position + (camData.transform.forward * 0.2f)  - (camData.transform.up * 0.03f) ;
                throwable.angle = math.radians(-camData.transform.localRotation.eulerAngles.x);
                r.Value = camData.transform.localRotation;
            }

        }).Run();
    }
}
