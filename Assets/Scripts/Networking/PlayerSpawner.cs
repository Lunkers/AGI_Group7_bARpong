using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    //[SerializeField] public GameObject sessionOrigin;
    private void Start()
    {
     //  PhotonNetwork.Instantiate(playerPrefab.name,new Vector3(0,0,0),Quaternion.identity);
       // instance.transform.SetParent(sessionOrigin.transform);
    }
    private void PlayerLocationSpawn()
    {
        
    }
}
