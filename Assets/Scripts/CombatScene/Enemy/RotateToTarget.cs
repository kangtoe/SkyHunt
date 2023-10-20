using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviourPun
{
    public float turnSpeed = 1;

    FindTarget Ft
    {
        get
        {
            if (!ft) ft = GetComponent<FindTarget>();
            return ft;
        }
    }
    FindTarget ft;
    Transform Target => Ft.target;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        //target = GameManager.instance.GetCurrentPlayer();        

        // 개체 별 회전시간에 약간의 차이를 둔다. // 멀티 환경에서 적용하고 싶은 경우 RPC 통신으로 함수 호출 필요(귀찮아서 안함)
        //float minMult = 0.9f;
        //float maxMult = 1.1f;

        //float randomMultiplier = Random.Range(minMult, maxMult);
        //turnSpeed *= randomMultiplier;
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (Target) RotateTo(Target.position, turnSpeed);
    }


    void RotateTo(Vector3 targetPos, float _rotateSpeed)
    {
        if (!Target) return;

        Vector3 dir = targetPos - transform.position;

        // 회전각 구하기
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // 회전 값 구하기
        Quaternion quat = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        // 회전 보간
        transform.rotation = Quaternion.Lerp(transform.rotation, quat, Time.deltaTime * _rotateSpeed);
    }
}
