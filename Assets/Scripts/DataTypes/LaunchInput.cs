using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct LaunchInput : IComponentData
{
    public KeyCode launchKey;
    public KeyCode angleIncrease;
    public KeyCode angleDecrease;
    public KeyCode forceIncrease;
    public KeyCode forceDecrease;
}
