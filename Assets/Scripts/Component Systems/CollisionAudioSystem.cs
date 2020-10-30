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





[UpdateAfter(typeof(EndFramePhysicsSystem))]
//[UpdateBefore(typeof(PlayCollisionAudioSystem))]
public class CollisionAudioSystem : SystemBase
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    EndFixedStepSimulationEntityCommandBufferSystem bufferSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        bufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();

    }


    struct CollisionAudioSystemJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<AudioTableTag> tableObject;
        [ReadOnly] public ComponentDataFromEntity<AudioCupTag> cupObject;
        [ReadOnly] public ComponentDataFromEntity<Throwable> ballObject;

        public ComponentDataFromEntity<AudioComponent> audioGroup;
        public void Execute(CollisionEvent collisionEvent)
        {
           
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;
            Debug.Log(entityA);
            bool entityAIsTable = tableObject.HasComponent(entityA);
            bool entityBIsTable = tableObject.HasComponent(entityB);
            bool entityAIsCup = cupObject.HasComponent(entityA);
            bool entityBIsCup = cupObject.HasComponent(entityB);
            bool entityAIsBall = ballObject.HasComponent(entityA);
            bool entityBIsBall = ballObject.HasComponent(entityB);

            if(entityAIsCup && entityBIsBall)
            {
                AudioComponent modifiedAudiogroup = audioGroup[entityB];
                modifiedAudiogroup.isCup = true;
                modifiedAudiogroup.isTable = false;
                audioGroup[entityB] = modifiedAudiogroup;
            }
            if(entityAIsBall && entityBIsCup)
            {
                AudioComponent modifiedAudiogroup = audioGroup[entityA];
                modifiedAudiogroup.isCup = true;
                modifiedAudiogroup.isTable = false;
                audioGroup[entityA] = modifiedAudiogroup;
            }
            if (entityAIsTable && entityBIsBall)
            {
                AudioComponent modifiedAudiogroup = audioGroup[entityB];
                modifiedAudiogroup.isTable = true;
                modifiedAudiogroup.isCup = false;
                audioGroup[entityB] = modifiedAudiogroup;
            }
            if (entityAIsBall && entityBIsTable)
            {
                AudioComponent modifiedAudiogroup = audioGroup[entityA];
                modifiedAudiogroup.isTable = true;
                modifiedAudiogroup.isCup = false;
                audioGroup[entityA] = modifiedAudiogroup;
            }
        }
    }
    protected override void OnUpdate()
    {
        Dependency = new CollisionAudioSystemJob
        {
            tableObject = GetComponentDataFromEntity<AudioTableTag>(true),
            cupObject = GetComponentDataFromEntity<AudioCupTag>(true),
            ballObject = GetComponentDataFromEntity<Throwable>(true),
            audioGroup = GetComponentDataFromEntity<AudioComponent>(false),
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);
        bufferSystem.AddJobHandleForProducer(Dependency);
    }
}
     