using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    ShooterBase shooter;    

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        shooter = GetComponent<ShooterBase>();
        shooter.targetLayer = LayerMask.NameToLayer("Player");       
    }   
}
