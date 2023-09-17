using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCtrl : MonoBehaviourPun
{
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!photonView.IsMine)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.blue;
        }
    }
}
