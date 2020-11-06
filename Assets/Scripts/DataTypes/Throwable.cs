using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Throwable :  IComponentData {
    public float mass; //the mass in grams
    public bool thrown;
    
    // If the entity has been grabbed by the user and the user is currently throwing.
    public bool grabbed;
}

