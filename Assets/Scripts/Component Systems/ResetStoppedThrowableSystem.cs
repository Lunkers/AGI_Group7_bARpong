using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class ResetStoppedThrowableSystem : SystemBase
{
    EndFixedStepSimulationEntityCommandBufferSystem bufferSystem;
    static readonly float3 velocityLimit = new float3(0.1f, 0.1f, 0.1f);

    protected override void OnCreate()
    {
        bufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
       
    }
    protected override void OnUpdate()
    {
        EntityCommandBuffer commandBuffer = bufferSystem.CreateCommandBuffer();
        Entities.ForEach((ref Entity e, ref Throwable throwable, ref PhysicsVelocity velocity) => {
            if(throwable.thrown) {
                if((math.abs(velocity.Linear.x) <= velocityLimit.x) && (math.abs(velocity.Linear.y) <= velocityLimit.y) && (math.abs(velocity.Linear.z) <= velocityLimit.z)) {
                    //reset throwable
                    commandBuffer.AddComponent(e, new ResetTag());
                }
            }
        }).Run();   
    }
}
