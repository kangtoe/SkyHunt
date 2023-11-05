using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviourPun
{
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<ScoreManager>();

            return instance;
        }
    }
    private static ScoreManager instance;

    int currScore = 0;
    public int CurrScore => currScore;

    const string RECORD_KEY = "SCORE_RECORD";
    public int BestRecord
    {
        get
        {
            if (PlayerPrefs.HasKey(RECORD_KEY) == false) return 0;
            else return PlayerPrefs.GetInt(RECORD_KEY);
            
        }
        set
        {
            PlayerPrefs.SetInt(RECORD_KEY, value);
        }
    }

    private void Start()
    {
        //디버그용
        //PlayerPrefs.DeleteKey(RECORD_KEY);
    }

    public void AddScore(int score)
    {
        Debug.Log("AddScore : " + score + " || player " + photonView.OwnerActorNr);

        // 내 로컬 점수 증가
        currScore += score;
        UiManager.Instance.UpdateMyScoreText(currScore);

        // 다른 로컬에서 내 점수 업데이트
        photonView.RPC(nameof(UpdateOtherScore), RpcTarget.OthersBuffered, currScore);

        // 슈터 경험치 획득
        GameObject myShip = PlayerSpwaner.Instance.GetMyPlayer();
        PlayerShooter shooter = myShip.GetComponent<PlayerShooter>();
        shooter.GetExp(score);
    }

    // 다른 로컬에서 내 로컬의 점수를 갱신
    [PunRPC]
    public void UpdateOtherScore(int score)
    {        
        UiManager.Instance.UpdateOtherScoreText(score);
    }
}
