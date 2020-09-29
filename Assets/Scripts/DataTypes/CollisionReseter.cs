using Unity.Entities;

[GenerateAuthoringComponent]
public struct CollisionReseter : IComponentData
{
    public bool resetThrowables;
}
