using Unity.Entities;

[GenerateAuthoringComponent]
public struct Throwable :  IComponentData {
    public float angle;
    public float initialVelocity;
}
