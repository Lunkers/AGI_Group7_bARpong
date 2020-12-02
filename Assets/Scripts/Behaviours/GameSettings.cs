using UnityEngine;
using UnityEngine.Assertions;

public sealed class GameSettings
{
    public static AudioCollection audioCollection;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeWithScene()
    {
        var audioCollectionGO = GameObject.Find("AudioCollection");
        if(audioCollectionGO != null){
            audioCollection = audioCollectionGO.GetComponent<AudioCollection>();
        }
        Assert.IsNotNull(audioCollection);
    }
}