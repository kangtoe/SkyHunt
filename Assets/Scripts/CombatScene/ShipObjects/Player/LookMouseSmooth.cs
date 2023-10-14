using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookMouseSmooth : MonoBehaviourPun
{
    Vector3 mousePos;
    public float rotateSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;    

        mousePos = GetMouseWorldPos();
        //mousePos.z = 0f;
        //Debug.Log("mosue pos : " + mousePos);
        
        LookPosSmooth(mousePos, rotateSpeed);        
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        return worldPos;
    }

    void LookPosSmooth(Vector3 targetPos, float _rotateSpeed)
    {
        Vector3 dir = targetPos - transform.position;
         
        // 회전각 구하기
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // 회전 값 구하기
        Quaternion quat = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        // 회전 보간
        transform.rotation = Quaternion.Lerp(transform.rotation, quat, Time.deltaTime * _rotateSpeed);
    }

}
