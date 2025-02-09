using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMng : MonoBehaviour
{
    [Header("Player Info")]
    public Transform PlayerTrans;
    public PlayerAttack Playerattack;
    public PlayerStat Playerstat;
    public PlayerSkill PlayerSkill;
    public PlayerControl PlayerControl;

    public Transform Left;
    public Transform Right;
    public Transform Up;
    public Transform Down;

    public Vector3[] ModelScale;
    public Vector3[] MuzzlePosition;
    public Vector3[] LeftBoostposition;
    public Vector3[] RightBoostposition;
    public Vector3[] CollisionSize;
    public Vector3[] CollisionCentor;
    public Color[] BoostColor;
    public Color[] BoostLightColor;

    public Material[] ItemMat;

    public LayerMask EnemyMask;
    public LayerMask MeteoMask;

    public bool bPlayerDead;
    public bool bPause;

    float ScreenSizeX;
    float ScreenSizeY;

    public Transform GemPoint;

    private static GameMng gamemng;
    public static GameMng ins
    {
        get
        {
            if (gamemng == null)
            {
                gamemng = FindObjectOfType<GameMng>();

                if (gamemng == null)
                {
                    GameObject gameobj = new GameObject();
                    gameobj.name = "GameMng";
                    gamemng = gameobj.AddComponent<GameMng>();
                }
            }
            return gamemng;
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
        DataMng.ins.nMainWeapon = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].nMainWeapon;
        DataMng.ins.nSubWeapon = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].nSubWeapon;

        StageMng.ins.StageStart(DataMng.ins.nCurStage);

        PlayerTrans.localScale = ModelScale[DataMng.ins.nSelectAirplane];
        Playerattack.Muzzle.transform.localPosition = MuzzlePosition[DataMng.ins.nSelectAirplane];
        PlayerControl.Flames[0].transform.parent.localPosition = LeftBoostposition[DataMng.ins.nSelectAirplane];
        PlayerControl.Flames[1].transform.parent.localPosition = RightBoostposition[DataMng.ins.nSelectAirplane];

        PlayerControl.Flames[0].transform.localPosition = new Vector3(-0.33f, 0, 0);
        PlayerControl.Flames[1].transform.localPosition = new Vector3(-0.33f, 0, 0);

        var main0 = PlayerControl.Flames[0].main;
        var main1 = PlayerControl.Flames[1].main;

        main0.startColor = BoostColor[DataMng.ins.nSelectAirplane];
        main1.startColor = BoostColor[DataMng.ins.nSelectAirplane];

        PlayerControl.Flames[0].GetComponentInParent<Light>().color = BoostLightColor[DataMng.ins.nSelectAirplane];
        PlayerControl.Flames[1].GetComponentInParent<Light>().color = BoostLightColor[DataMng.ins.nSelectAirplane];

        Playerstat.fAttack = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].fAttack;
        Playerstat.fMaxHp = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].fHp;
        Playerstat.fHp = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].fHp;
        PlayerControl.fMoveSpeed = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].fMoveSpeed;
        Playerattack.fBulletLaunchTime = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].fBulletLaunchTime;
        Playerattack.fOriginalLaunchTIme = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].fBulletLaunchTime;

        PlayerTrans.GetComponent<MeshRenderer>().material = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].material;
        PlayerTrans.GetComponent<MeshFilter>().mesh = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].mesh;
        PlayerTrans.GetComponent<BoxCollider>().size = CollisionSize[DataMng.ins.nSelectAirplane];
        PlayerTrans.GetComponent<BoxCollider>().center = CollisionCentor[DataMng.ins.nSelectAirplane];
        PlayerTrans.localScale = ModelScale[DataMng.ins.nSelectAirplane];
        Playerstat.prevMat = DataMng.ins.planeinfo[DataMng.ins.nSelectAirplane].material;

        Playerstat.fHp = Mathf.Clamp(Playerstat.fHp, 0, Playerstat.fMaxHp);
    }

    void Update()
    {
        if (Cursor.visible && !bPause && StageMng.ins.bStage)
            Cursor.visible = false;
        else if(bPause || !StageMng.ins.bStage)
            Cursor.visible = true;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bPause = !bPause;
            UIMng.ins.PauseWindow.SetActive(bPause);
        }

        if (bPause)
        {
            if(StageMng.ins.bStage)
                Time.timeScale = 0;
            SoundMng.ins.EffectPause();
            SoundMng.ins.backGroundPause();
        }else
        {
            if (StageMng.ins.bStage)
                Time.timeScale = 1;
            SoundMng.ins.EffectUnPause();
            SoundMng.ins.backGroundUnPause();
        }

    }
}
