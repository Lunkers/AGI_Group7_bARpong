using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Collider = Unity.Physics.Collider;
using UnityEngine;

/**
    Simple implementation of a throw motion, works on throwable components
**/
public class ThrowMotionSystem : JobComponentSystem
{

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityManager entityManager = EntityManager;
        Entities.WithStructuralChanges().ForEach((ref Entity e, ref LaunchInput l, ref Throwable t, ref PhysicsCollider collider) =>
        {
            bool launchPressed = Input.GetKey(l.launchKey);
            if (launchPressed)
            {
                //debug to check input works
                Debug.Log("pressed space");

                //add initial velocity
                entityManager.AddComponentData(e, new PhysicsVelocity{
                    Linear = new float3(t.initialVelocity * math.cos(t.angle), t.initialVelocity * math.sin(t.angle), 0)
                });
                //add physics mass to entity
                entityManager.AddComponentData(e, PhysicsMass.CreateDynamic(collider.MassProperties, 1f));

            }

        }).Run();

        return default;
    }
}