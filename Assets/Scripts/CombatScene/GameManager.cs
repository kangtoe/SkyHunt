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
        UiManager.Instance.UpdateMyNameText(PhotonNetwork.LocalPlayer.NickName);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            UiManager.Instance.InactiveOtherUi();
        }
        else
        {
            Photon.Realtime.Player other = PhotonNetwork.PlayerListOthers[0];
            UiManager.Instance.UpdateOtherNameText(other.NickName);
        }        
    }

    // 디버그용
    public void EndRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false; // 방을 닫습니다.
        PhotonNetwork.CurrentRoom.IsVisible = false; // 무작위 매치 메이킹에 공간이 보이지 않게 만듭니다.
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(GameSettings.LobbyScene);
    }

    public void GameOver()
    {
        UiManager.Instance.ActiveOverPanel();

        SoundManager.Instance.PlaySound("Over");

        // 점수 기록 표기
        {
            int currScore = ScoreManager.Instance.CurrScore;
            int recordScore = ScoreManager.Instance.BestRecord;

            // 취득 점수 표시
            UiManager.Instance.SetLadtScoreText(currScore);
            // 최고 점수 표시
            UiManager.Instance.SetBestScoreText(recordScore);

            if (currScore == 0) return;

            // 비교하여 신기록 표기 여부 결정 및 저장
            if (currScore > recordScore)
            {
                UiManager.Instance.ActiveRecordUi();
                ScoreManager.Instance.BestRecord = currScore;
            }
        }     
    }

    [PunRPC]
    void InactiveOtherUi()
    {
        UiManager.Instance.InactiveOtherUi();
    }

    // 현재 게임 나가기 -> 버튼 이벤트
    public void QuitGame()
    {
        SoundManager.Instance.PlaySound("Exit");
        
        // 다른 플레이어에서 현재 플레이어에 대한 UI 표시 비활성화
        photonView.RPC(nameof(InactiveOtherUi), RpcTarget.Others);
        
        //PhotonNetwork.AutomaticallySyncScene = false;      
        OnwershipTrans();
        PhotonNetwork.LeaveRoom(); 
    }

    // 룸을 떠날 때 자동실행 (콜백)
    public override void OnLeftRoom()
    {
        //Debug.Log("OnLeftRoom");
        
        SceneManager.LoadScene(GameSettings.LobbyScene);                
    }

    public void OnwershipTrans()
    {
        //roomOptions.CleanupCacheOnLeave = false;

        // 현재 방에 있는 플레이어 리스트를 가져옵니다.
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        // 자신 이외 방에 남은 플레이어 없음
        if (players.Length == 1)
        {
            Debug.Log("no actor left");
            return;
        }

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

        Debug.Log("OnwershipTrans to : " + otherPlayerActorNumber);

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
                // 플레이어 기체는 소유권 이전 안함
                if (photonView.GetComponent<MovePlayerShip>())
                {
                    Debug.Log("not TransferOwnership : " + photonView.name);
                    continue;
                } 

                // 소유권을 다른 플레이어에게 이전합니다.
                photonView.TransferOwnership(otherPlayerActorNumber);
            }
        }        
    }
}