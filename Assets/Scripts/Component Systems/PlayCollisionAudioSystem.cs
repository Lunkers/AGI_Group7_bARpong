using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Physics;
using UnityEngine;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics.Systems;
using UnityEngine.UIElements;

[UpdateAfter(typeof(CollisionAudioSystem))]
public class PlayCollisionAudioSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;
    private AudioClip hitCup;
    private AudioClip ballBounce;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        hitCup = Resources.Load("HitCup") as AudioClip;
        ballBounce = Resources.Load("BallBounce") as AudioClip;
    }
    protected override void OnUpdate()
    {

        EntityCommandBuffer entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();
        Entities.WithoutBurst().ForEach((Entity e, in AudioComponent audioData) =>
        {

            var audioSource = EntityManager.GetComponentObject<AudioSource>(audioData.AudioSource);

            if (audioData.isCup)
            {
                Debug.Log("SHOULD PLAY HITCUP");
                audioSource.PlayOneShot(hitCup);
                AudioComponent temp = audioData;
                temp.isCup = false;
                EntityManager.SetComponentData(e, temp);
            }
            if(audioData.isTable)
            {
                Debug.Log("SHOULD PLAY ballbounce");
                audioSource.PlayOneShot(ballBounce);
                AudioComponent temp = audioData;
                temp.isTable = false;
                EntityManager.SetComponentData(e, temp);
                
            }
        }).Run();
      

    }
}
