using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    private Touch initialTouch;
    public GameObject ball;

    void Start()
    {
        var position = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
        position.z = 0.2f;
        Instantiate(ball, position, Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up));
    }

    void Update()
    {
        var touch = Input.GetTouch(0);
        var phase = touch.phase;

        if (phase == TouchPhase.Began)
        {
            initialTouch = touch;
        }
        else if (phase == TouchPhase.Moved || phase == TouchPhase.Stationary)
        {
            ball.transform.position = new Vector2(0.0f, touch.deltaPosition.y);
        }
        else if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
        {
        }
    }
}
