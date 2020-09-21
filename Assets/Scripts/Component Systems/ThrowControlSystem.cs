using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class NewBehaviourScript : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        Entities.ForEach((ref LaunchInput l, ref Throwable t) => {
            if(Input.GetKey(l.angleIncrease)){
                t.angle +=  0.01f;
                Debug.Log(t.angle);
                return;
            }
            if(Input.GetKey(l.angleDecrease)){
                t.angle -= 0.01f;
                Debug.Log(t.angle);
                return;
            }
        }).Run();
        return default;
    }
}
