using Unity.Entities;
using UnityEngine;



[UpdateAfter(typeof(FixedStepSimulationSystemGroup))] //update after score checking
public class BounceAudioPlayerSystem : SystemBase
{

    protected override void OnUpdate() 
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithAll<BouncePlayTag>().WithStructuralChanges().ForEach((Entity e, AudioSource audioSource) =>
        {
            audioSource.clip = GameManager.instance.audioCollection.BallOnCupAudio;
            audioSource.Play();
            entityManager.RemoveComponent<BouncePlayTag>(e);
            //entityManager.DestroyEntity(e);
        }).Run();

        // commandBuffer.Playback(EntityManager);
        // commandBuffer.Dispose();
    }
}
