using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
#if UNITY_IOS
        using UnityEngine.XR.ARKit;
#endif






public class ARCoach : MonoBehaviour
{
    private ARSession session;
    void Start()
    {
    #if UNITY_IOS
        if (session.subsystem is ARKitSessionSubsystem)
        {
            var subsystem = (ARKitSessionSubsystem)session.subsystem;
            subsystem.requestedCoachingGoal = ARCoachingGoal.HorizontalPlane;
            subsystem.coachingActivatesAutomatically = true;
        }

    #endif
    }
}