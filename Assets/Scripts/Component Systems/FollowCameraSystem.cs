﻿using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class FollowCameraSystem : SystemBase
{
   protected override void OnUpdate()
    {

        EntityManager entityManager = World.EntityManager;
        Entities.WithoutBurst().ForEach((ref Translation translation, ref Rotation r, in ref Throwable throwable) =>
        {
            if (!throwable.thrown)
            {
                var camData = entityManager.GetComponentObject<Camera>(throwable.camera);
                //Debug.Log(camData.transform.position);
                translation.Value = camData.transform.position + camData.transform.forward * 5;
                throwable.angle = math.radians(-camData.transform.localRotation.eulerAngles.x);
                r.Value = camData.transform.localRotation;
            }

        }).Run();
    }
}