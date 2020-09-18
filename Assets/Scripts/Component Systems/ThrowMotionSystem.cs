using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

/**
    Simple implementation of a throw motion, works on throwable components
**/
public class ThrowMotionSystem : JobComponentSystem {

    protected override JobHandle OnUpdate(JobHandle inputDeps){
        float time = (float) Time.ElapsedTime;
        Entities.ForEach((ref PhysicsVelocity velocity, ref Translation position, in Throwable throwable) => { 
            //calculate Y velocity
            velocity.Linear.y  = throwable.initialVelocity * math.sin(throwable.angle) - (math.abs(Physics.gravity.y) * time); //use abs since gravity is negative sometimes
            //calculate X velocity
            velocity.Linear.x = throwable.initialVelocity * math.cos(throwable.angle);

        }).Run();

        return default;
    }   
}