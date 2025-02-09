using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMng : MonoBehaviour
{
    [Header("Player Stat UI")]
    public Image PlayerHp;
    public Text PlayerHpText;
    public Image PlayerUpgrade;
    public Text PlayerUpgradeText;

    [Header("Game UI")]
    public Text CurScore;
    public Text CurGem;
    public GameObject PauseWindow;

    [Header("Text Effect")]
    public Transform WorldCanvas;
    public GameObject OriginalTextEffect;
    List<TextEffect> TextEffectList = new List<TextEffect>();

    [Header("Result UI")]
    public Text Score;
    public Text HighScore;
    public Text Gem;
    public Text WhatStage;
    public Text StageName;
    public Text Grade;


    public Text StageNametex;


    public Text Bosstex;

    public Sprite[] Weaponicon;
    public Sprite[] ItemIcons;
    public Image MainWeapon;
    public Image Subweapon;
    public Image Itemimg;

    [ColorUsage(true, true)]
    public Color ResultVitoryColor;
    [ColorUsage(true, true)]
    public Color ResultDefeatColor;
    public Color DefeatTextColor;
    public Color VictoryTextColor;
    public Text Resulttext;
    public MeshRenderer ResultLaser;
    public Color[] GradeColors;
    public Color[] GradeOutColors;
    public GameObject MobileUI;

    private static UIMng uimng;
    public static UIMng ins
    {
        get
        {
            if (uimng == null)
            {
                uimng = FindObjectOfType<UIMng>();

                if (uimng == null)
                {
                    GameObject uiobj = new GameObject();
                    uiobj.name = "UIMng";
                    uimng = uiobj.AddComponent<UIMng>();
                }
            }
            return uimng;
        }
    }

    private void Start()
    {
        //if (DataMng.ins.bMobile)
        //    MobileUI.SetActive(true);
        //else
        //    MobileUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStatUI();
        GameUI();

        StageName.text = DataMng.ins.CurStageName;

        if(GameMng.ins.Playerattack.bThisMain)
        {
            MainWeapon.sprite = Weaponicon[DataMng.ins.nMainWeapon];
            Subweapon.sprite = Weaponicon[DataMng.ins.nSubWeapon];
        }
        else
        {
            MainWeapon.sprite = Weaponicon[DataMng.ins.nSubWeapon];
            Subweapon.sprite = Weaponicon[DataMng.ins.nMainWeapon];
        }

        if(GameMng.ins.PlayerSkill.ThisItem == ITEM._NONE)
        {
            Itemimg.gameObject.SetActive(false);
        }
    }

    void PlayerStatUI()
    {
        PlayerHp.fillAmount = Mathf.Lerp(PlayerHp.fillAmount, GameMng.ins.Playerstat.fHp / GameMng.ins.Playerstat.fMaxHp, Time.deltaTime * 4);
        PlayerHpText.text = GameMng.ins.Playerstat.fHp.ToString("N0") + " /" + GameMng.ins.Playerstat.fMaxHp.ToString();

        PlayerUpgrade.fillAmount = Mathf.Lerp(PlayerUpgrade.fillAmount, GameMng.ins.Playerstat.fUpgrade / 100, Time.deltaTime * 4);
        PlayerUpgradeText.text = GameMng.ins.Playerstat.fUpgrade.ToString("N0") + " /" + 100;
    }

    void GameUI()
    {
        CurScore.text = "SCORE : " + DataMng.ins.nCurScore.ToString();
        CurGem.text = DataMng.ins.nCurGem.ToString();
    }

    public void PauseBtn()
    {
        GameMng.ins.bPause = true;
        PauseWindow.SetActive(true);
    }

    public void ResultUI()
    {
        if (GameMng.ins.bPlayerDead)
        {
            Resulttext.color = DefeatTextColor;
            Resulttext.text = "DEFEAT";
            ResultLaser.material.SetColor("_Color", ResultDefeatColor);
        }
        else
        {
            Resulttext.color = VictoryTextColor;
            Resulttext.text = "VICTORY";
            ResultLaser.material.SetColor("_Color", ResultVitoryColor);
        }

        Score.text = DataMng.ins.nCurScore.ToString();
        HighScore.text = DataMng.ins.nHighScores[DataMng.ins.nCurStage].ToString();
        Gem.text = DataMng.ins.nCurGem.ToString();
        WhatStage.text = "STAGE " + (DataMng.ins.nCurStage + 1).ToString();
        StageName.text = DataMng.ins.CurStageName;


        if (DataMng.ins.nCurScore >= 10000)
        {
            Grade.text = "S";
            Grade.color = GradeColors[6];
            Grade.GetComponent<Outline>().effectColor = GradeOutColors[6];
        }
        else if (DataMng.ins.nCurScore >= 8500)
        {
            Grade.text = "A";
            Grade.color = GradeColors[5];
            Grade.GetComponent<Outline>().effectColor = GradeOutColors[5];
        }
        else if (DataMng.ins.nCurScore >= 7000)
        {
            Grade.text = "B";
            Grade.color = GradeColors[4];
            Grade.GetComponent<Outline>().effectColor = GradeOutColors[4];
        }
        else if (DataMng.ins.nCurScore >= 5000)
        {
            Grade.text = "C";
            Grade.color = GradeColors[3];
            Grade.GetComponent<Outline>().effectColor = GradeOutColors[3];
        }
        else if (DataMng.ins.nCurScore >= 3000)
        {
            Grade.text = "D";
            Grade.color = GradeColors[2];
            Grade.GetComponent<Outline>().effectColor = GradeOutColors[2];
        }
        else if (DataMng.ins.nCurScore >= 1500)
        {
            Grade.text = "E";
            Grade.color = GradeColors[1];
            Grade.GetComponent<Outline>().effectColor = GradeOutColors[1];
        }
        else
        {
            Grade.text = "F";
            Grade.color = GradeColors[0];
            Grade.GetComponent<Outline>().effectColor = GradeOutColors[0];
        }

        Invoke("ScreenADShow", 13);

        AnimationMng.ins.play("Result", DirectorUpdateMode.UnscaledGameTime);
    }


    public void ReStart()
    {
        Time.timeScale = 1f;
        GameMng.ins.bPause = false;
        PauseWindow.SetActive(false);
        SoundMng.ins.EffectUnPause();
        SoundMng.ins.backGroundUnPause();
        Scene.ins.FadeScene("Game");
    }

    public void GotoMenu()
    {
        Time.timeScale = 1f;
        Scene.ins.FadeScene("Menu");
    }

    public void CloseResult()
    {
        Time.timeScale = 1f;
    }

    public void Play()
    {
        Time.timeScale = 1;
        GameMng.ins.bPause = false;
        PauseWindow.SetActive(false);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        PauseWindow.SetActive(false);
        GameMng.ins.bPause = false;
        SoundMng.ins.EffectUnPause();
        SoundMng.ins.backGroundUnPause();
        Scene.ins.FadeScene("Menu");
    }

    public void CreatetextEffect(Vector3 pos, Color color, Color outColor, float amount, string text , Vector2 scale)
    {
        bool findtext = false;
        for (int i = 0; i < TextEffectList.Count; i++)
        {
            if (!TextEffectList[i].GetUse())
            {
                TextEffectList[i].CreateTextEffect(pos, amount, color, outColor, text, scale);
                findtext = true;
                break;
            }
        }


        if (!findtext)
        {
            TextEffect dmgtext = Instantiate(OriginalTextEffect).GetComponent<TextEffect>();
            TextEffectList.Add(dmgtext);
            dmgtext.transform.SetParent(WorldCanvas, false);
            dmgtext.CreateTextEffect(pos, amount, color, outColor, text, scale);
        }
    }

}
