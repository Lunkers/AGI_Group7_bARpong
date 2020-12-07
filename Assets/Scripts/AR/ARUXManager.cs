using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ARUXManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instructional text for the UI")]
    TMP_Text _InstructionText;
    public TMP_Text InstructionText
    {
        get => _InstructionText;
        set => _InstructionText = value;
    }


    // [SerializeField]
    // [Tooltip("Video clip for finding planes")]
    // VideoClip _FindPlaneClip;
    // public VideoClip FindPlaneClip
    // {
    //     get => _FindPlaneClip;
    //     set => _FindPlaneClip = value;
    // }


    // [SerializeField]
    // [Tooltip("Video explaining tap to place")]
    // VideoClip _TapToPlaceClip;
    // public VideoClip TapToPlaceClip
    // {
    //     get => _TapToPlaceClip;
    //     set => _TapToPlaceClip = value;
    // }

    [SerializeField]
    [Tooltip("ARKit coaching overlay")]
    ARCoach _ARKitCoach;
    public ARCoach ARKitCoach
    {
        get => _ARKitCoach;
        set => _ARKitCoach = value;
    }


    // [SerializeField]
    // [Tooltip("Raw image used for videoplayer reference")]
    // RawImage _RawImage;
    // public RawImage rawImage
    // {
    //     get => _RawImage;
    //     set => _RawImage = value;
    // }

    [SerializeField]
    [Tooltip("UI fade-in time")]
    float _FadeOnDuration = 1.0f;

    [SerializeField]
    [Tooltip("UI fade-out time")]
    float _FadeOffDuration = 0.5f;

    Color _AlphaWhite = new Color(1, 1, 1, 0);

    Color _White = new Color(1, 1, 1, 1);

    Color _TargetColor;
    Color _StartColor;
    Color _LerpColor;
    bool _FadeOn;
    bool _FadeOff;
    bool _Tweening;
    bool _UsingARKitCoaching;
    float _TweenTime;
    float _TweenDuration;

    const string _MoveDeviceText = "Move your device slowly";
    const string _TapToPlaceText = "Tap The screen to place the play area";

    public static event Action onFadeOffComplete;

    [SerializeField]
    Texture _Transparent;

    public Texture transparent
    {
        get => _Transparent;
        set => _Transparent = value;
    }

    RenderTexture _RenderTexture;

    // Start is called before the first frame update
    void Start()
    {
        _StartColor = _AlphaWhite;
        _TargetColor = _White;
    }

    // Update is called once per frame
    void Update()

    {
        // try to avoid video player crashing the app
        // if (!_VideoPlayer.isPrepared)
        // {
        //     return;
        // }

        // if (_FadeOff || _FadeOn)
        // {
        //     if (_FadeOn)
        //     {
        //         _StartColor = _AlphaWhite;
        //         _TargetColor = _White;
        //         _TweenDuration = _FadeOnDuration;
        //         _FadeOff = false;
        //     }

        //     if (_FadeOff)
        //     {
        //         _StartColor = _White;
        //         _TargetColor = _AlphaWhite;
        //         _TweenDuration = _FadeOffDuration;
        //         _FadeOn = false;
        //     }

        //     if (_TweenTime < 1)
        //     {
        //         _TweenTime += Time.deltaTime / _TweenDuration;
        //         _LerpColor = Color.Lerp(_StartColor, _TargetColor, _TweenTime);
        //         // _RawImage.color = _LerpColor;

        //         _Tweening = true;
        //     }

        //     else
        //     {
        //         _TweenTime = 0;
        //         _FadeOff = false;
        //         _FadeOn = false;
        //         _Tweening = false;

        //         //check if it was a fade off 
        //         if (_TargetColor == _AlphaWhite)
        //         {
        //             //Do things
        //             if (onFadeOffComplete != null)
        //             {
        //                 onFadeOffComplete();
        //             }

        //             // _RenderTexture = _VideoPlayer.targetTexture;
        //             _RenderTexture.DiscardContents();
        //             _RenderTexture.Release();
        //             Graphics.Blit(_Transparent, _RenderTexture);
        //         }
        //     }


    }

    public void ShowTapToPlace()
    {
        // _VideoPlayer.clip = _TapToPlaceClip;
        // _VideoPlayer.Play();
        _InstructionText.text = _TapToPlaceText;
        _FadeOn = true;
    }

    public void ShowCrossPlatformFindPlane()
    {
        // _VideoPlayer.clip = _FindPlaneClip;
        // _VideoPlayer.Play();
        _InstructionText.text = _MoveDeviceText;
        _FadeOn = true;
    }

    public void ShowCoachingOverlay()
    {
        if (_ARKitCoach)
        {
            if (_ARKitCoach.supported)
            {
                _ARKitCoach.ActivateCoaching(true);

                _UsingARKitCoaching = true;
            }
            else
            {
                Debug.LogWarning("Coaching overlay not supported on this platform");
            }
        }
    }

    public bool ARKitCoachingSupported()
    {
        if (_ARKitCoach)
        {
            return _ARKitCoach.supported;
        }

        return false;
    }

    public void FadeOffCurrentUI()
    {
        if (_UsingARKitCoaching)
        {
            //assumes coaching overlay is first in order
            //Disable instantly rather than animating off
            _ARKitCoach.DisableCoaching(false);
            _UsingARKitCoaching = false;
            _InstructionText.color = _AlphaWhite;

            if (onFadeOffComplete != null)
            {
                onFadeOffComplete();
            }
            _FadeOff = true;
        }

        // if (_VideoPlayer.clip != null)
        // {
        //     if (_Tweening || _FadeOn)
        //     {
        //         _TweenTime = 1.0f;
        //         _RawImage.color = _AlphaWhite;
        //         _InstructionText.color = _AlphaWhite;
        //         if (onFadeOffComplete != null)
        //         {
        //             onFadeOffComplete();
        //         }
        //     }

        //     _FadeOff = true;
        // }
    }
}


