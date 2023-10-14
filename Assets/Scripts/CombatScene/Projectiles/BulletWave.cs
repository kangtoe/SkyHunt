using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �¿�� ������ �ٲپ�� ȸ��
// �������� ���ư��� ���ؼ��� ���� �� ȸ��
public class BulletWave : BulletBase
{
    [Header("Wave Info")]
    float ellapsedTime = 0f; //  �ﰢ �Լ� ������ �׸��� ���� ���Ǵ� ��� �ð�
    float moveMult = 0.02f; // �ﰢ �Լ� ������ �׸��� �ӵ�
    public float width = 5; // ����
    public float height = 5; // ����
    public bool waveInverse; // x��ǥ ���� ����

    override protected void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        MoveWave();
    }
     
    void MoveWave()
    {
        ellapsedTime += moveMult * movePower;

        float x1 = ellapsedTime;
        float y1 = Mathf.Sin(x1);

        float x2 = ellapsedTime += moveMult * movePower;
        float y2 = Mathf.Sin(x2);

        int dirX;
        if (waveInverse) dirX = -1;
        else dirX = 1;

        Vector2 delta = new Vector2(dirX * (y2 - y1) * width, (x2 - x1) * height);
        
        rbody.velocity = transform.rotation * delta;
    }
}
