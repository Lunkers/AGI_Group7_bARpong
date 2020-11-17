using Unity.Entities;
using UnityEngine;

// [UpdateInGroup(typeof(SimulationSystemGroup))]
// public class PhysicsFixedRateHack : SystemBase
// {
//   private bool _IsInitialized;

//   protected override void OnUpdate() {
//       if(!_IsInitialized){
//           Debug.Log(typeof(SimulationSystemGroup));
//               FixedRateUtils.EnableFixedRateWithCatchUp(World.GetOrCreateSystem<SimulationSystemGroup>(),
//                                     UnityEngine.Time.fixedDeltaTime);
//               _IsInitialized = true;
//       }
//       Debug.Log(UnityEngine.Time.time);
//   }
// }
