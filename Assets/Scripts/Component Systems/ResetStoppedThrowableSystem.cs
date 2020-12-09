using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class ResetStoppedThrowableSystem : SystemBase
{
    EndFixedStepSimulationEntityCommandBufferSystem bufferSystem;
    static readonly float3 velocityLimit = new float3(0.001f, 0.001f, 0.001f);

    protected override void OnCreate()
    {
        bufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
       
    }
    protected override void OnUpdate()
    {
        EntityCommandBuffer commandBuffer = bufferSystem.CreateCommandBuffer();
        Entities.ForEach((ref Entity e, ref Throwable throwable, ref PhysicsVelocity velocity) => {
            if(throwable.thrown) {
                var velocityLength = math.sqrt(math.pow(velocity.Linear.x, 2.0f) + math.pow(velocity.Linear.y, 2.0f) + math.pow(velocity.Linear.z, 2.0f));
                var velocityLimitLength = math.sqrt(math.pow(velocityLimit.x, 2.0f) + math.pow(velocityLimit.y, 2.0f) + math.pow(velocityLimit.z, 2.0f));
                if(velocityLength < velocityLimitLength) {
                    //reset throwable
                    commandBuffer.AddComponent(e, new ResetTag());
                }
            }
        }).Run();   
    }
}
