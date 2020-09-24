﻿using Unity.Entities;
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

    public void Launch()
    {
        EntityManager entityManager = EntityManager;
        Entities.WithStructuralChanges().ForEach((ref Entity e, ref LaunchInput l, ref Throwable t, ref PhysicsCollider collider) =>
        {
            if (t.thrown)
            {
                return;
            }
            //debug to check input works
            Debug.Log("Launching");
            //add initial velocity
            //get camera direction
            var cameraData = entityManager.GetComponentObject<Camera>(t.camera);
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
}