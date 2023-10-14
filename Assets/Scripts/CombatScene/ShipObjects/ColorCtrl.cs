using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCtrl : MonoBehaviourPun
{        
    [SerializeField]
    SpriteRenderer[] spriteRenderer;    
    [SerializeField]
    TrailRenderer[] trailRenderer;

    public void SetColor(Color color)
    {
        //if(!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        //if(!trailRenderer) trailRenderer = GetComponentInChildren<TrailRenderer>();

        //Debug.Log(name + " : set color = " + color);

        foreach (SpriteRenderer sprite in spriteRenderer)
        {
            sprite.color = color;            
        }
        foreach (TrailRenderer trail in trailRenderer)
        {
            trail.startColor = color;
            color.a = 0;
            trail.endColor = color;
        }
    }
}
