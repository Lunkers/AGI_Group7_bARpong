using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class ThrowIndicator : MonoBehaviour
{
    enum State
    {
        Idle,
        Swiping,
        Thrown
    }

    private State _state;
    private State state {
        get {
            return _state;
        }

        set {
            if (_state != value) {
                stateChangeTime = Time.time;
            }

            _state = value;
        }
    }
    private float stateChangeTime = 0.0f;

    public Image image;
    public Text speedText;

    public Color maxPowerColor = Color.red;

    private Color idleColor = Color.clear;
    private Color thrownColor = Color.clear;

    public float timeToShowIndicator = 2.0f;
    public AnimationCurve fadeAnimation;

    private bool didThrow()
    {
        return false;
        // EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        // foreach (var entity in entityManager.GetAllEntities())
        // {
        //     if (entityManager.HasComponent<Throwable>(entity))
        //     {
        //         var throwable = entityManager.GetComponentData<Throwable>(entity);
        //         return throwable.thrown;
        //     }
        // }
        // return false;
    }

    void Start()
    {
        state = State.Idle;
        image.color = idleColor;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            state = State.Swiping;
            var normalizedDeltaY = touch.deltaPosition.y / (float) Screen.height;
            var v = normalizedDeltaY / touch.deltaTime;
            const float maxV = 8.0f;

            if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Ended) && v > 0.0f && !didThrow())
            {
                state = State.Thrown;
                thrownColor = Color.Lerp(Color.white, maxPowerColor, v/maxV);
                image.color = thrownColor;
                speedText.text = Mathf.Round(v*10.0f)/10.0f + " m/s";
                speedText.color = Color.white;
            }
            else
            {
                image.color = idleColor;
            }
        }
        else
        {
            if (state == State.Thrown)
            {
                if (Time.time - stateChangeTime >= timeToShowIndicator)
                {
                    state = State.Idle;
                    image.color = idleColor;
                    speedText.text = "";
                }
                else
                {
                    float t = fadeAnimation.Evaluate((Time.time-stateChangeTime)/timeToShowIndicator);
                    image.color = Color.Lerp(thrownColor, idleColor, t);;
                    speedText.color = Color.Lerp(Color.white, idleColor, t);;
                }
            }
        } 
    }
}