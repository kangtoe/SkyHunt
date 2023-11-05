using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerPrefs를 통한 정보 저장 & 불러오기
public static class SaveManager
{
    static string showTutorialKey = "showTutorial";
    static string bestScoreKey = "bestScore";    
    static string sfxVolumeKey = "sfxVolume";
    static string bgmVolumeKey = "bgmVolume";

    // 튜토리얼 보여줄 예정?
    public static bool ShowTutorial
    {
        get
        {
            int i = PlayerPrefs.GetInt(showTutorialKey);
            return i == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(showTutorialKey, value ? 1 : 0);
        }
    }

    // play Scene에서 최고 점수
    public static int BestScore
    {
        get
        {
            return PlayerPrefs.GetInt(bestScoreKey);
        }
        set
        {
            PlayerPrefs.SetInt(bestScoreKey, value);
        }
    }    

    // 배경 볼륨
    static float SfxVolume_default = 0.5f;
    public static float SfxVolume
    {
        get 
        {
            // 이전 저장 데이터 없는 경우 기본 값 할당
            if (!PlayerPrefs.HasKey(sfxVolumeKey))
            {
                PlayerPrefs.SetFloat(sfxVolumeKey, SfxVolume_default);
            }

            return PlayerPrefs.GetFloat(sfxVolumeKey);
        }
        set
        {
            PlayerPrefs.SetFloat(sfxVolumeKey, value);
        }
    }

    // 이팩트 볼륨
    static float BgmVolume_default = 0.5f;
    public static float BgmVolume
    {
        get
        {
            // 이전 저장 데이터 없는 경우 기본 값 할당
            if (!PlayerPrefs.HasKey(bgmVolumeKey))
            {
                PlayerPrefs.SetFloat(bgmVolumeKey, BgmVolume_default);
            }

            return PlayerPrefs.GetFloat(bgmVolumeKey);

        }
        set
        {
            PlayerPrefs.SetFloat(bgmVolumeKey, value);
        }
    }

    // 모든 저장 데이터 삭제 메소드
    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }
}
