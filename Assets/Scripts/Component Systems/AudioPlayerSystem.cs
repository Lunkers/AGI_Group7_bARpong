using Unity.Burst;
using Unity.Entities;
using UnityEngine;



[UpdateAfter(typeof(FixedStepSimulationSystemGroup))] //update after score checking
public class AudioPlayerSystem : SystemBase
{

    protected override void OnUpdate() 
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithAll<AudioPlayTag>().WithStructuralChanges().ForEach((Entity e, AudioSource audioSource) =>
        {
            Debug.Log("Entity with audio play tag");
            Debug.Log(e);
            Debug.Log("Should play sound");
            audioSource.Play();
            entityManager.RemoveComponent<AudioPlayTag>(e);
            //entityManager.DestroyEntity(e);
        }).Run();

        // commandBuffer.Playback(EntityManager);
        // commandBuffer.Dispose();
    }

}