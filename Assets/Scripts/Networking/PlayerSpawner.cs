using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    private PhotonView photonView;
    //[SerializeField] public GameObject sessionOrigin;
    private void Start() //Fortsätt här 
    {

        var player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 14, -24), Quaternion.Euler(16, 0, 0)); //ser fel, kontrollerar rätt
        
        photonView = player.GetComponent<PhotonView>();
        if(photonView.IsMine)
        {
            Camera camera = player.GetComponent<Camera>();
            camera.enabled = true;
        }
        else
        {
            Camera camera = player.GetComponent<Camera>();
            camera.enabled = false;
        }
        /* if (!playerPrefab.GetPhotonView().IsMine)  
         {
             PhotonNetwork.Destroy(playerPrefab);
         }
         else
         {
             PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 14, -24), Quaternion.Euler(16, 0, 0));
             playerPrefab.GetComponent<Camera>().enabled = true;
         }*/


        // instance.transform.SetParent(sessionOrigin.transform);
    }
    private void PlayerLocationSpawn()
    {
        
    }
}
