using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class SwipeThrower : MonoBehaviour
{
    private ThrowMotionSystem throwMotionSystem;
    private Animate animate;

    public GameObject objectToThrow;
    public float initialViewportY = 0.1f;
    private float speed = 0.0f;
    private float y0 = 0.0f;
    private float v0 = 0.0f;
    private float t0 = 0.0f;
    private float grabbedAt = 0.0f;
    private bool grabbed = false;

    void Start()
    {
        animate = objectToThrow.GetComponent<Animate>();
        throwMotionSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ThrowMotionSystem>();
        ResetObject();
    }

    void Update()
    {
        if (Input.touchCount > 0 && !animate.animating)
        {
            var touch = Input.GetTouch(0);
            
            var newObjectPosition = GetObjectPositionFromTouch(touch);

            var y = newObjectPosition.y;
            var dt = Time.time - t0;
            var dy = y-y0;
            var v = dy / dt;
            var acceleration = (v - v0) / dt;
            
            if (touch.phase == TouchPhase.Began && DidTap(touch))
            {
                grabbed = true;
                grabbedAt = Time.time;
            }
            else if (grabbed && touch.phase == TouchPhase.Moved)
            {
                objectToThrow.transform.position = newObjectPosition;
            }
            else if (grabbed && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                grabbed = false;
                if (Time.time - grabbedAt > 0.05f && v > 1.0f)
                {
                    Debug.Log(v + " m/s");
                    throwMotionSystem.Launch(v);
                    // ResetObjectWithAnimation();
                }
                else {
                    ResetObjectWithAnimation();
                }
            }

            v0 = v;
            y0 = objectToThrow.transform.position.y;
            t0 = Time.time;
        }

        objectToThrow.transform.position += new Vector3(0.0f, speed, 0.0f);
    }

    private bool DidTap(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0.0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == objectToThrow;
        }
        else
        {
            return false;
        }
    }

    private Vector3 GetObjectPositionFromTouch(Touch touch)
    {
        var cameraDistance = Vector3.Distance(Camera.main.transform.position, objectToThrow.transform.position);
        var newObjectPosition = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, touch.position.y, cameraDistance));
        newObjectPosition.x = 0.0f;
        newObjectPosition.z = objectToThrow.transform.position.z;
        return newObjectPosition;
    }

    private Vector3 GetObjectMinPosition()
    {
        var cameraDistance = Vector3.Distance(Camera.main.transform.position, objectToThrow.transform.position);
        var minPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, initialViewportY, cameraDistance));
        minPosition.z = objectToThrow.transform.position.z;
        return minPosition;
    }

    private void ResetObject()
    {
        objectToThrow.transform.position = GetObjectMinPosition();
    }

    private void ResetObjectWithAnimation()
    {
        var to = GetObjectMinPosition();
        var from = objectToThrow.transform.position;
        var velocity = 0.3f;
        var duration = (to - from).magnitude / velocity;
        animate.Position(GetObjectMinPosition(), duration);
    }
}
