using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Animatable :  IComponentData {
    public float3 initialPosition;
    public float3 targetPosition;
    public float initialTime;
    public float targetTime;
    public bool animating;
}

