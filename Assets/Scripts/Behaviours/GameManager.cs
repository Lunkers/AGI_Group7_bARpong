using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine.UI;
using Unity.Transforms;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text scoreText;
    public AudioCollection audioCollection;
    //public int amtCups;

    private int curScore;
    //private Entity ballEntityPrefab;
    private EntityManager manager;
    //private BlobAssetStore blobAssetStore;
    public bool tablePlaced = false;

    private void Awake() {
        instance = this;
    }
    private void Start(){
        curScore = 6;
        DisplayScore();
    }
    private void DisplayScore(){
        if (scoreText)
            scoreText.text = $"Cups left: { curScore}";
    }

    private void Update() {
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
            SceneManager.LoadScene(0);
        }
        
        
    }


}
