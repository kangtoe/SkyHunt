using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShooterBase : MonoBehaviourPun
{    
    public Transform[] firePoints;
    public float fireDelay; // �⺻ źȯ �߻簣�� (���轴�Ϳ��� ������ ���� ���� ������ �پ��)
    protected float lastFireTime = 0f; // ������ źȯ ��� ����

    [Header("Projectile Info")]
    public GameObject projectilePrefab; // �߻��� źȯ
    public GameObject hitEffect;
    public LayerMask targetLayer;
    public int damage = 0;
    public int impactPower = 0;
    public int projectileMovePower = 10;
    public float projectileLiveTime = 3f;
    public Color? projectileColor = null;

    // ��� �õ�
    protected virtual void TryFire()
    {
        //Debug.Log("TryFire");

        // ������ �߻�κ��� ����� �ð� ������ �־��°�
        if (Time.time >= lastFireTime + fireDelay)
        {
            // ������ �߻���� ����
            lastFireTime = Time.time;

            //Debug.Log("fire!");
            Fire();
        }
    }

    // ���� ��� -> shooter�� firePoint ������ projectile�� ����
    protected virtual void Fire()
    {
        //if (!PhotonNetwork.IsMasterClient) return;

        foreach (Transform firePoint in firePoints)
        {
            string name = "Projectiles/" + projectilePrefab.name;
            Vector3 pos = firePoint.position;
            Quaternion quat = firePoint.rotation;           

            // �߻�ü ����
            GameObject go = PhotonNetwork.Instantiate(name, pos, quat);
            go.GetComponent<BulletBase>().Init(
                hitEffect, targetLayer, damage, impactPower, projectileMovePower, projectileLiveTime);
        }        
    }
}
