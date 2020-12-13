using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SwipeThrower : MonoBehaviour
{
    private ThrowMotionSystem throwMotionSystem;

    void Start()
    {
        throwMotionSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ThrowMotionSystem>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
            {
                var normalizedDeltaPosition = new Vector2(touch.deltaPosition.x / (float) Screen.width, touch.deltaPosition.y / (float) Screen.height);
                var v = normalizedDeltaPosition / touch.deltaTime;
                if (v.y > 0.0f)
                {
                    throwMotionSystem.Launch(v.y);
                }
            }
        }
    }
}
