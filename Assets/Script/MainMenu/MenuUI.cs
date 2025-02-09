using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class MenuUI : MonoBehaviour
{
    public GameObject Hangar;
    public GameObject Option;
    public GameObject Stage;
    public GameObject MainMenuBtn;
    public StageSwipe swipe;

    public Animator CamAni;
    public Animator AirPlane;
    public Animator HangarAni;
    public Animator MainMenu;
    public Material Disolve;
    //TODO 비행기 무기 변경
    //public WeaponSwipe MainweaponSwipe;
    //public WeaponSwipe SubweaponSwipe;

    public Text GemTex;
    public Text BuyText;

    public Text TexName;
    public Text TexHp;
    public Text TexAttack;
    public Text TexSpeed;
    public Text TexAttackTime;
    public int nCurAirplane;

    public Sprite StageLockImg;

    public Image[] Weapon = new Image[2];
    public Sprite[] WeaponIcon;

    public Slider VFX;
    public Slider Music;

    float ScreenSizeX;
    float ScreenSizeY;

    PostProcessVolume PV;
    PostProcessLayer PL;

    private static MenuUI menuUi;
    public static MenuUI ins
    {
        get
        {
            if (menuUi == null)
            {
                menuUi = FindObjectOfType<MenuUI>();

                if (menuUi == null)
                {
                    GameObject bulletobj = new GameObject();
                    bulletobj.name = "MenuMng";
                    menuUi = bulletobj.AddComponent<MenuUI>();
                }
            }
            return menuUi;
        }
    }

    private void Awake()
    {

        if (Screen.width == ScreenSizeX && Screen.height == ScreenSizeY) return;

        float targetaspect = 16.0f / 9.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;
        Camera camera = FindObjectOfType<Camera>();

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

        ScreenSizeX = Screen.width;
        ScreenSizeY = Screen.height;
    }

    private void Start()
    {
        BuyText.text = "구매하기 - " + DataMng.ins.planeinfo[nCurAirplane].nPrice.ToString() + "GEM";
        TexName.text = DataMng.ins.planeinfo[nCurAirplane].name;
        TexHp.text = "체력 : " + DataMng.ins.planeinfo[nCurAirplane].fHp.ToString();
        TexSpeed.text = "이동속도 : " + DataMng.ins.planeinfo[nCurAirplane].fMoveSpeed.ToString();
        TexAttackTime.text = "공격속도 : " + DataMng.ins.planeinfo[nCurAirplane].fBulletLaunchTime.ToString(); ;
        TexAttack.text = "공격력 : " + DataMng.ins.planeinfo[nCurAirplane].fAttack.ToString();
        EffectMng.ins.CreateEffect(new Vector3(1000, 1000, 1000), "WhiteBulletDead");
        EnemyMng.ins.Load();
        SoundMng.ins.PlayBackground("MainMenu");

        PV = FindObjectOfType<PostProcessVolume>();
        PL = FindObjectOfType<PostProcessLayer>();
    }

    private void Update()
    {
        GemTex.text = DataMng.ins.nGem.ToString();
        if (!Cursor.visible)
            Cursor.visible = true;

        SetEffVolume();
        SetBackVolume();
    }


    void Change()
    {
        AirPlane.Play("Disolve");
        TexName.text = DataMng.ins.planeinfo[nCurAirplane].name;
        TexHp.text = "체력 : " + DataMng.ins.planeinfo[nCurAirplane].fHp.ToString();
        TexSpeed.text = "이동속도 : " + DataMng.ins.planeinfo[nCurAirplane].fMoveSpeed.ToString();
        TexAttackTime.text = "공격속도 : " + DataMng.ins.planeinfo[nCurAirplane].fBulletLaunchTime.ToString();
        TexAttack.text = "공격력 : " + DataMng.ins.planeinfo[nCurAirplane].fAttack.ToString();

        Weapon[0].sprite = WeaponIcon[DataMng.ins.planeinfo[nCurAirplane].nMainWeapon];
        Weapon[1].sprite = WeaponIcon[DataMng.ins.planeinfo[nCurAirplane].nSubWeapon];

        if (!DataMng.ins.planeinfo[nCurAirplane].bOwn)
        {
            BuyText.text = "구매하기 - " + DataMng.ins.planeinfo[nCurAirplane].nPrice.ToString() + "GEM";
        }
        else
        {
            if (DataMng.ins.nSelectAirplane != nCurAirplane)
                BuyText.text = "선택";
            else
                BuyText.text = "선택완료";
        }

        //TODO 비행기 무기 변경
        //MainweaponSwipe.SetPos(DataMng.ins.planeinfo[nCurAirplane].nMainWeapon);
        //SubweaponSwipe.SetPos(DataMng.ins.planeinfo[nCurAirplane].nSubWeapon);
    }

    public string GetlevelString(LEVEL level)
    {
        switch(level)
        {
            case LEVEL.EASY:
                return "LEVEL - EASY";
            case LEVEL.NORMAL:
                return "LEVEL - NORMAL";
            case LEVEL.HARD:
                return "LEVEL - HARD";
        }

        return "LEVEL - NONE";
    }

    public void ExitClient()
    {
        Application.Quit();
        SoundMng.ins.PlayEffect("ButtonClick");
    }

    public void EnterHangar()
    {
        nCurAirplane = 0;
        Change();
        GemTex.text = DataMng.ins.nGem.ToString();
        Hangar.SetActive(true);
        HangarAni.Play("HangarEnter");
        CamAni.Play("Hangar");
        MainMenu.Play("MainMenuEnter");
        SoundMng.ins.PlayEffect("ButtonClick");
    }

    public void ExitHangar()
    {
        Hangar.SetActive(false);
        HangarAni.Play("HangarExit");
        CamAni.Play("MainMenu");
        MainMenu.Play("MainMenuExit");
        SoundMng.ins.PlayEffect("ButtonClick");
    }
    
    public void EnterStage()
    {
        Stage.SetActive(true);
        swipe.ScaleReset();
        SoundMng.ins.PlayEffect("ButtonClick");
    }

    public void ExitStage()
    {
        Stage.SetActive(false);
        SoundMng.ins.PlayEffect("ButtonClick");
    }

    public void EnterOption()
    {
        Option.SetActive(true);
        MainMenuBtn.SetActive(false);
        SoundMng.ins.PlayEffect("ButtonClick");
        PV.enabled = false;
    }

    public void ExitOption()
    {
        Option.SetActive(false);
        MainMenuBtn.SetActive(true);
        SoundMng.ins.PlayEffect("ButtonClick");
        PV.enabled = true;
    }

    public void LeftWay()
    {
        if (nCurAirplane - 1 >= 0 && AnimationMng.ins.GetAniState(AirPlane, "Empty"))
        {
            nCurAirplane--;
            nCurAirplane = Mathf.Clamp(nCurAirplane, 0, DataMng.ins.planeinfo.Length - 1);
            Change();
        }
        SoundMng.ins.PlayEffect("ButtonClick");
    }

    public void BuyBtn()
    {
        if(DataMng.ins.planeinfo[nCurAirplane].nPrice <= DataMng.ins.nGem && !DataMng.ins.planeinfo[nCurAirplane].bOwn)
        {
            DataMng.ins.nGem -= DataMng.ins.planeinfo[nCurAirplane].nPrice;
            DataMng.ins.planeinfo[nCurAirplane].bOwn = true;
            BuyText.text = "선택";
        }
        else if(DataMng.ins.planeinfo[nCurAirplane].bOwn)
        {
            DataMng.ins.nSelectAirplane = nCurAirplane;
            BuyText.text = "선택완료";
        }
        SoundMng.ins.PlayEffect("ButtonClick");
    }

    public void RightWay()
    {
        if (nCurAirplane + 1 < DataMng.ins.planeinfo.Length && AnimationMng.ins.GetAniState(AirPlane, "Empty"))
        {
            nCurAirplane++;
            nCurAirplane = Mathf.Clamp(nCurAirplane, 0, DataMng.ins.planeinfo.Length - 1);
            Change();
        }
        //nCurAirplane++;
        //Change();
        SoundMng.ins.PlayEffect("ButtonClick");
    }

    public void ButtonEnterSnd()
    {
        SoundMng.ins.PlayEffect("ButtonEnter");
    }

    public void SetEffVolume()
    {
        DataMng.ins.fVFXVol = VFX.value;
    }

    public void SetBackVolume()
    {
        DataMng.ins.fMusicVol = Music.value;
    }
}
