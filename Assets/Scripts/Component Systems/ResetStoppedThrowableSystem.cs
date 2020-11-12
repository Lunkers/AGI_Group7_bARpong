using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class ResetStoppedThrowableSystem : SystemBase
{
    EndFixedStepSimulationEntityCommandBufferSystem bufferSystem;

    protected override void OnCreate()
    {
        bufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        EntityCommandBuffer commandBuffer = bufferSystem.CreateCommandBuffer();
        Entities.ForEach((ref Entity e, ref Throwable throwable, ref PhysicsVelocity velocity) => {
            if(throwable.thrown) {
                if(math.abs(velocity.Linear).Equals(float3.zero)) {
                    //reset throwable
                    commandBuffer.AddComponent(e, new ResetTag());
                }
            }
        }).Run();   
    }
}
