using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TouchInputBehaviour : MonoBehaviour
{
    public TablePlacementState tablePlacementState;
    private ThrowMotionSystem throwMotionSystem;
    private Touch touch;

    //public CheckTablePlaced placedChecker;
    // Start is called before the first frame update
    void Start()
    {
        throwMotionSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ThrowMotionSystem>();
    }

    // Update is called once per frame
    void Update()
    {


        touch = Input.GetTouch(0);
        if (GameManager.instance.tablePlaced)
        {
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                throwMotionSystem.Launch();
            }
        }

    }
}
