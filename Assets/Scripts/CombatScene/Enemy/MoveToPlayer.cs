using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현재 방향과 관련없이 player를 향해 이동하도록 힘을 가한다 
public class MoveToPlayer : MonoBehaviour
{
    public float movePower = 1f;
    Rigidbody2D rbody;

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

    Vector2 Dir
    {
        get {
            if (Target) return (Target.position - transform.position).normalized;
            else return transform.up;
        }
    }

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();          
        rbody.AddForce(Dir * movePower, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        rbody.AddForce(Dir * movePower * rbody.mass);
    }
}
