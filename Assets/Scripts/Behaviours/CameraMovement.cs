using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 10f;
    public Entity PingPongBall = Entity.Null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Pressed Space");
            ThrowMotionSystem throwSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ThrowMotionSystem>();
            throwSystem.Launch();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Pressed Space");
            ThrowMotionSystem throwSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ThrowMotionSystem>();
            throwSystem.Reset();
        }
    }


}
