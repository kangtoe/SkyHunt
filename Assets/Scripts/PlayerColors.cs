using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어별 고유 색 설정 목적으로 제작 // 현재 버전 실제 사용x
public class PlayerColors : MonoBehaviour
{
    public static PlayerColors Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<PlayerColors>();

            return instance;
        }
    }
    private static PlayerColors instance;

    [SerializeField]
    Color[] playerColors;

    public Color GetPlayerColor(int playerIdx)
    {
        int colorIdx = playerIdx % playerColors.Length;
        return playerColors[colorIdx];
    }
}
