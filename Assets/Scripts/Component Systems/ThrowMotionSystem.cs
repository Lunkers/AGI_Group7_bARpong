using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.GraphicsIntegration;
using UnityEngine;

/**
    Simple implementation of a throw motion, works on throwable components
**/
public class ThrowMotionSystem : SystemBase
{


    protected override void OnUpdate()
    {
        //check for throwables with 0 velocity, and reset them
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithAll<ResetTag>().WithStructuralChanges().ForEach((ref Entity e, ref PhysicsVelocity velocity, ref Throwable throwable) =>
        {
            if (throwable.thrown)
            {
                throwable.thrown = !throwable.thrown;
                entityManager.RemoveComponent(e, typeof(PhysicsMass));
                entityManager.RemoveComponent(e, typeof(ResetTag));
                velocity.Linear = new float3(0, 0, 0);
            }
        }).Run();
        return;
    }

    //Adds physics properties to the throwables, and "launches" them
    public void Launch(float velocity)
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithStructuralChanges().ForEach((ref Entity e, ref Throwable t, ref PhysicsCollider collider) =>
        {
            if (t.thrown)
            {
                return;
            }

            //add initial velocity
            //get camera direction
            Camera cameraData = Camera.main;
            //var cameraData = entityManager.GetComponentObject<Camera>(t.camera);
            var camDirection = cameraData.transform.forward;
            
            // Calculate the throw angle
            var cameraAngle = Vector3.Angle(Vector3.up, Camera.main.transform.forward);
            var t_angle = Mathf.Abs((cameraAngle-90.0f)/90.0f);
            var angle = Mathf.Lerp(Mathf.PI/4.0f, 0.0f, t_angle);

            entityManager.AddComponentData(e, new PhysicsVelocity
            {
                Linear = new float3(camDirection.x * velocity * math.cos(angle), velocity * math.sin(angle), camDirection.z * velocity * math.cos(angle))
            });
            //add physics mass to entity
            t.thrown = true;
            entityManager.AddComponentData(e, PhysicsMass.CreateDynamic(collider.MassProperties, t.mass / 1000));
            entityManager.AddComponentData(e, new PhysicsGraphicalSmoothing {
                ApplySmoothing = 1,
            });
        }).Run();
    }

    //resets the ball back to camera
    public void Reset()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithStructuralChanges().ForEach((ref Entity e, ref PhysicsVelocity velocity, ref Throwable throwable) =>
        {
            if (throwable.thrown)
            {
                throwable.thrown = !throwable.thrown;
                entityManager.RemoveComponent(e, typeof(PhysicsMass));
                velocity.Linear = new float3(0, 0, 0);
            }
        }).Run();
    }
}