using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMng : MonoBehaviour
{
    public PlayerData plyaerData;
    private static DataMng instance;
    public static DataMng ins
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataMng>();
                if (instance != null)
                    DontDestroyOnLoad(instance.gameObject);

                if (instance == null)
                {
                    GameObject dateobj = new GameObject("DataMng");

                    instance = dateobj.AddComponent<DataMng>();
                    DontDestroyOnLoad(dateobj);
                }
            }
            return instance;
        }
    }

    [Header("Weapon Number")]
    public int nMainWeapon;
    public int nSubWeapon;

    [Header("Volume Setting")]
    public float fMusicVol;
    public float fVFXVol;

    public int nCurStage;

    public int nCurScore;
    public int nCurGem;

    public int nSelectAirplane;

    public int nGem;

    public string CurStageName;


    public const int nTotalStage = 4;

    public int[] nHighScores;

    public bool bReset;

    public bool[] bStageLock;

    public AirPlaneInfo[] planeinfo;

    public bool bMobile;

    private void Awake()
    {
        nHighScores = new int[nTotalStage];
        bStageLock = new bool[nTotalStage];
        bStageLock[0] = true;
        nHighScores = plyaerData.nHighScores;
        nGem = plyaerData.nGem;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.F1))
        {
            nGem += 100;
        }
    }

    private void OnDestroy()
    {
        plyaerData.nHighScores = nHighScores;
        plyaerData.nGem = nGem;
    }
}
