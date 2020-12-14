using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 10f;
    public GameObject Ball;
    ThrowMotionSystem throwMotionSystem;

    Vector2 cameraRotation = new Vector2 (0, 0);
	public float cameraFreeLookSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        throwMotionSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ThrowMotionSystem>();
        Debug.Log(throwMotionSystem != null);
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
            throwMotionSystem.Launch(4.0f);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.E))
        {
            Instantiate(Ball);
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
            throwMotionSystem.Reset();
        }

        if (Input.GetKey(KeyCode.Z))
        {
            transform.Rotate(speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.X))
        {
            transform.Rotate(-speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetButton("Fire2"))
        {
            cameraRotation.y += Input.GetAxis("Mouse X");
            cameraRotation.x += -Input.GetAxis("Mouse Y");
            transform.eulerAngles = (Vector2)cameraRotation * cameraFreeLookSpeed;
        }
    }


}
