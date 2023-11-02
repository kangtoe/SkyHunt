using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;

public enum GameState
{ 
    PlayerWait,
    Playing
}

// 점수 관리
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }
    private static GameManager instance;

    private void Start()
    {
        //PhotonNetwork.CurrentRoom.AutoCleanUp = false;
    }

    // 현재 게임 나가기 -> 버튼 이벤트
    public void QuitGame()
    {
        PhotonNetwork.LeaveRoom(); 
    }

    // 룸을 떠날 때 자동실행 (콜백)
    public override void OnLeftRoom()
    {
        // 마스터 클라이언트면 게임 종료
        OnwershipTrans();
        SceneManager.LoadScene("Lobby");
    }

    void OnwershipTrans()
    {
        Debug.Log("OnLeftRoom");

        // 모든 플레이어 리스트를 가져옵니다.
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        int otherPlayerActorNumber = -1;

        foreach (Photon.Realtime.Player player in players)
        {
            // 현재 플레이어가 아닌 다른 플레이어의 ActorNumber를 찾습니다.
            if (player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                // otherPlayerActorNumber를 사용하여 다른 플레이어의 ActorNumber를 얻을 수 있습니다.
                otherPlayerActorNumber = player.ActorNumber;
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            // 마스터 클라이언트 권한을 다른 플레이어에게 이전
            PhotonNetwork.SetMasterClient(PhotonNetwork.CurrentRoom.GetPlayer(otherPlayerActorNumber));
        }

        // 오브젝트 소유권을 이전하는 코드를 여기에서 실행합니다.
        // 현재 플레이어가 소유한 모든 오브젝트들을 가져옵니다.
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        foreach (PhotonView photonView in photonViews)
        {
            // 현재 플레이어가 소유한 오브젝트인 경우
            if (photonView.IsMine)
            {
                        // 소유권을 다른 플레이어에게 이전합니다.
                photonView.TransferOwnership(otherPlayerActorNumber);
            }
        }        
    }
}