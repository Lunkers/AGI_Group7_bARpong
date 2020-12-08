using Unity.Entities;
using UnityEngine;


[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(BouncingAudioSystem))]
public class BounceAudioPlayerSystem : SystemBase
{
   protected override void OnCreate()
    {
        base.OnCreate();
    }
    protected override void OnUpdate() 
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithoutBurst().WithAll<CupBounceTag>().WithStructuralChanges().ForEach((Entity e, AudioSource audioSource) =>
        {
            //Debug.Log("Bouncing on cup");
            audioSource.clip = GameManager.instance.audioCollection.BallOnCupAudio;
            audioSource.Play();
            entityManager.RemoveComponent<CupBounceTag>(e);

            //entityManager.DestroyEntity(e);
        }).Run();

        // commandBuffer.Playback(EntityManager);
        // commandBuffer.Dispose();
    }
    
}
