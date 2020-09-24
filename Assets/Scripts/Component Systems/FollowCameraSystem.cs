using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class FollowCameraSystem : SystemBase
{
   protected override void OnUpdate()
    {

        EntityManager entityManager = EntityManager;
        Entities.WithoutBurst().ForEach((ref Translation translation, ref Rotation r, in ref Throwable throwable) =>
        {
            if (!throwable.thrown)
            {
                var camData = entityManager.GetComponentObject<Camera>(throwable.camera);
                //Debug.Log(camData.transform.position);
                translation.Value = camData.transform.position + camData.transform.forward * 5;
            }

        }).Run();
    }
}
