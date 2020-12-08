using Unity.Entities;
using Unity.Physics;
using Unity.Collections;
using Unity.Physics.Systems;
using Unity.Jobs;


[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(TargetScoringSystem))]
public class BouncingAudioSystem : SystemBase
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
         [ReadOnly] public ComponentDataFromEntity<TableTag> tableObject;
         [ReadOnly] public ComponentDataFromEntity<CupTag> cupObject;
         [ReadOnly] public ComponentDataFromEntity<Throwable> ballObject;
        public EntityCommandBuffer commandBuffer;

         public void Execute(CollisionEvent collisionEvent)
         {
             Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;
             bool entityAIsTable = tableObject.HasComponent(entityA);
             bool entityBIsTable = tableObject.HasComponent(entityB);
             bool entityAIsCup = cupObject.HasComponent(entityA);
             bool entityBIsCup = cupObject.HasComponent(entityB);
             bool entityAIsBall = ballObject.HasComponent(entityA);
             bool entityBIsBall = ballObject.HasComponent(entityB);

             if (entityAIsCup && entityBIsBall)
             {
                commandBuffer.AddComponent(entityB, new CupBounceTag());
             }
             if (entityAIsBall && entityBIsCup)
             {
                commandBuffer.AddComponent(entityA, new CupBounceTag());
             }
             if (entityAIsTable && entityBIsBall)
             {
                commandBuffer.AddComponent(entityB, new TableBounceTag());
            }
             if (entityAIsBall && entityBIsTable)
             {
                commandBuffer.AddComponent(entityA, new TableBounceTag());
             }
         }
     }
    protected override void OnUpdate()
    {
        
       Dependency = new CollisionAudioSystemJob
        {
            tableObject = GetComponentDataFromEntity<TableTag>(true),
            cupObject = GetComponentDataFromEntity<CupTag>(true),
            ballObject = GetComponentDataFromEntity<Throwable>(true),
            commandBuffer = bufferSystem.CreateCommandBuffer(),
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);
        bufferSystem.AddJobHandleForProducer(Dependency);
        Dependency.Complete();
    }
}
