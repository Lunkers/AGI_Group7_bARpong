using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine.UI;
using Unity.Transforms;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text scoreText;

    private int curScore;
    private Entity ballEntityPrefab;
    private EntityManager manager;
    private BlobAssetStore blobAssetStore;


    private void Start(){
        curScore = 0;
        DisplayScore();
    }
    private void DisplayScore(){
        scoreText.text = "Score: " + curScore;
    }

    public void IncreaseScore() {
        curScore++;
        DisplayScore();
        
    }


}
