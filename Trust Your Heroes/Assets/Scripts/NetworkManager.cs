using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private bool TriesToConnectToMaster;
    private bool TriesToConnectToRoom;

    public static bool firstPlayer = false;

    void Start()
    {
        TriesToConnectToMaster = false;
        TriesToConnectToRoom = false;
        DontDestroyOnLoad(gameObject);
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu";
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.GameVersion = "v1";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            TriesToConnectToMaster = true;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        TriesToConnectToMaster = false;
        TriesToConnectToRoom = false;
        Debug.Log(cause);
        Debug.Log("Disconnected!");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        TriesToConnectToMaster = false;
        Debug.Log("Connected to master");
    }

    public override void OnLeftRoom()
    {
        YourHeroTeam.heroNames[0] = "";
        YourHeroTeam.heroNames[1] = "";
        YourHeroTeam.heroNames[2] = "";
        YourHeroTeam.heroNames[3] = "";
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("Left room");
        Destroy(gameObject);
    }

    public void OnLeave()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public void OnClickConnectToRoom()
    {
        if (!PhotonNetwork.IsConnected || TriesToConnectToMaster)
            return;
        PhotonNetwork.NickName = "Player2";
        firstPlayer = false;
        TriesToConnectToRoom = true;
        PhotonNetwork.JoinRoom("Room" + MainMenu.mapNumber + "-" + (PhotonNetwork.CountOfRooms - 1));
        Debug.Log("Connecting to a room...");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom("Room" + MainMenu.mapNumber + "-" + PhotonNetwork.CountOfRooms, options);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        PhotonNetwork.NickName = "Player1";
        firstPlayer = true;
        Debug.Log("Created room successfully");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Failed to create room");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            switch (MainMenu.mapNumber) //Opens scene depending on the chosen map
            {
                case 1:
                    SceneManager.LoadScene("DarkTownScene");
                    break;
                case 2:
                    SceneManager.LoadScene("ChristmasScene");
                    break;
                case 3:
                    SceneManager.LoadScene("DesertScene");
                    break;
            }
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room " + PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.IsMasterClient)
            Debug.Log("Waiting for other player...");
        Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | Room name: " + PhotonNetwork.CurrentRoom.Name);
    }
}
