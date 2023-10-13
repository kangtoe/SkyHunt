using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviourPun
{
    public float turnSpeed = 1;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        //target = GameManager.instance.GetCurrentPlayer();

        // ��ü �� ȸ���ð��� �ణ�� ���̸� �д�.
        float minMult = 0.9f;
        float maxMult = 1.1f;

        float randomMultiplier = Random.Range(minMult, maxMult);
        turnSpeed *= randomMultiplier;

        InvokeRepeating(nameof(UpdateTarget), 0, 1); // �ֱ⸶�� Ÿ�� ���� ����
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (target) RotateTo(target.position, turnSpeed);
    }

    void UpdateTarget()
    {
        target = PlayerSpwaner.Instance.GetClosestPlayer(transform.position).transform;
    }

    void RotateTo(Vector3 targetPos, float _rotateSpeed)
    {
        if (!target) return;

        Vector3 dir = targetPos - transform.position;

        // ȸ���� ���ϱ�
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // ȸ�� �� ���ϱ�
        Quaternion quat = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        // ȸ�� ����
        transform.rotation = Quaternion.Lerp(transform.rotation, quat, Time.deltaTime * _rotateSpeed);
    }
}
