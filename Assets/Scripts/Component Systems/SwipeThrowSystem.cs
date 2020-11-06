using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using System;

public class SwipeThrowSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<Throwable>().ForEach((ref LocalToWorld localToWorld, ref Translation translation, ref Rotation rotation, ref Throwable throwable) =>
        {
            if (Input.touchCount > 0 && !throwable.thrown)
            {
                var touch = Input.GetTouch(0);
                var velocityScreen = touch.deltaPosition / touch.deltaTime;
                var velocityWorld = Camera.main.ScreenToWorldPoint(new Vector3(velocityScreen.x, velocityScreen.y, localToWorld.Position.z));

                if (touch.phase == TouchPhase.Began && TouchOnObject(touch))
                {
                    throwable.grabbed = true;
                }
                else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && throwable.grabbed)
                {
                    var throwVelocity = Mathf.Max(0.0f, velocityWorld.y) * 3.0f;
                    Debug.Log(throwVelocity + " m/s");
                    var throwMotionSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ThrowMotionSystem>();
                    throwable.grabbed = false;
                    throwMotionSystem.Launch(throwVelocity, (float)Math.PI / 4.0f);
                }
            }
        });
    }

    private bool MouseOnObject()
    {
        return true;
    }

    private bool TouchOnObject(Touch touch)
    {
        return true;
    }

    private float3 GetObjectPositionFromMouse(float3 objectPosition)
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = objectPosition.z;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return new float3(mousePosition.x, mousePosition.y, mousePosition.z);
    }

    private float3 GetObjectPositionFromTouch(float3 objectPosition, Touch touch)
    {
        var touchPosition = new Vector3(touch.position.x, touch.position.y, objectPosition.z);
        touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        return new float3(touchPosition.x, touchPosition.y, touchPosition.z);
    }

    private float3 GetObjectOrigin(float3 position)
    {
        var initialViewportY = 0.1f;
        var objectPosition = new Vector3(position.x, position.y, position.z);
        var cameraDistance = Vector3.Distance(Camera.main.transform.position, objectPosition);
        var minPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, initialViewportY, cameraDistance));
        minPosition.z = objectPosition.z;
        return new float3(minPosition.x, minPosition.y, minPosition.z);
    }
}
