using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSetting : MonoBehaviour
{
    string RES_KEY = "DROPDOWN_KEY";

    int currentOption;
    Dropdown options;

    List<string> optionList = new List<string>();

    void Awake()
    {
        if (PlayerPrefs.HasKey(RES_KEY) == false) currentOption = 0;
        else currentOption = PlayerPrefs.GetInt(RES_KEY);
    }

    void Start()
    {
        options = GetComponent<Dropdown>();

        options.ClearOptions();

        optionList.Add("1280 * 720");
        optionList.Add("1600 * 900");
        optionList.Add("1920 * 1080");                     

        options.AddOptions(optionList);

        options.value = currentOption;

        options.onValueChanged.AddListener(delegate { SetDropDown(options.value); });
        SetDropDown(currentOption); //최초 옵션 실행이 필요한 경우
    }

    void SetDropDown(int option)
    {
        PlayerPrefs.SetInt(RES_KEY, option);

        switch (option)
        {
            case 0:
                Screen.SetResolution(1280, 720, false);
                break;
            case 1:
                Screen.SetResolution(1600, 900, false);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, false);
                break;
        }

        // option 관련 동작
        Debug.Log("current option : " + option);
    }
}
