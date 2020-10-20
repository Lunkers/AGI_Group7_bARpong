using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateAfter(typeof(EndFramePhysicsSystem))] //update after simulation has ran for the frame
public class ResetThrowableSystem : SystemBase
{
    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepPhysicsWorld;
    EntityQuery triggerResetGroup;
    EndFixedStepSimulationEntityCommandBufferSystem bufferSystem;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        bufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
        triggerResetGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                typeof(CollisionReseter)
            }
        });
    }

    [BurstCompile]
    struct TriggerResetThrowableJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<CollisionReseter> collisionReseterGroup;
        public ComponentDataFromEntity<Throwable> throwableGroup;
        public ComponentDataFromEntity<PhysicsVelocity> physicsVelocityGroup;
        public EntityCommandBuffer commandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isBodyATrigger = collisionReseterGroup.HasComponent(entityA);
            bool isBodyBTrigger = collisionReseterGroup.HasComponent(entityB);

            //ignore triggers overlapping each other
            if (isBodyATrigger && isBodyBTrigger)
            {
                return;
            }

            bool isBodyADynamic = physicsVelocityGroup.HasComponent(entityA);
            bool isBodyBDynamic = physicsVelocityGroup.HasComponent(entityB);

            //Ignore overlapping static bodies
            if( (isBodyBTrigger && !isBodyADynamic) ||(isBodyATrigger && !isBodyBDynamic))
            {
                return;
            }

            var triggerEntity = isBodyATrigger ? entityA : entityB;
            var dynamicEntity = isBodyATrigger ? entityB : entityA;

            var triggerResetComponent = collisionReseterGroup[triggerEntity];

            if(triggerResetComponent.resetThrowables)
            {
                //reset thrown statueś here
                var throwable = throwableGroup[dynamicEntity];
                throwable.thrown = false;
                throwableGroup[dynamicEntity] = throwable;
                //remove velocity here
                var velocity = physicsVelocityGroup[dynamicEntity];
                velocity.Linear = new float3(0, 0, 0);
                physicsVelocityGroup[dynamicEntity] = velocity;
                ////remove velocity
                commandBuffer.RemoveComponent<PhysicsMass>(dynamicEntity);
                
            }
        }
    }

    protected override void OnUpdate()
    {
        if (triggerResetGroup.CalculateEntityCount() == 0)
        {
            return;
        }
        Dependency = new TriggerResetThrowableJob
        {
            collisionReseterGroup = GetComponentDataFromEntity<CollisionReseter>(true),
            throwableGroup = GetComponentDataFromEntity<Throwable>(),
            physicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>(),
            commandBuffer = bufferSystem.CreateCommandBuffer(),

        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);

        bufferSystem.AddJobHandleForProducer(Dependency);
    }
}

 
