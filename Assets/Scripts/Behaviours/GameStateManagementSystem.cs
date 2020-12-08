using Unity.Burst;
using Unity.Entities;
using UnityEngine;

public enum GameStates
{
    Placing,
    Playing,
    Won,
    Lost
}

[UpdateAfter(typeof(FixedStepSimulationSystemGroup))] //update after score checking
public class GameStateManagementSystem : SystemBase
{

    protected override void OnUpdate() 
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithAll<DeleteTag>().WithStructuralChanges().ForEach((Entity e) =>
        {
            GameManager.instance.IncreaseScore();
            
            entityManager.DestroyEntity(e);
        }).Run();

        // commandBuffer.Playback(EntityManager);
        // commandBuffer.Dispose();
    }

}