using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
#if UNITY_IOS
using UnityEngine.XR.ARKit;
#endif


[RequireComponent(typeof(ARSession))]
public class ARCoach : MonoBehaviour
{

    enum CoachingGoals
    {
        Tracking,
        HorizontalPlane,
        VerticalPlane,
        AnyPlane
    }
    [SerializeField]
    [Tooltip("Coaching goal associated with the coaching overlay")]
#if !UNITY_IOS
#pragma warning disable CS0414
#endif
    CoachingGoals _Goal = CoachingGoals.Tracking;

#if !UNITY_IOS
#pragma warning restore CS0414
#endif


#if UNITY_IOS
    /// <summary>
    /// The [ARCoachingGoal](https://developer.apple.com/documentation/arkit/arcoachinggoal) associated with the overlay
    /// </summary>

    public ARCoachingGoal goal
    {
        get
        {
            if (GetComponent<ARSession>().subsystem is ARKitSessionSubsystem sessionSubsystem)
            {
                return sessionSubsystem.requestedCoachingGoal;
            }
            else
            {
                return (ARCoachingGoal)_Goal;
            }
        }
        set
        {
            _Goal = (CoachingGoals)value;
            if (supported && GetComponent<ARSession>().subsystem is ARKitSessionSubsystem sessionSubsystem)
            {
                sessionSubsystem.requestedCoachingGoal = value;
            }
        }
    }
#endif

    [SerializeField]
    [Tooltip("Whether the coaching overlay should activate automatically")]
    bool _AutoActivate = true;

    public bool autoActivate
    {
        get
        {
#if UNITY_IOS
            if (supported && GetComponent<ARSession>().subsystem is ARKitSessionSubsystem sessionSubsystem)
            {
                return sessionSubsystem.coachingActivatesAutomatically;
            }
#endif
            return _AutoActivate;
        }
        set
        {
            _AutoActivate = value;

#if UNITY_IOS
            if (supported && GetComponent<ARSession>().subsystem is ARKitSessionSubsystem sessionSubsystem)
            {
                sessionSubsystem.coachingActivatesAutomatically = value;
            }
#endif
        }
    }

    /// <summary>
    /// Whether the [ARCoachingGoal](https://developer.apple.com/documentation/arkit/arcoachinggoal) is supported.
    /// </summary>

    public bool supported
    {
        get
        {
#if UNITY_IOS
            return ARKitSessionSubsystem.coachingOverlaySupported;
#else
            return false;
#endif
        }
    }

    void OnEnable() {
        #if UNITY_IOS
        if(supported && GetComponent<ARSession>().subsystem is ARKitSessionSubsystem sessionSubsystem){
            sessionSubsystem.requestedCoachingGoal = (ARCoachingGoal)_Goal;
            sessionSubsystem.coachingActivatesAutomatically = _AutoActivate;
        }
        else
        #endif
        {
            Debug.LogError("ARCoachingOverlayView is not supported by this device");
        }
    }

    public void ActivateCoaching(bool animated){
        #if UNITY_IOS
            if(supported && GetComponent<ARSession>().subsystem is ARKitSessionSubsystem sessionSubsystem){
                sessionSubsystem.SetCoachingActive(true, animated ? ARCoachingOverlayTransition.Animated : ARCoachingOverlayTransition.Instant);
            }
            else
        #endif
            {
                Debug.LogWarning("ARCoaching overlay is not supported");
            }
    }

    public void DisableCoaching(bool animated)
    {
        #if UNITY_IOS
        if(supported && GetComponent<ARSession>().subsystem is ARKitSessionSubsystem sessionSubsystem){
            sessionSubsystem.SetCoachingActive(false, animated ? ARCoachingOverlayTransition.Animated : ARCoachingOverlayTransition.Instant);
        }
        #endif
        {
            Debug.LogWarning("ARCoaching overlay is not supported");
        }
    }
}