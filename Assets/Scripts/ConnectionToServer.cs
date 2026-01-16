using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Collections.Generic;

public class ConnectionToServer : MonoBehaviourPunCallbacks
{
    public static ConnectionToServer Instance;
    [SerializeField] private TMP_InputField inputRoomName;
    [SerializeField] private TMP_Text roomName;

    [SerializeField] private Transform transformRoomList;
    [SerializeField] private GameObject roomItemPrefab;

    [SerializeField] private GameObject playerListItem;
    [SerializeField] private Transform transformPlayerList;

    [SerializeField] private GameObject startGameButton;
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in transformRoomList)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomItemPrefab, transformRoomList).GetComponent<RoomItem>().SetUp(roomList[i]);
        }
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
    }
    
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        Instance = this;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        WindowsManager.Layout.OpenLayout("MainMenu");
        Debug.Log("Connected to Lobby !");
    }

    public void StartGameLevel(int levelIndex)
    {
        PhotonNetwork.LoadLevel(levelIndex);
    }
    
    public void CreateNewRoom() 
    {
        if (string.IsNullOrEmpty(inputRoomName.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(inputRoomName.text);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.IsMasterClient) startGameButton.SetActive(true);
        else startGameButton.SetActive(false);
    }
    
    public override void OnJoinedRoom()
    {
        WindowsManager.Layout.OpenLayout("GameRoom");
        
        if(PhotonNetwork.IsMasterClient) startGameButton.SetActive(true);
        else startGameButton.SetActive(false);
        
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform trans in transformPlayerList)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItem, transformPlayerList).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        WindowsManager.Layout.OpenLayout("MainMenu");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItem, transformPlayerList).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void ConnectToRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    
}
