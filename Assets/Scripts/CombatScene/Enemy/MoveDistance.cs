using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// targe과 일정거리 유지
public class MoveDistance : MoveStandard
{
    public float distance = 3;
    public float relaxZone = 1;

    FindTarget Ft
    {
        get {
            if (!ft) ft = GetComponent<FindTarget>();
            return ft;
        }
    }
    FindTarget ft;
    Transform Target => Ft.target;


    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        if (!Target)
        {
            rbody.AddForce(transform.up * movePower * rbody.mass);
            return;
        }

        float currentDist = (Target.transform.position - transform.position).magnitude;

        if (currentDist > distance + relaxZone)
        {
            // 너무 먼 거리 -> 접근
            rbody.AddForce(transform.up * movePower);
        }
        else if (currentDist < distance - relaxZone)
        {
            // 너무 가까운 거리 -> 후퇴
            rbody.AddForce(transform.up * movePower * -1);
        }
        else
        {            
            // 적정 거리 -> 우측 이동
            rbody.AddForce(transform.right * movePower * 0.5f);
        }
    }
}
