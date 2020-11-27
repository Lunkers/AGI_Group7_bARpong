using Unity.Entities;

[GenerateAuthoringComponent]
public struct AudioPlayTag : IComponentData
{
    public bool playAudio;
}
