using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float initialTime;
    private float targetTime;
    public bool animating { get; private set; } = false;

    public void Position(Vector3 position, float duration)
    {
        initialPosition = transform.position;
        initialTime = Time.time;
        targetPosition = position;
        targetTime = duration;
        animating = true;
    }

    void Update()
    {
        if (animating)
        {
            var t = (Time.time-initialTime)/targetTime;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, Mathf.SmoothStep(0.0f, 1.0f, t));

            if (t >= 1.0f)
            {
                transform.position = targetPosition;
                animating = false;
            }
        }
    }
}
