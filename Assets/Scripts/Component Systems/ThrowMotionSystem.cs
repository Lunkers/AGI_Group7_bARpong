using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

/**
    Simple implementation of a throw motion, works on throwable components
**/
public class ThrowMotionSystem : JobComponentSystem
{

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.WithStructuralChanges().ForEach((ref PhysicsVelocity velocity,ref Translation translation, ref Rotation rotation,  ref LaunchInput l, ref Throwable t) =>
        {
            bool launchPressed = Input.GetKey(l.launchKey);
            if (launchPressed)
            {
                Debug.Log("pressed space!");
                velocity.Linear.x = t.initialVelocity * math.cos(t.angle);
                velocity.Linear.y = t.initialVelocity * math.sin(t.angle);
            }

        }).Run();

        return default;
    }
}