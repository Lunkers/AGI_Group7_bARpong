using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

public class ARCoach : MonoBehaviour
{
    private ARSession session;

    void Start()
    {
        session = FindObjectOfType<ARSession>();

        if (session.subsystem is ARKitSessionSubsystem)
        {
            var subsystem = (ARKitSessionSubsystem)session.subsystem;
            subsystem.requestedCoachingGoal = ARCoachingGoal.HorizontalPlane;
            subsystem.coachingActivatesAutomatically = true;
        }
    }

    void Update()
    {
    }
}
