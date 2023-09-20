using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

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

    //public Text scoreText;
    public Transform[] spawnPositions;
    //public GameObject[] playerBulletPrefabs;
    public GameObject playerPrefab;

    //public GameObject ballPrefab;
    //private int[] playerScores;
    [Header("디버그 : 현재 스폰된 플레이어 기체들")]
    [SerializeField]
    List<Transform> spwanedPlayerShips = new List<Transform>();

    private void Start()
    {
        //playerScores = new[] { 0, 0 };
        SpawnPlayerObject(); // 각 플레이어가 하나씩 오브젝트 생성
        
        if (PhotonNetwork.IsMasterClient)
        {
            // 볼 스폰
            //PhotonNetwork.Instantiate(ballPrefab.name, Vector2.zero, Quaternion.identity).GetComponent<Ball>();
        }        
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

        GameObject go = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);

        // 생성 후 처리        
        AddPlayer_RPC(go.GetPhotonView().ViewID);        
    }

    [PunRPC]
    void AddPlayer(int _viewId)
    {
        PhotonView photonView = PhotonView.Find(_viewId);
        GameObject newObject = photonView.gameObject;
        spwanedPlayerShips.Add(newObject.transform);
        Debug.Log("AddPlayer : " + newObject.name);

        // 각 플레이어 기체 색 변경
        SetPlayerColors_RPC();
    }

    void AddPlayer_RPC(int viewId)
    {        
        photonView.RPC(nameof(AddPlayer), RpcTarget.AllBuffered, viewId);
    }

    [PunRPC]
    void SetPlayerColors()
    {
        Color _color;
        for (int i = 0; i < spwanedPlayerShips.Count; i++)
        {            
            Debug.Log("spwanedPlayerShips count : " + spwanedPlayerShips.Count);
            Debug.Log("set playership color index : " + i);

            // 기체 및 탄환 색 설정                        
            //int tmp = i % 2;
            //if (tmp == 0) _color = Color.blue;
            //else _color = Color.green;            

            Transform ship = spwanedPlayerShips[i];            
            
            if (ship.GetComponent<PhotonView>().IsMine) _color = Color.blue;
            else _color = Color.green;            

            ColorCtrl shipColor = ship.GetComponent<ColorCtrl>();
            shipColor.SetColor(_color);
            ShooterBase shooter = ship.GetComponent<ShooterBase>();
            shooter.color = _color;
        }
    }

    void SetPlayerColors_RPC()
    {               
        photonView.RPC(nameof(SetPlayerColors), RpcTarget.All);
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

    public Transform GetClosestPlayer(Vector3 pos)
    {
        float closestDistance = Mathf.Infinity;
        Transform closestObject = null;

        foreach (Transform ship in spwanedPlayerShips)
        {
            // 플레이어 또는 기준 지점과 각 오브젝트 간의 거리를 계산
            float distance = Vector3.Distance(ship.position, pos);

            // 현재까지 가장 가까운 오브젝트보다 더 가까운 오브젝트를 찾으면 업데이트
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = ship;
            }
        }

        return closestObject;
    }

    //public void AddScore(int playerNumber, int score)
    //{
    //    if (!PhotonNetwork.IsMasterClient) return; // 호스트만 점수 증가 제어

    //    playerScores[playerNumber - 1] += score;

    //    photonView.RPC(nameof(RPCUpdateScoreText), RpcTarget.All, playerScores[0].ToString(), playerScores[1].ToString());
    //}

    //// 실행 : 마스터 -> 다른 모든 로컬로 전파
    //[PunRPC]
    //private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    //{
    //    scoreText.text = $"{player1ScoreText} : {player2ScoreText}";
    //}
}