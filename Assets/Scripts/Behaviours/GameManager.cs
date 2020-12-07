using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine.UI;
using Unity.Transforms;
using UnityEngine.SceneManagement;
using TMPro;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text scoreText;
    public Text timeText;
    public GameObject endPanel;
    public TMP_Text resultText;
    //public int amtCups;

    private int curScore;
    //private Entity ballEntityPrefab;
    private EntityManager manager;
    //private BlobAssetStore blobAssetStore;
    public bool tablePlaced = false;
    private float elapsedTime;

    private void Awake() {
        instance = this;
    }
    private void Start(){
        curScore = 6;
        elapsedTime = 0;
        DisplayScore();
    }
    private void DisplayScore(){
        if (scoreText)
            scoreText.text = $"Cups left: { curScore}";
    }

    private void Update() {
        elapsedTime += Time.deltaTime;
        timeText.text = elapsedTime.ToString("F2");
        //TODO: add code for unity fixed timestep here
        //should match server tick rate when we get photon going    
    }

    public void IncreaseScore() {
        curScore = curScore -1;
        DisplayScore();
        if(curScore == 0){
            // disable fixed rate before scene switch to avoid crashes
            //FixedRateUtils.DisableFixedRate(World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SimulationSystemGroup>());
            //DESTROY BALL
            EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
            em.DestroyEntity(em.UniversalQuery);
            resultText.text = elapsedTime.ToString("F2")+ " seconds!";
            endPanel.SetActive(true);

        }
        
        
    }


}
