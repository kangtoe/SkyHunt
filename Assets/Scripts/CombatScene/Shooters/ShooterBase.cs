using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class ShooterBase : MonoBehaviourPun
{    
    public Transform[] firePoints;
    public float fireDelay; // 기본 탄환 발사간격 (스톰슈터에서 레벨이 오를 수록 간격이 줄어듦)
    protected float lastFireTime = 0f; // 마지막 탄환 사격 시점

    [Header("Projectile Info")]
    public GameObject projectilePrefab; // 발사할 탄환
    //public GameObject hitEffect;// 발사체 프리펩에서 지정할것 : 동기화 직렬화 불가
    public LayerMask targetLayer; 
    public int damage = 0;
    public int impactPower = 0;
    public int projectileMovePower = 10;
    public float projectileLiveTime = 3f;
    public Color color; // 발사체 색

    // 사격 시도
    protected virtual void TryFire()
    {
        //Debug.Log("TryFire");        

        // 마지막 발사로부터 충분한 시간 간격이 있었는가
        if (Time.time >= lastFireTime + fireDelay)
        {
            // 마지막 발사시점 갱신
            lastFireTime = Time.time;

            //Debug.Log("fire!");
            Fire();
        }
    }

    protected virtual GameObject CreateBullet(Vector3 pos, Quaternion rot)
    {
        // 발사체 생성
        string str = "Projectiles/" + projectilePrefab.name;
        GameObject go = PhotonNetwork.Instantiate(str, pos, rot);
        return go;        
    }

    // 실제 사격 -> shooter의 firePoint 방향대로 projectile을 생성
    protected virtual void Fire()
    {        
        foreach (Transform firePoint in firePoints)
        {            
            Vector3 pos = firePoint.position;
            Quaternion quat = firePoint.rotation;

            GameObject go = CreateBullet(pos, quat);            
            
            go.GetComponent<BulletBase>().Init_RPC(
                targetLayer, damage, impactPower, projectileMovePower, projectileLiveTime, color.r, color.g, color.b);
        }        
    }
}
