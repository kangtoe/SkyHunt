using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 볼, 플레이어 생성
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

    public Text scoreText;
    public Transform[] spawnPositions;
    public GameObject playerPrefab;
    public GameObject ballPrefab;

    private int[] playerScores;

    private void Start()
    {
        playerScores = new[] { 0, 0 };
        SpawnPlayerObject(); // 각 플레이어가 하나씩 오브젝트 생성

        if (PhotonNetwork.IsMasterClient) SpawnBall();
    }

    private void SpawnPlayerObject()
    {
        // 플레이어 엑터 넘버 -> 해당 스폰 지점 구하기
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        if (localPlayerIndex >= spawnPositions.Length)
        {
            Debug.LogError("localPlayerIndex error : " + localPlayerIndex);
            return;
        }
        var spawnPosition = spawnPositions[localPlayerIndex];

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);
    }

    private void SpawnBall()
    {
        PhotonNetwork.Instantiate(ballPrefab.name, Vector2.zero, Quaternion.identity).GetComponent<Ball>();
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

    public void AddScore(int playerNumber, int score)
    {
        if (!PhotonNetwork.IsMasterClient) return; // 호스트만 점수 증가 제어

        playerScores[playerNumber - 1] += score;

        photonView.RPC(nameof(RPCUpdateScoreText), RpcTarget.All, playerScores[0].ToString(), playerScores[1].ToString());
    }

    // 실행 : 마스터 -> 다른 모든 로컬로 전파
    [PunRPC]
    private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    {
        scoreText.text = $"{player1ScoreText} : {player2ScoreText}";
    }
}