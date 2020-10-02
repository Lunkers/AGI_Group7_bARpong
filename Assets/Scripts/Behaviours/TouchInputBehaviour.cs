using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TouchInputBehaviour : MonoBehaviour
{
    private ThrowMotionSystem throwMotionSystem;
    private Touch touch;

    public GameObject PlacementIndicator;
    // Start is called before the first frame update
    void Start()
    {
        throwMotionSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ThrowMotionSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlacementIndicator.activeSelf)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                throwMotionSystem.Launch();
            }
        }
    }
}
