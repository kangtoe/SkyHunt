using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    private int[] playerScores;
    public Text scoreText;

    private void Start()
    {
        playerScores = new[] { 0, 0 };
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
