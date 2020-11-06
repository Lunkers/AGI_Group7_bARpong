using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class AnimationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        EntityManager entityManager = World.EntityManager;
        Entities.WithoutBurst().ForEach((ref Translation translation, in ref Animatable animatable) =>
        {
            if (animatable.animating)
            {
                var t = (Time.ElapsedTime-animatable.initialTime)/animatable.targetTime;

                translation.Value.x = Mathf.Lerp(animatable.initialPosition.x, animatable.targetPosition.x, Mathf.SmoothStep(0.0f, 1.0f, (float)t));
                translation.Value.y = Mathf.Lerp(animatable.initialPosition.y, animatable.targetPosition.y, Mathf.SmoothStep(0.0f, 1.0f, (float)t));
                translation.Value.z = Mathf.Lerp(animatable.initialPosition.z, animatable.targetPosition.z, Mathf.SmoothStep(0.0f, 1.0f, (float)t));

                if (t >= 1.0f)
                {
                    translation.Value = animatable.targetPosition;
                    animatable.animating = false;
                }
            }
        }).Run();
    }
}
