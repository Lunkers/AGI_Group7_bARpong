using UnityEngine;
using Unity.Entities;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(BouncingAudioSystem))]
public class TableBounceAudioSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
    }

    protected override void OnUpdate()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithoutBurst().WithAll<TableBounceTag>().WithStructuralChanges().ForEach((Entity e, AudioSource audioSource) =>
        {
            //Debug.Log("Bouncing on table");
            audioSource.clip = GameManager.instance.audioCollection.BallOnWoodAudio;
            audioSource.Play();
            EntityManager.RemoveComponent<TableBounceTag>(e);
        }).Run();
    }
}
