using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(FixedStepSimulationSystemGroup))] //update after score checking
public class IncreaseScoreSystem : SystemBase
{

    protected override void OnUpdate() 
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithAll<DeleteTag>().WithStructuralChanges().ForEach((Entity e, in ScoringCollider sc) =>
        {
            GameManager.instance.IncreaseScore();
            
            //entityManager.DestroyEntity(e);
        }).Run();

        // commandBuffer.Playback(EntityManager);
        // commandBuffer.Dispose();
    }

}