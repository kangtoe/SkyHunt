using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어(자기, 다른 플레이어) 체력 UI 제어
// 스코어 UI 제어
// 옵션 버튼 UI

public class UiManager : MonoBehaviour
{
    public static UiManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UiManager>();

            return instance;
        }
    }
    private static UiManager instance;

    [SerializeField]
    Text otherDeathText;

    [SerializeField]
    Image myHpImage;
    [SerializeField]
    Image otherHpImage;

    [SerializeField]
    Text myScoreText;
    [SerializeField]
    Text otherScoreText;

    [SerializeField]
    Text myNameText;
    [SerializeField]
    Text otherNameText;

    [Header("Score record")]
    [SerializeField]
    Text bestScoreText;
    [SerializeField]
    Text lastScoreText;
    [SerializeField]
    Text recordText;

    [Header("level info")]
    [SerializeField]
    Text levelText;
    [SerializeField]
    Image expImage;

    [Header("Panels")]
    [SerializeField]
    GameObject optionPanel;
    [SerializeField]
    GameObject overPanel;

    bool isOnOption = false; // 옵션 판넬 활성화 중? 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 옵션 버튼 황성화 중, UI 제외한 어느 클릭이라도 있으면 판넬 닫기
    }

    public void SetBestScoreText(int i)
    {        
        bestScoreText.text = "BEST SCORE : " + i.ToString("D5");
    }

    public void SetLadtScoreText(int i)
    {        
        lastScoreText.text = "SCORE : " + i.ToString("D5");
    }

    public void ActiveRecordUi()
    {
        recordText.enabled = true;
    }

    public void ActiveOverPanel()
    {
        InactiveOptionPanel();
        overPanel.SetActive(true);
    }

    public void ActiveOptionPanel()
    {
        optionPanel.SetActive(true);
        isOnOption = true;
    }

    public void InactiveOptionPanel()
    {
        optionPanel.SetActive(false);
        isOnOption = false;
    }    

    public void InactiveOtherUi()
    {
        otherNameText.transform.parent.gameObject.SetActive(false);
        //otherHpImage.enabled = false;
        //otherNameText.enabled = false;
        //otherScoreText.enabled = false;
        //otherDeathText.enabled = false;
    }

    public void ActiveOtherDeathUi()
    {
        otherDeathText.enabled = true;
    }

    public void UpdateMyScoreText(int i)
    {
        myScoreText.text = "SCORE : " + i.ToString("D6");
    }
    public void UpdateOtherScoreText(int i)
    {
        Debug.Log("UpdateOtherScoreText : " + i);
        otherScoreText.text = "SCORE : " + i.ToString("D6");
    }

    public void UpdateMyHpGage(float ratio)
    {
        myHpImage.fillAmount = ratio;
    }

    public void UpdateOtherHpGage(float ratio)
    {
        otherHpImage.fillAmount = ratio;
    }

    public void UpdateExpGage(float ratio)
    {
        expImage.fillAmount = ratio;
    }

    public void UpdateLevelText(string str)
    {
        levelText.text = "LV. " + str;
    }

    public void UpdateMyNameText(string str)
    {
        myNameText.text = str;        
    }

    public void UpdateOtherNameText(string str)
    {
        otherNameText.text = str;    
    }
}
