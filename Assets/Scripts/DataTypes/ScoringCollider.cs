using Unity.Entities;

[GenerateAuthoringComponent]
public struct ScoringCollider:IComponentData {
    public int score;
}