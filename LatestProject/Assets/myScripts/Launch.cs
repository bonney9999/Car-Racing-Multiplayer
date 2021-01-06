using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launch : MonoBehaviourPunCallbacks
{
    public static Launch GetClass;

    public GameObject Loading, Menu, Creates, RoomMenu, ErrorMenu, FindRoom;
    int C1, C2, C3;
    private float num;
    public TMP_Text Create_Text, ErrorText, Roomtext;
    public Transform RoomList_Trans;
    public GameObject RoomList_Prefabs;
    RoomInfo roomInfo;
    public Transform PlayerList_Trans;
    public GameObject PlayerList_Prefabs;
    public GameObject StartGameButton;


    private void Awake()
    {
        GetClass = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Loading.SetActive(true);
       
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        print("Connected Using Setting");
    }

    public override void OnJoinedLobby()
    {
        Loading.SetActive(false);
        Menu.SetActive(true);
        print("Joined Lobby");
        PhotonNetwork.NickName = "PLayer " + Random.Range(0, 1000).ToString();

        C1 = Random.Range(0, 9999);
        C2 = Random.Range(0, 9999);
        C3 = Random.Range(0, 9999);
        num = C1 + C2 + C3;
    }

    public void CreateButton()
    {
        Menu.SetActive(false);
        Creates.SetActive(true);
        Create_Text.text = num.ToString();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(num.ToString());
        Loading.SetActive(true);
        Creates.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        Loading.SetActive(false);
        RoomMenu.SetActive(true);
        Roomtext.text = PhotonNetwork.CurrentRoom.Name;

        foreach(Transform child in PlayerList_Trans)
        {
            Destroy(child.gameObject);
        }

        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerList_Prefabs, PlayerList_Trans).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        RoomMenu.SetActive(false);
        Loading.SetActive(false);
        ErrorMenu.SetActive(true);
        ErrorText.text = "Failed to Create Room " + message;
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LeaveRoom()
    {
        RoomMenu.SetActive(false);
        Loading.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        Loading.SetActive(true);
        FindRoom.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        Loading.SetActive(false);
        Menu.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in RoomList_Trans)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;

            Instantiate(RoomList_Prefabs, RoomList_Trans).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerList_Prefabs, PlayerList_Trans).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }


    // Botton

    public void FindRoomButton()
    {
        Menu.SetActive(false);
        FindRoom.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void OkButton()
    {
        ErrorMenu.SetActive(false);
        Menu.SetActive(true);
    }

    public void FindBack()
    {
        FindRoom.SetActive(false);
        Menu.SetActive(true);
    }

}
