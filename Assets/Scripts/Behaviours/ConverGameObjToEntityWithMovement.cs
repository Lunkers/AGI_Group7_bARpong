using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;


public struct GOData: IComponentData{}
public class ConverGameObjToEntityWithMovement : MonoBehaviour, IConvertGameObjectToEntity
{
    //public AudioSource audioSource;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem){
        if(TryGetComponent<AudioSource>(out var audioSource)){
            // Debug.Log("found audio source!");
            conversionSystem.AddHybridComponent(audioSource);
        }
    }
}
