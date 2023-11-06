using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShooter : ShooterBase
{
    int currentExp;
    int NextLevelExp => shooterLevel * 1100;    

    public int shooterLevel = 1; // 높은 레벨의 슈터는 더 강한 공격을 함
    protected int maxShooterLevel = 5; // 슈터 레벨의 최고 상한
    public bool IsMaxLevel => shooterLevel == maxShooterLevel;

    private void Start()
    {
        GetExp(0);
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        //UI위에 커서가 있을때 = ture
        if (EventSystem.current.IsPointerOverGameObject() == true) return;

        if (Input.GetMouseButton(0))
        {
            TryFire();
        }
    }

    public void GetExp(int i)
    {
        currentExp += i;
        if (currentExp >= NextLevelExp)
        {
            LevelUp();     
            int leftExp = currentExp - NextLevelExp;
            currentExp = leftExp;
        }

        float ratio;
        if (IsMaxLevel)
        {
            ratio = 1;
        }
        else
        {
            ratio = (float)currentExp / NextLevelExp;
        }
        UiManager.Instance.UpdateExpGage(ratio);
    }

    void LevelUp()
    {
        shooterLevel++;
        shooterLevel = Mathf.Clamp(shooterLevel, 1, maxShooterLevel);

        string str;
        if (IsMaxLevel)
        {            
            str = "MAX";
        }
        else
        {
            str = shooterLevel.ToString();
        }
        UiManager.Instance.UpdateLevelText(str);
    }

    protected override void Fire()
    {
        int numberOfBullets = shooterLevel; // 생성할 총알의 개수                

        Transform tf = firePoints[0];
        Vector3 pos = tf.position;
        Quaternion rot = tf.rotation;

        float intervalX = 0.2f;
        float intervalY = 0.2f;

        // 총알 생성
        for (int i = 0; i < numberOfBullets; i++)
        {
            float adjustX;
            float adjustY;

            if (numberOfBullets == 1)
            {
                adjustX = 0;                
            }
            else
            {                
                if (numberOfBullets % 2 == 1)
                {
                    // 발사체 생성 개수 홀수
                    int centerIdx = numberOfBullets / 2;
                    adjustX = centerIdx - i; adjustY = Mathf.Abs(adjustX);
                }
                else
                {
                    // 발사체 생성 개수 짝수
                    int centerIdxUp = numberOfBullets / 2;
                    int centerIdxDown = centerIdxUp - 1;
                    if (i <= centerIdxDown) adjustX = i - centerIdxDown - 0.5f;
                    else adjustX = i - centerIdxUp + 0.5f;

                    Debug.Log("i : "+ i +" || adjust : " + adjustX);
                }                
            }

            // 위치 조정
            adjustY = -Mathf.Abs(adjustX);
            pos = tf.TransformPoint(tf.localPosition + new Vector3(adjustX * intervalX, adjustY * intervalY, 0));

            // 발사체 생성
            GameObject go = CreateBullet(pos, rot);
            
            go.GetComponent<BulletBase>().Init_RPC(
            targetLayer, damage, impactPower, projectileMovePower, projectileLiveTime, color.r, color.g, color.b);       
        }

        SoundManager.Instance.PlaySound("Laser");
    }
}
