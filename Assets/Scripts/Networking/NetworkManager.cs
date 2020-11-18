using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField joinRoomInputfield = null;
    [SerializeField] private Button joinButton = null;

    [SerializeField] private TMP_InputField hostRoomInputfield = null;
    [SerializeField] private Button hostButton = null;

    private const int MaxPlayersPerRoom = 2;

    private void Awake()
    {
        //PhotonNetwork.AutomaticallySyncScene = true; // senast ingen scen för någon av användarna, men denna skall användas någonstans
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster() => Debug.Log("Connected to Master");
    public void JoinRoom()
    {
        string RoomName = joinRoomInputfield.text;

       

        PhotonNetwork.JoinRoom(RoomName);
        
      
        Debug.Log("Attempting to connect to room:  " + RoomName);
    }

    public void CreateRoom()
    {
        string RoomName = hostRoomInputfield.text;
        PhotonNetwork.NickName = RoomName;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RoomName, options, TypedLobby.Default);
        Debug.Log("Inside createRoom function and created room: " + RoomName);
        
    }

    //When opponent has joined
    public override void OnJoinedRoom() //fortsätt här - pröva med OnJoinLobby() 
    {
        Debug.Log("Client successfully joined a room.");

        int numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

        if (numberOfPlayers != MaxPlayersPerRoom)
        {

            Debug.Log("Client is waiting for opponent.");

        }
        else
        {

            Debug.Log("Matching is ready to begin!");
            PhotonNetwork.LoadLevel("SampleScene");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            Debug.Log("Match is ready to begin.");


            //PhotonNetwork.LoadLevel("Beer_game");
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
      

        Debug.Log("Disconnected due to: " + cause);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
