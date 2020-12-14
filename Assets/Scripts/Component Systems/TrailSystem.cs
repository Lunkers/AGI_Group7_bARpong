using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class TrailSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((TrailRenderer trailRenderer, ref Throwable throwable) =>
        {
            trailRenderer.enabled = throwable.thrown;
        }).WithoutBurst().Run();
    }
}
