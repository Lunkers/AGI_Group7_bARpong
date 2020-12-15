using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ResetButton : MonoBehaviour
{
    private ThrowMotionSystem throwMotionSystem;

    void Start()
    {
        throwMotionSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ThrowMotionSystem>();
    }

    public void HandleResetButtonPressed()
    {
        throwMotionSystem.Reset();
    }
}