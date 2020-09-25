using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Throwable :  IComponentData {
    public float angle;
    public float initialVelocity;

    public float mass; //the mass in grams
    public bool thrown;
    public Entity camera;
}

