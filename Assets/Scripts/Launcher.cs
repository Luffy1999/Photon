
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;


public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private TMP_Text ErrorMessage;
    [SerializeField] private Transform roomListContent;
    [SerializeField] private GameObject roomItemPrefab;
    [SerializeField] private Transform PlayerListContent;
    [SerializeField] private GameObject PlayerItemPrefab;
    [SerializeField] GameObject BotonStart;

    public static Launcher Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // conectar al master
        Debug.Log("Conectando");
        MenuManager.Instance.OpenMenuName("Loading");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        Debug.Log("Conectado a server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transfo in roomListContent)
        {
            Destroy(transfo.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            
            Instantiate(roomItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Conectado al lobby");
        MenuManager.Instance.OpenMenuName("Home");
        PhotonNetwork.NickName = "player" + Random.Range(0, 1000).ToString("0000");

    }
    
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
      
        MenuManager.Instance.OpenMenuName("Loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenuName("Room");
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        foreach (Transform playerT in PlayerListContent)
        {
            Destroy(playerT.gameObject);
        }

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++) 
        {
            Instantiate(PlayerItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        BotonStart.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        BotonStart.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ErrorMessage.text = "Error al crear la sala" + message;
        MenuManager.Instance.OpenMenuName("Error");
    }

    public void JoinRoom(RoomInfo _info)
    {
        PhotonNetwork.JoinRoom(_info.Name);
        MenuManager.Instance.OpenMenuName("Loading");
    }
    
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenuName("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenuName("Home");
    }
    
}
 
