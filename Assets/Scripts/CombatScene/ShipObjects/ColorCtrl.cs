using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCtrl : MonoBehaviourPun
{        
    SpriteRenderer spriteRenderer;    
    TrailRenderer trailRenderer;

    public void SetColor(Color color)
    {
        if(!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if(!trailRenderer) trailRenderer = GetComponentInChildren<TrailRenderer>();

        //Debug.Log(name + " : set color = " + color);

        spriteRenderer.color = color;

        if (trailRenderer)
        {
            trailRenderer.startColor = color;
            color.a = 0;
            trailRenderer.endColor = color;
        }
    }
}
