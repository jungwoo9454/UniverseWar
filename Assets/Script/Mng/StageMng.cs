using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public delegate void STAGEFUNC();

public class StageMng : MonoBehaviour
{
    //const int nStages = 2;
    public bool bStagePause;
    public bool bStage;        //현재 스테이지 진행중인지 아닌지

    int nCurStage;

    List<Coroutine> EnemyCo = new List<Coroutine>();

    STAGEFUNC[] StageFunc = new STAGEFUNC[2];

    private static StageMng stagemng;
    public static StageMng ins
    {
        get
        {
            if (stagemng == null)
            {
                stagemng = FindObjectOfType<StageMng>();
                if (stagemng != null)
                    DontDestroyOnLoad(stagemng.gameObject);
                if (stagemng == null)
                {
                    GameObject stageobj = new GameObject();
                    stageobj.name = "StageMng";
                    stagemng = stageobj.AddComponent<StageMng>();
                    DontDestroyOnLoad(stageobj);
                }
            }
            return stagemng;
        }
    }

    private void Start()
    {
        if (StageFunc[0] == null)
        {
            StageFunc[0] = Stage1;
            StageFunc[1] = Stage2;
        }
    }
    public void StageStart(int stage)
    {
        nCurStage = stage;

        EnemyMng.ins.SetDisable();

        DataMng.ins.nCurGem = 0;
        DataMng.ins.nCurScore = 0;

        //TODO 주석 제거
        AnimationMng.ins.play("Intro", DirectorUpdateMode.UnscaledGameTime);

        bStage = true;
        StartCoroutine(StageStart());

        for (int i = 0; i < EnemyCo.Count; i++)
            StopCoroutine(EnemyCo[i]);

        EnemyCo.Clear();
    }

    IEnumerator StageStart()
    {
        bStagePause = true;
        yield return new WaitForSeconds(4.5f);
        bStagePause = false;

        StageFunc[nCurStage]();
    }

    public void GameOver()
    {

        bStage = false;
        DataMng.ins.nGem += DataMng.ins.nCurGem;
        DataMng.ins.bStageLock[DataMng.ins.nCurStage + 1] = true;

        if (DataMng.ins.nCurScore > DataMng.ins.nHighScores[nCurStage])
        {
            DataMng.ins.nHighScores[nCurStage] = DataMng.ins.nCurScore;
        }

        for (int i = 0; i < EnemyCo.Count; i++)
            StopCoroutine(EnemyCo[i]);

        EnemyCo.Clear();
    }

    void Stage1()
    {
        Stage1Enter();
    }

    void Stage2()
    {
        Stage2Enter();
    }

    void Stage1Enter()
    {

        Enemyspawn(1, "RedEnemy", new Vector3(70, -15, 0));
        Enemyspawn(2f, "RedEnemy", new Vector3(70, 10, 0));
        Enemyspawn(3f, "RedEnemy", new Vector3(70, -15, 0));
        Enemyspawn(4f, "RedEnemy", new Vector3(70, 0, 0));

        Enemyspawn(8f, "Blade", new Vector3(75, 10, 0));
        Enemyspawn(8f, "Blade", new Vector3(75, -10, 0));

        Enemyspawn(11.5f, "RedEnemy", new Vector3(80, 12, 0));
        Enemyspawn(11.5f, "MiniAirplane", new Vector3(72, 0, 0));
        Enemyspawn(11.5f, "RedEnemy", new Vector3(80, -12, 0));

        Enemyspawn(15f, "MiniAirplane", new Vector3(70, 0, 0));
        Enemyspawn(20f, "MiniAirplane", new Vector3(70, 15, 0));
        Enemyspawn(20f, "MiniAirplane", new Vector3(70, -15, 0));

        Enemyspawn(25f, "RedAirplane", new Vector3(70, -15, 0));
        Enemyspawn(25f, "RedAirplane", new Vector3(70, 15, 0));

        Enemyspawn(30f, "BlueAirplane", new Vector3(70, -17, 0));
        Enemyspawn(30f, "BlueAirplane", new Vector3(70, 17, 0));
        Enemyspawn(35f, "BlueAirplane", new Vector3(70, 0, 0));

        Enemyspawn(40f, "Blade", new Vector3(75, 70, 0));
        Enemyspawn(40f, "Blade", new Vector3(75, -70, 0));
        Enemyspawn(40f, "Blade", new Vector3(-75, -70, 0));
        Enemyspawn(40f, "Blade", new Vector3(-75, 70, 0));


        Enemyspawn(45f, "Canon", new Vector3(70, 0, 0));
        Enemyspawn(48f, "RedEnemy", new Vector3(75, 10, 0));
        Enemyspawn(48f, "RedEnemy", new Vector3(70, 0, 0));
        Enemyspawn(48f, "RedEnemy", new Vector3(75, -10, 0));

        Enemyspawn(55f, "BlueAirplane", new Vector3(70, 0, 0));
        Enemyspawn(60f, "Canon", new Vector3(70, -16, 0));
        Enemyspawn(60f, "Canon", new Vector3(70, 16, 0));

        Enemyspawn(68f, "MiniAirplane", new Vector3(70, 0, 0));
        Enemyspawn(68f, "MiniAirplane", new Vector3(70, 15, 0));
        Enemyspawn(68f, "MiniAirplane", new Vector3(70, -15, 0));
        Enemyspawn(72f, "RedAirplane", new Vector3(70, -15, 0));
        Enemyspawn(72f, "RedAirplane", new Vector3(70, 15, 0));


        Enemyspawn(79f, "Blade", new Vector3(75, 70, 0));
        Enemyspawn(79f, "Blade", new Vector3(75, -70, 0));
        Enemyspawn(79f, "Blade", new Vector3(-75, -70, 0));
        Enemyspawn(79f, "Blade", new Vector3(-75, 70, 0));
        Enemyspawn(79f, "Blade", new Vector3(0, 75, 0));
        Enemyspawn(79f, "Blade", new Vector3(0, -75, 0));

        EnemyCo.Add(StartCoroutine(BossAni(90, "INSECT")));
        Enemyspawn(93, "Insect", new Vector3(90, 0, 0));
    }

    void Stage2Enter()
    {
        Enemyspawn(1f, "RedEnemy", new Vector3(70, 15, 0));
        Enemyspawn(1f, "RedEnemy", new Vector3(70, 0, 0));
        Enemyspawn(1f, "RedEnemy", new Vector3(70, -15, 0));

        Enemyspawn(7f, "MiniAirplane", new Vector3(70, 15, 0));
        Enemyspawn(7f, "MiniAirplane", new Vector3(70, -15, 0));


        Enemyspawn(12f, "RedEnemy", new Vector3(70, 13, 0));
        Enemyspawn(12f, "RedEnemy", new Vector3(70, -13, 0));

        Enemyspawn(17f, "Canon", new Vector3(70, 12, 0));
        Enemyspawn(18f, "BlueAirplane", new Vector3(70, -7, 0));

        Enemyspawn(24f, "Blade", new Vector3(75, 30, 0));
        Enemyspawn(24f, "Blade", new Vector3(-75, 30, 0));
        Enemyspawn(24f, "Blade", new Vector3(75, -30, 0));
        Enemyspawn(24f, "Blade", new Vector3(-75, -30, 0));


        Enemyspawn(30, "MiniAirplane", new Vector3(70, 0, 0));


        Enemyspawn(33, "RedAirplane", new Vector3(70, 0, 0));


        Enemyspawn(36f, "BlueAirplane", new Vector3(70, 12, 0));
        Enemyspawn(36f, "BlueAirplane", new Vector3(70, -12, 0));

        Enemyspawn(38f, "Blade", new Vector3(75, -20, 0));
        Enemyspawn(38f, "Blade", new Vector3(75, 20, 0));
        Enemyspawn(38f, "Blade", new Vector3(-75, 20, 0));
        Enemyspawn(38f, "Blade", new Vector3(-75, -20, 0));


        Enemyspawn(45f, "RedEnemy", new Vector3(70, 18, 0));
        Enemyspawn(45f, "RedEnemy", new Vector3(70, 18, 0));

        Enemyspawn(48f, "Canon", new Vector3(70, 0, 0));

        Enemyspawn(50f, "RedAirplane", new Vector3(70, -15, 0));
        Enemyspawn(50f, "RedAirplane", new Vector3(70, 15, 0));

        Enemyspawn(55f, "MiniAirplane", new Vector3(70, -11, 0));
        Enemyspawn(55f, "MiniAirplane", new Vector3(70, 11, 0));

        Enemyspawn(60f, "BlueAirplane", new Vector3(70, 0, 0));

        Enemyspawn(63f, "RedEnemy", new Vector3(70, 0, 0));

        Enemyspawn(66f, "RedEnemy", new Vector3(70, 7, 0));
        Enemyspawn(66f, "RedEnemy", new Vector3(70, 7, 0));

        Enemyspawn(69f, "RedEnemy", new Vector3(70, 12, 0));
        Enemyspawn(69f, "RedEnemy", new Vector3(70, 0, 0));
        Enemyspawn(69f, "RedEnemy", new Vector3(70, -12, 0));


        Enemyspawn(75f, "Canon", new Vector3(70, 0, 0));

        Enemyspawn(78f, "Blade", new Vector3(75, -20, 0));
        Enemyspawn(78f, "Blade", new Vector3(75, 20, 0));
        Enemyspawn(78f, "Blade", new Vector3(-75, 20, 0));
        Enemyspawn(78f, "Blade", new Vector3(-75, -20, 0));

        Enemyspawn(83f, "MiniAirplane", new Vector3(70, -11, 0));
        Enemyspawn(83f, "MiniAirplane", new Vector3(70, 11, 0));


        Enemyspawn(88f, "RedAirplane", new Vector3(70, 12, 0));
        Enemyspawn(88f, "BlueAirplane", new Vector3(70, 0, 0));
        Enemyspawn(88f, "RedAirplane", new Vector3(70, -12, 0));


        EnemyCo.Add(StartCoroutine(BossAni(98, "PH-452")));
        Enemyspawn(101, "Boss", new Vector3(100, 0, 0));
    }

    void Enemyspawn(float fTime, string name, Vector3 pos)
    {
        EnemyCo.Add(StartCoroutine(EnemyspawnCo(fTime, name, pos)));
    }

    IEnumerator EnemyspawnCo(float fTime, string name, Vector3 pos)
    {
        yield return new WaitForSeconds(fTime);
        EnemyMng.ins.CreateEnemy(name, pos);
    }

    IEnumerator BossAni(float fTime, string name)
    {
        yield return new WaitForSeconds(fTime);
        AnimationMng.ins.play("Boss", DirectorUpdateMode.UnscaledGameTime);
        UIMng.ins.Bosstex.text = name;
        bStagePause = true;
    }
}
