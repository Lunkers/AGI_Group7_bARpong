using Unity.Entities;

[GenerateAuthoringComponent]

public struct ScoringCollider:IComponentData {
    public int score;
    public bool deleteOnScore;
    public Entity cup;
}