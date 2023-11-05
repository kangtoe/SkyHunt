using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// 볼륨 제어
public class VolumeControl : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;

    [SerializeField]
    float min_dB = -40;
    [SerializeField]
    float max_dB = 0;

    [SerializeField]
    float currentVolume_dB_bgm;
    [SerializeField]
    float currentVolume_dB_sfx;

    [Header("볼륨 슬라이더 ui")]
    [SerializeField]
    Slider slider_sfx;
    [SerializeField]
    Slider slider_bgm;

    [Range(0, 1)]
    [SerializeField]
    float bgmVolume;
    public float BgmVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        {
            bgmVolume = value;
            UpdateMixer();
        }
    }

    [Range(0, 1)]
    [SerializeField]
    float sfxVolume;
    public float SfxVolume
    {
        get
        {
            return sfxVolume;
        }
        set
        {
            sfxVolume = value;            
            UpdateMixer();
        }
    }

    private void Start()
    {
        BgmVolume = SaveManager.BgmVolume;
        SfxVolume = SaveManager.SfxVolume;

        // 슬라이더 상호작용 설정 : 변경한 슬라이더 값 적용 및 저장
        {
            if (slider_sfx)
            {
                slider_sfx.onValueChanged.AddListener(delegate
                {
                    float val = slider_sfx.value;
                    // 저장 값 수정
                    SaveManager.SfxVolume = val;
                    // 실제 볼륨 조절
                    SfxVolume = val;
                });

                // 슬라이더 : 현재 볼륨에 따라 슬라이더 값 할당
                slider_sfx.value = SaveManager.SfxVolume;
            }

            if (slider_bgm)
            {
                slider_bgm.onValueChanged.AddListener(delegate
                {
                    float val = slider_bgm.value;
                    // 저장 값 수정
                    SaveManager.BgmVolume = val;
                    // 실제 볼륨 조절
                    BgmVolume = val;
                });

                // 슬라이더 : 현재 볼륨에 따라 슬라이더 값 할당
                slider_bgm.value = SaveManager.BgmVolume;
            }                        
        }
    }

    private void OnValidate()
    {
        UpdateMixer();
    }

    // 실제 볼륨 제어
    void UpdateMixer()
    {
        currentVolume_dB_bgm = Get_dB(bgmVolume);
        audioMixer.SetFloat("BGM", currentVolume_dB_bgm);

        currentVolume_dB_sfx = Get_dB(sfxVolume);
        audioMixer.SetFloat("SFX", currentVolume_dB_sfx);
    }

    float Get_dB(float volume)
    {
        float dB;
        if (volume == 0) dB = - 80; // 완전 음소거
        else dB = Mathf.Lerp(min_dB, max_dB, volume);

        return dB;
    }
}
