using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyMainPanel : MonoBehaviourPunCallbacks
{
    [Header("Login Panel")]
    public GameObject LoginPanel;

    public InputField PlayerNameInput;

    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;

    public InputField RoomNameInputField;
    public InputField MaxPlayersInputField;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomPanel;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;

    [Header("User Name Text")]
    public Text userNameUi;

    [Space(20)]
    public Button StartGameButton;
    public GameObject PlayerListEntryPrefab;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    #region UNITY

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();

        PlayerNameInput.text = "Player " + Random.Range(1000, 10000);

        UpdateUserNameUi();
        if (PhotonNetwork.IsConnectedAndReady)
        {
            OnConnectedToMaster();
        }
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        this.SetActivePanel(SelectionPanel.name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {        
        CreateRoom();        
    }

    public override void OnJoinedRoom()
    {
        // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
        cachedRoomList.Clear();

        SetActivePanel(InsideRoomPanel.name);

        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(InsideRoomPanel.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<MyPlayerListEntry>().Initialize(p.ActorNumber, p.NickName);

            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(GameSettings.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<MyPlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
            }

            playerListEntries.Add(p.ActorNumber, entry);
        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());

        Hashtable props = new Hashtable
            {
                {GameSettings.PLAYER_LOADED_LEVEL, false}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(SelectionPanel.name);

        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        playerListEntries.Clear();
        playerListEntries = null;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        GameObject entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(InsideRoomPanel.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<MyPlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListEntries.Add(newPlayer.ActorNumber, entry);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(GameSettings.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<MyPlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
            }
        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    #endregion

    #region UI CALLBACKS

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        SoundManager.Instance.PlaySound("Click");
        SetActivePanel(SelectionPanel.name);
    }

    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;            
            PhotonNetwork.ConnectUsingSettings();
            UpdateUserNameUi();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }

        SoundManager.Instance.PlaySound("Click");
    }

    public void OnCreateRoomButtonClicked_OnSelection()
    {
        SetActivePanel("CreateRoomPanel");
        string roomName = "Room " + Random.Range(1000, 10000);
        RoomNameInputField.text = roomName;

        SoundManager.Instance.PlaySound("Click");
    }

    public void OnCreateRoomButtonClicked_OnRoomSetting()
    {
        string roomName = RoomNameInputField.text;        
        CreateRoom(roomName);

        SoundManager.Instance.PlaySound("Click");
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        SetActivePanel(JoinRandomRoomPanel.name);

        // 무작위 방에 참가합니다. 만약 해당 조건을 만족하는 방이 없으면 콜백에서 새로운 방 생성.
        PhotonNetwork.JoinRandomRoom();

        SoundManager.Instance.PlaySound("Click");
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();

        SoundManager.Instance.PlaySound("Click");
    }  

    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        SetActivePanel(RoomListPanel.name);

        SoundManager.Instance.PlaySound("Click");
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        // 모든 룸 참가자 Main 씬 로드
        PhotonNetwork.LoadLevel("Combat");

        SoundManager.Instance.PlaySound("Start");
    }

    #endregion

    void CreateRoom(string roomName = null)
    {
        roomName = string.IsNullOrEmpty(roomName) ? "Room " + Random.Range(1000, 10000) : roomName;        
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    void UpdateUserNameUi()
    {
        string str = PhotonNetwork.LocalPlayer.NickName;
        Debug.Log("UpdateUserNameUi : " + str);
        if (str == "") userNameUi.text = "";
        else userNameUi.text = "USER : " + str;        
    }

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(GameSettings.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    public void LocalPlayerPropertiesUpdated()
    {
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    private void SetActivePanel(string activePanel)
    {
        LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
        CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
        JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
        RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
        InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<MyRoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, (byte)info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }
}