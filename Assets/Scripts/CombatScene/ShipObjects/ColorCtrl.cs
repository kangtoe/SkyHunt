using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCtrl : MonoBehaviourPun
{
    Color myColor;

    SpriteRenderer spriteRenderer;
    TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();        

        if (!photonView.IsMine)
        {
            myColor = spriteRenderer.color = Color.green;
        }
        else
        {
            myColor = spriteRenderer.color = Color.red;
        }

        spriteRenderer.color = myColor;

        if (trailRenderer)
        {
            trailRenderer.startColor = myColor;
            myColor.a = 0;
            trailRenderer.endColor = myColor;
        }        
    }
}
