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
    public void Launch()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithStructuralChanges().ForEach((ref Entity e, ref Throwable t, ref PhysicsCollider collider) =>
        {
            if (t.thrown)
            {
                return;
            }
            //debug to check input works
            Debug.Log("Launching");
            //add initial velocity
            //get camera direction
            Camera cameraData = Camera.main;
            //var cameraData = entityManager.GetComponentObject<Camera>(t.camera);
            var camDirection = cameraData.transform.forward;
    

            entityManager.AddComponentData(e, new PhysicsVelocity
            {
                Linear = new float3(camDirection.x * t.initialVelocity * math.cos(t.angle), t.initialVelocity * math.sin(t.angle), camDirection.z * t.initialVelocity * math.cos(t.angle))
            });
            //add physics mass to entity
            t.thrown = true;
            entityManager.AddComponentData(e, PhysicsMass.CreateDynamic(collider.MassProperties, t.mass / 1000));
        }).Run();
    }

    //resets the ball back to camera
    public void Reset()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
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