using UnityEngine;
// <summary>
// Simple class to keep track of multiple audio files,
// In order to be able to hotswitch them in our systems as needed.
// Example usage:
//  audioSource.clip = AudioCollection.ScorePointAudio;
//  audioSource.play();
// <summary/>
public class AudioCollection: MonoBehaviour {
    public AudioClip BallOnWoodAudio;
    public AudioClip BallOnCupAudio;

    public AudioClip ScorePointAudio;
}