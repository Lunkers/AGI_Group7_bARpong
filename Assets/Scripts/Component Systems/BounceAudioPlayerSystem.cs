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
            Debug.Log("Entity with audio play tag");
            Debug.Log(e);
            Debug.Log("Should play sound");
            audioSource.clip = GameSettings.audioCollection.BallOnCupAudio;
            audioSource.Play();
            entityManager.RemoveComponent<BouncePlayTag>(e);
            //entityManager.DestroyEntity(e);
        }).Run();

        // commandBuffer.Playback(EntityManager);
        // commandBuffer.Dispose();
    }
}
