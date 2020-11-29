using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MultiPlayerMovementController : MonoBehaviourPun
{
    public float speed = 10f;
    ThrowMotionSystem throwMotionSystem;
    private Transform avatar;

    // Start is called before the first frame update
    void Start()
    {
        throwMotionSystem =World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ThrowMotionSystem>();
        Debug.Log(throwMotionSystem != null);
        avatar = gameObject.transform.GetChild(0);


    }

    // Update is called once per frame
    void Update()
       
    {
        if (gameObject.GetPhotonView().IsMine)
            TakeInput();
    }


    private void TakeInput()
    {
        avatar.forward = transform.forward;
        avatar.right = transform.right;
        avatar.localPosition = new Vector3(0, 0, 0);
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
            throwMotionSystem.Launch();
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

    }
}
