using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

/**
    Simple implementation of a throw motion, works on throwable components
**/
public class ThrowMotionSystem : SystemBase
{


    protected override void OnUpdate()
    {
        return;
    }

    //Adds physics properties to the throwables, and "launches" them
    public void Launch(float initialVelocity, float angle)
    {
        EntityManager entityManager = EntityManager;
        Entities.WithStructuralChanges().ForEach((ref Entity e, ref Throwable t, ref PhysicsCollider collider) =>
        {
            if (t.thrown)
            {
                return;
            }
            
            //add initial velocity
            //get camera direction
            var cameraData = Camera.main;
            var camDirection = cameraData.transform.forward;

            entityManager.AddComponentData(e, new PhysicsVelocity
            {
                Linear = new float3(camDirection.x * initialVelocity * math.cos(angle), initialVelocity * math.sin(angle), camDirection.z * initialVelocity * math.cos(angle))
            });
            //add physics mass to entity
            t.thrown = true;
            entityManager.AddComponentData(e, PhysicsMass.CreateDynamic(collider.MassProperties, t.mass / 1000));
        }).Run();
    }

    public void Launch(float3 vector)
    {
    }

    //resets the ball back to camera
    public void Reset()
    {
        EntityManager entityManager = EntityManager;
        Entities.WithStructuralChanges().ForEach((ref Entity e,ref PhysicsVelocity velocity, ref Throwable throwable) =>
        {
            if (throwable.thrown)
            {
                throwable.thrown = !throwable.thrown;
                EntityManager.RemoveComponent(e, typeof(PhysicsMass));
                velocity.Linear = new float3(0, 0, 0);
            }
        }).Run();
    }
}