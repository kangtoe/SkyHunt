using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// linearDrag = 1�϶�, FixedUpdate -> AddForced 1�� ���� ���ϸ� ��ӿ�Ѵ�.
// ���� �� ��ӿ�� ���� �ӷ��� ������ ForceMode2D.Impulse�� ���� ���Ѵ�. 
// �̷��� ó���ϴ� ������ rigidbody�� ���� ������ ���� ����������, 
// lineardrag�� ���� ������ ���� ������ ������ ���̸鼭 ��ӿ�� �Ϸ��� ���� ���ÿ� ó���ϱ� �����̴�.
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

    // �����ӿ� ��ü �� �ణ�� ���̸� �д�.
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
