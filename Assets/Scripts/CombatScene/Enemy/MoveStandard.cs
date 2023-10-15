using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// linearDrag = 1일때, FixedUpdate -> AddForced 1의 힘을 가하면 등속운동한다.
// 시작 시 등속운동과 같은 속력을 내도록 ForceMode2D.Impulse로 힘을 가한다. 
// 이렇게 처리하는 이유는 rigidbody를 통해 임의의 힘이 가해졌을때, 
// lineardrag를 통해 가해진 힘의 영향을 서서히 줄이면서 등속운동을 하려는 힘을 동시에 처리하기 위함이다.
public class MoveStandard : MonoBehaviour
{
    public float movePower = 1f;
    protected Rigidbody2D rbody;

    virtual protected void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        AdjustStat();

        rbody.AddForce(transform.up * movePower, ForceMode2D.Impulse);
    }

    // 움직임에 개체 별 약간의 차이를 둔다.
    void AdjustStat()
    {
        float minMult = 0.9f;
        float maxMult = 1.1f;

        float randomMultiplier = Random.Range(minMult, maxMult);
        movePower *= randomMultiplier;
    }

    virtual protected void FixedUpdate()
    {
        rbody.AddForce(transform.up * movePower * rbody.mass);
    }
}
