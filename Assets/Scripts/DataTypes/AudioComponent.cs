using System.ComponentModel;
using Unity.Entities;


[GenerateAuthoringComponent]
public struct AudioComponent : IComponentData
{
    public bool isTable;
    public bool isCup;
    public Entity AudioSource;
}
   