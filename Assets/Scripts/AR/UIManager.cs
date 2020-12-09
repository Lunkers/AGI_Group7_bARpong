using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public struct UXHandle
{
    public UIManager.InstructionUI InstructionalUI;

    public UIManager.InstructionGoals Goal;

    public UXHandle(UIManager.InstructionUI ui, UIManager.InstructionGoals goal)
    {
        InstructionalUI = ui;
        Goal = goal;
    }
}

public class UIManager : MonoBehaviour
{

    [SerializeField]
    bool _StartWithInstructionalUI = true;
    public bool startWithInstructionalUI
    {
        get => _StartWithInstructionalUI;
        set => _StartWithInstructionalUI = value;
    }

    public enum InstructionUI
    {
        CrossPlatformFindPlane,
        ARKitCoachingOverlay,
        TapToPlace,
        None
    };

    [SerializeField]
    InstructionUI _InstructionalUI;
    public InstructionUI instructionalUI
    {
        get => _InstructionalUI;
        set => _InstructionalUI = value;
    }

    [SerializeField]
    InstructionUI _SecondaryInstructionalUI = InstructionUI.TapToPlace;

    public InstructionUI secondaryInstructionUI
    {
        get => _SecondaryInstructionalUI;
        set => _SecondaryInstructionalUI = value;
    }

    public enum InstructionGoals
    {
        FoundAPlane,
        FoundMultiplePlanes,
        PlacedAnObject,
        None
    }

    [SerializeField]
    InstructionGoals _InstructionalGoal;
    public InstructionGoals instructionalGoal
    {
        get => _InstructionalGoal;
        set => _InstructionalGoal = value;
    }
    [SerializeField]
    InstructionGoals _SecondaryGoal = InstructionGoals.PlacedAnObject;
    public InstructionGoals secondayGoal
    {
        get => _SecondaryGoal;
        set => _SecondaryGoal = value;
    }

    [SerializeField]
    [Tooltip("Fallback to cross-platform UI if ARKit coaching overlay is not supported")]
    bool _CoachingOverlayFallback;
    public bool coachingOverlayFallback
    {
        get => _CoachingOverlayFallback;
        set => _CoachingOverlayFallback = value;
    }

    [SerializeField]
    GameObject _ARSessionOrigin;
    public GameObject arSessionOrigin
    {
        get => _ARSessionOrigin;
        set => _ARSessionOrigin = value;
    }

    [SerializeField]
    bool _ShowSecondaryInstructionalUI;

    public bool showSecondaryInstructionUI
    {
        get => _ShowSecondaryInstructionalUI;
        set => _ShowSecondaryInstructionalUI = value;
    }

    [SerializeField]
    ARUXManager _AnimationManager;
    public ARUXManager animationManager
    {
        get => _AnimationManager;
        set => _AnimationManager = value;
    }
    bool _FadedOff = false;

    Func<bool> _GoalReached;
    bool _SecondaryGoalReached;

    Queue<UXHandle> _UXOrderedQueue;
    UXHandle _CurrentHandle;
    bool _ProcessingInstructions;
    bool _PlacedObject;

    [SerializeField]
    ARPlaneManager _PlaneManager;

    public ARPlaneManager planeManager
    {
        get => _PlaneManager;
        set => _PlaneManager = value;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        ARUXManager.onFadeOffComplete += FadeComplete;
        TablePlacer.onTablePlaced += () =>
        {
            Debug.Log("Table placed, goal should be met");
            _PlacedObject = true;
        };

        GetManagers();
        _UXOrderedQueue = new Queue<UXHandle>();

        if (_StartWithInstructionalUI)
        {
            _UXOrderedQueue.Enqueue(new UXHandle(_InstructionalUI, _InstructionalGoal));
        }
        if (_ShowSecondaryInstructionalUI)
        {
            _UXOrderedQueue.Enqueue(new UXHandle(_SecondaryInstructionalUI, _SecondaryGoal));
        }
    }

    void OnDisable()
    {
        ARUXManager.onFadeOffComplete -= FadeComplete;
    }

    // Update is called once per frame
    void Update()
    {
        if (_UXOrderedQueue.Count > 0 && !_ProcessingInstructions)
        {
            //pop off 
            _CurrentHandle = _UXOrderedQueue.Dequeue();
            Debug.Log($"Current Goal: {_CurrentHandle.Goal}");
            //fade on 
            FadeOnInstructionalUI(_CurrentHandle.InstructionalUI);
            _GoalReached = GetGoal(_CurrentHandle.Goal);
            _ProcessingInstructions = true;
            _FadedOff = false;
        }

        if (_ProcessingInstructions)
        {
            //Start listening for goal reached
            if (_GoalReached.Invoke())
            {
                // if goal reached, fade off
                if (!_FadedOff)
                {
                    _FadedOff = true;
                    _AnimationManager.FadeOffCurrentUI();
                }
            }
        }

    }

    void GetManagers()
    {
        if (_ARSessionOrigin)
        {
            if (_ARSessionOrigin.TryGetComponent(out ARPlaneManager aRPlaneManager))
            {
                _PlaneManager = aRPlaneManager;
            }
        }
    }

    Func<bool> GetGoal(InstructionGoals goal)
    {
        switch (goal)
        {
            case InstructionGoals.FoundAPlane:
                return PlanesFound;

            case InstructionGoals.FoundMultiplePlanes:
                return MultiplePlanesFound;

            case InstructionGoals.PlacedAnObject:
                return PlacedObject;

            case InstructionGoals.None:
                return () => false;
        }

        return () => false;
    }

    void FadeOnInstructionalUI(InstructionUI ui)
    {
        Debug.Log("HERE IS THE UI OBJECT");
        Debug.Log(ui);
        Debug.Log("HERE IS ANIMATION MANAGER");
        Debug.Log(_AnimationManager);
        switch (ui)
        {
            case InstructionUI.CrossPlatformFindPlane:
                _AnimationManager.ShowCrossPlatformFindPlane();
                break;

            case InstructionUI.ARKitCoachingOverlay:
                if (_AnimationManager.ARKitCoachingSupported())
                {
                    _AnimationManager.ShowCoachingOverlay();
                }
                else
                {
                    // fall back to cross-platform overlay
                    if (_CoachingOverlayFallback)
                    {
                        _AnimationManager.ShowCrossPlatformFindPlane();
                    }
                }
                break;

            case InstructionUI.TapToPlace:
                Debug.Log("Should show tap to place text");
                _AnimationManager.ShowTapToPlace();
                break;


            case InstructionUI.None:

                break;

        }
    }

    bool PlanesFound() => _PlaneManager && _PlaneManager.trackables.count > 0;
    bool MultiplePlanesFound() => _PlaneManager && _PlaneManager.trackables.count > 1;

    void FadeComplete()
    {
        _ProcessingInstructions = false;
    }
    bool PlacedObject()
    {
        return _PlacedObject;
    }

    public void AddToQueue(UXHandle uXHandle)
    {
        _UXOrderedQueue.Enqueue(uXHandle);
    }

    public void TestFlipPlacementBool()
    {
        _PlacedObject = true;
    }
}
