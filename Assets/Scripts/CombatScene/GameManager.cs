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
        
    }

    // 현재 게임 나가기 -> 버튼 이벤트
    public void QuitGame()
    {
        PhotonNetwork.LeaveRoom(); // OnLeftRoom 자동실행 (콜백)
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

}