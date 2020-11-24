using Photon.Pun;
using Photon.Realtime;
using System;
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
    public TMP_Text error;

    private const int MaxPlayersPerRoom = 2;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true; // senast ingen scen för någon av användarna, men denna skall användas någonstans
    }


    public override void OnConnectedToMaster() => Debug.Log("Connected to Master");
    public void JoinRoom()
    {
        string RoomName = joinRoomInputfield.text;

       

        PhotonNetwork.JoinRoom(RoomName);
        //Check if room do not exist - Display it! 
      
        Debug.Log("Attempting to connect to room:  " + RoomName);
    }

    public void CreateRoom()
    {
        bool isLoading = false;
        string RoomName = hostRoomInputfield.text;
        PhotonNetwork.NickName = RoomName;
        
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = MaxPlayersPerRoom;
        PhotonNetwork.CreateRoom(RoomName, options, TypedLobby.Default);
        Debug.Log("Inside createRoom function and created room: " + RoomName);
        
    }

    public void LeaveOnReturn()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnecting from server.");
        PhotonNetwork.ConnectUsingSettings();
        //Destroy playerObject needed? 
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
            
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            Debug.Log("Match is ready to begin.");
            
            if (PhotonNetwork.IsMasterClient) 
            {
                try {
                    PhotonNetwork.LoadLevel("ARTest_Multiplayer");
                } 
                catch (Exception e)  {
                    error.text = e.ToString();
                }
               
                
            }
            
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected due to: " + cause);
    }

    // Update is called once per frame

}
