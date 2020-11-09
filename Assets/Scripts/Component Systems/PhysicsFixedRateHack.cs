using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class PhysicsFixedRateHack : SystemBase
{
  private bool _IsInitialized;

  protected override void OnUpdate() {
      if(!_IsInitialized){
          FixedRateUtils.EnableFixedRateWithCatchUp(
              World.GetOrCreateSystem<SimulationSystemGroup>(),
              UnityEngine.Time.fixedDeltaTime);
              _IsInitialized = true;
      }
  }
}
