using Unity.Burst;
using Unity.Entities;
using UnityEngine;



[UpdateAfter(typeof(FixedStepSimulationSystemGroup))] //update after score checking
public class AudioPlayerSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        //Debug.Log("Creating Audio system");
    }

    protected override void OnUpdate() 
    {
        //Debug.Log("Audio system triggered!");
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithoutBurst().WithAll<AudioPlayTag>().WithStructuralChanges().ForEach((Entity e, AudioSource audio) =>
        {
            // Debug.Log("Entity with audio play tag");
            // Debug.Log(e);
            // Debug.Log("Audio Source");
            //Debug.Log(audio);
            // Debug.Log("Should play sound");
            audio.clip = GameManager.instance.audioCollection.ScorePointAudio;
            audio.Play();
            entityManager.RemoveComponent<AudioPlayTag>(e);
            //entityManager.DestroyEntity(e);
        }).Run();

        // commandBuffer.Playback(EntityManager);
        // commandBuffer.Dispose();
    }

}