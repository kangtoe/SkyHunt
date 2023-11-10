using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyInfo : MonoBehaviourPun
{
    public int point = 100;
    public int spwanMinWave = 0; // 이 이후의 wave에만 등장 가능

    //private void Start()
    //{
    //    Damageable damageable = GetComponent<Damageable>();        
        
    //    damageable.onDeadLocal.AddListener(delegate
    //    {
    //        Debug.Log("damageable.lastHitObjOwner : " + damageable.lastHitObjOwner + "|| PhotonNetwork.LocalPlayer.ActorNumber: " + PhotonNetwork.LocalPlayer.ActorNumber);

    //        if (damageable.lastHitObjOwner != PhotonNetwork.LocalPlayer.ActorNumber) return;
    //        ScoreManager.Instance.AddScore(point);
    //    });
    //}
}
