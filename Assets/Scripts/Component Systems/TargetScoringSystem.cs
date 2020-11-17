using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))] // run after simulation is done for frame
public class TargetScoringSystem : SystemBase
{
    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepPhysicsWorld;
    EntityQuery scoreTriggerGroup;
    EndFixedStepSimulationEntityCommandBufferSystem bufferSystem;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        bufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
        scoreTriggerGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                typeof(ScoringCollider)
            }
        });
    }

    [BurstCompile]
    struct TriggerScoringJob: ITriggerEventsJob
    {
        public ComponentDataFromEntity<ScoringCollider> scoringColliderGroup;
        public ComponentDataFromEntity<Throwable> throwableGroup;
        public ComponentDataFromEntity<PhysicsVelocity> physicsVelocityGroup;
        public EntityCommandBuffer commandBuffer;


        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isBodyATrigger = scoringColliderGroup.HasComponent(entityA);
            bool isBodyBTrigger = scoringColliderGroup.HasComponent(entityB);

            //ignore triggers overlapping each other
            if (isBodyATrigger && isBodyBTrigger)
            {
                return;
            }

            // ignore non-scoring triggers as well
            if(!isBodyATrigger && !isBodyBTrigger)
            {
                return;
            }

            // NOTE: At the moment, the cups are static in the physics world.
            // We will want to add collision physics to them later though.
            bool isBodyADynamic = physicsVelocityGroup.HasComponent(entityA);
            bool isBodyBDynamic = physicsVelocityGroup.HasComponent(entityB);

            //Ignore overlapping static bodies
            if ((isBodyBTrigger && !isBodyADynamic) || (isBodyATrigger && !isBodyBDynamic))
            {
                return;
            }

            var goalEntity = isBodyATrigger ? entityA : entityB;
            var throwableEntity = isBodyATrigger ? entityB : entityA;

            //reset ball
            //{
            //    var throwable = throwableGroup[throwableEntity];
            //    throwable.thrown = false;
            //    //remove velocity here
            //    var velocity = physicsVelocityGroup[throwableEntity];
            //    velocity.Linear = new float3(0, 0, 0);
            //    physicsVelocityGroup[throwableEntity] = velocity;
            //    ////remove velocity
            //    commandBuffer.RemoveComponent(throwableEntity, typeof(PhysicsMass));
            //}

            // handle cup
            ScoringCollider goalComponent = scoringColliderGroup[goalEntity];
            // TODO: implement scoring here

            //remove cup if it should be deleted;
            if (goalComponent.deleteOnScore)
            {
                //commandBuffer.DestroyEntity(goalComponent.water);
                commandBuffer.DestroyEntity(goalComponent.cup);
                commandBuffer.AddComponent(goalEntity, new DeleteTag()); // tag for score increment/deletion
                
            }
        }
    }

    protected override void OnUpdate()
    {
        if(scoreTriggerGroup.CalculateEntityCount() == 0)
        {
            // here we could trigger a game state change, since there are no cups in the scene
            return;
        }

        Dependency = new TriggerScoringJob
        {
            scoringColliderGroup = GetComponentDataFromEntity<ScoringCollider>(),
            throwableGroup = GetComponentDataFromEntity<Throwable>(),
            physicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>(),
            commandBuffer = bufferSystem.CreateCommandBuffer(),

        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);

        bufferSystem.AddJobHandleForProducer(Dependency);
    }
}
