using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CombatTest : MonoBehaviourPun
{
    [SerializeField]
    Text txt;
    
    [Space]

    [SerializeField]
    GameObject enemy1;
    [SerializeField]
    GameObject enemy2;
    [SerializeField]
    GameObject enemy3;
    [SerializeField]
    GameObject enemy4;
    [SerializeField]
    GameObject enemy5;
    [SerializeField]
    GameObject enemy6;
    [SerializeField]
    GameObject enemy7;
    [SerializeField]
    GameObject enemy8;
    [SerializeField]
    GameObject enemy9;
    [SerializeField]
    GameObject enemy0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnemySpwaner.instance.SpwanEnemyRandomPos(enemy1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EnemySpwaner.instance.SpwanEnemyRandomPos(enemy2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EnemySpwaner.instance.SpwanEnemyRandomPos(enemy3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EnemySpwaner.instance.SpwanEnemyRandomPos(enemy4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            EnemySpwaner.instance.SpwanEnemyRandomPos(enemy5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            EnemySpwaner.instance.SpwanEnemyRandomPos(enemy6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            EnemySpwaner.instance.SpwanEnemyRandomPos(enemy7);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            EnemySpwaner.instance.SpwanEnemyRandomPos(enemy8);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            EnemySpwaner.instance.SpwanEnemyRandomPos(enemy9);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            EnemySpwaner.instance.SpwanEnemyRandomPos(enemy0);
        }
    }

    private void OnValidate()
    {
        string str = "";
        str += "IsMasterClient : " + PhotonNetwork.IsMasterClient + "\n";
        str += "Enemy 1 : " + (enemy1 ? enemy1.name : "") + "\n";
        str += "Enemy 2 : " + (enemy2 ? enemy2.name : "") + "\n";
        str += "Enemy 3 : " + (enemy3 ? enemy3.name : "") + "\n";
        str += "Enemy 4 : " + (enemy4 ? enemy4.name : "") + "\n";
        str += "Enemy 5 : " + (enemy5 ? enemy5.name : "") + "\n";
        str += "Enemy 6 : " + (enemy6 ? enemy6.name : "") + "\n";
        str += "Enemy 7 : " + (enemy7 ? enemy7.name : "") + "\n";
        str += "Enemy 8 : " + (enemy8 ? enemy8.name : "") + "\n";
        str += "Enemy 9 : " + (enemy9 ? enemy9.name : "") + "\n";
        str += "Enemy 0 : " + (enemy0 ? enemy0.name : "") + "\n";

        txt.text = str;
    }
}
