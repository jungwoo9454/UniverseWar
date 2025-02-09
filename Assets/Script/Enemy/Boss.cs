using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public ParticleSystem Fire;

    float fAttackTime;
    float fbulletLaunchTime;

    float fLifeTime;

    bool bMark;
    float fAddAngle;

    float fMoveTime;

    bool bMoveWay;
    bool bMove;

    public bool bDown;

    int nPattern;

    bool Electronic;

    float fXMove;

    bool bAni;

    public ParticleSystem Charging;
    public Animator MyAni;

    private void FixedUpdate()
    {
        //EnemyRot(18,2.5f);

        fMoveTime += Time.deltaTime;

        if (fLifeTime >= 14.5f)
        {
            if (!bDown)
            {
                if (transform.position.x > 40)
                    fXMove = -4.5f;
                else
                    fXMove = 0;

                if (fMoveTime >= 1.8f)
                {
                    fMoveTime = 0;

                    if (Random.Range(0, 10) >= 5)
                        bMoveWay = false;
                    else
                        bMoveWay = true;
                }

                if (bMove)
                {
                    if (bMoveWay)
                        MyRigid.velocity = new Vector3(fXMove, 5, 0);
                    else
                        MyRigid.velocity = new Vector3(fXMove, -5, 0);
                }
                else
                {
                    MyRigid.velocity = Vector3.zero;
                }

                if (MyTrans.position.y >= 20)
                    bMoveWay = false;
                else if (MyTrans.position.y <= -19)
                    bMoveWay = true;
            }
            else
            {
                MyRigid.velocity = Vector3.down * fSpeed * 0.8f;
                MyRigid.rotation = Quaternion.Euler(Mathf.Lerp(0, -45, fMoveTime * 0.12f), 0, Mathf.Lerp(0, 50, fMoveTime * 0.15f));

                if(transform.position.y <= -20 && !bAni)
                {
                    End();
                    bAni = true;
                }
            }

        }
        else
        {
            if(fLifeTime >= 11.7f)
                MyRigid.velocity = new Vector3(-8, 0, 0);
        }

    }

    private void Update()
    {
        fAttackTime += Time.deltaTime;
        fLifeTime += Time.deltaTime;
        
        if(fLifeTime >= 14.5f)
        {
            if (!bDown)
            {
                if (fAttackTime >= 3.4f)
                {
                    nPattern = Random.Range(0, 3);
                    fAttackTime = 0;
                    Electronic = false;
                }

                switch (nPattern)
                {
                    case 0:
                        Pattern1();
                        break;
                    case 1:
                        Pattern2();
                        break;
                    case 2:
                        Pattern3();
                        break;
                }
            }
        }
    }

    public override void Dead()
    {
        if(bUse)
        {
            if (DotDmg != null)
                StopCoroutine(DotDmg);

            CameraMng.ins.Shake(0.18f, 0.3f);
            //TODO 점수표시 주석
            //UIMng.ins.CreatetextEffect(transform.position, Color.white, new Color(255, 0, 255), nScore, null, new Vector2(1.5f, 1.5f));
            DataMng.ins.nCurScore += nScore;
            DataMng.ins.nCurGem += EnemyCopyInfo.nGem;
            if (MyRenderer != null)
                MyRenderer.material = prevMat;
            else
                EnemySkinRendere.material = prevMat;

            bUse = false;

            MyAni.Play("Dead");
        }
    }

    void Pattern1()
    {
        bMove = false;
        if (AnimationMng.ins.GetAniState(GetComponent<Animator>(),"Empty") && !Electronic)
        {
            MyRigid.velocity = new Vector3(fXMove, -5, 0);
            Electronic = true;
            MyAni.Play("Attack1");
            Charging.Play();
            StartCoroutine(ElectroBall());
            SoundMng.ins.PlayEffect("Charging");
        }
    }

    void Pattern2()
    {
        bMove = true;
        fbulletLaunchTime += Time.deltaTime;
        if(fbulletLaunchTime >= 0.8f)
        {
            fbulletLaunchTime = 0;
            BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, 180, 9, 35, fAttack, BULLETCOLOR.YELLOW, 6);
        }
    }

    void Pattern3()
    {
        bMove = true;
        fbulletLaunchTime += Time.deltaTime;
        if (fbulletLaunchTime >= 0.25f)
        {
            fbulletLaunchTime = 0;
            if (fAddAngle >= 45)
                bMark = true;
            else if(fAddAngle <= -45)
                bMark = false;

            if (bMark)
                fAddAngle -= 7.5f;
            else
                fAddAngle += 7.5f;

            BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, 180 + fAddAngle, 0, 31, fAttack, BULLETCOLOR.WHITE, 6);
        }
    }

    public override void Create()
    {
        fLifeTime = 0;
        StartCoroutine(MovePoint());
        bDown = false;
        MyAni.Play("Empty");
    }

    IEnumerator ElectroBall()
    {
        yield return new WaitForSeconds(1.35f);
        float fAngle = Mathf.Atan2(GameMng.ins.PlayerTrans.position.y - MainMuzzle[0].position.y,
                GameMng.ins.PlayerTrans.position.x - MainMuzzle[0].position.x) * Mathf.Rad2Deg;
        BulletMng.ins.CreateElectroBall(false, Charging.transform.position, fAngle, 0, 100, 15);
        SoundMng.ins.PlayEffect("BossBulletFire");
        CameraMng.ins.Shake(0.2f, 1f);
    }

    IEnumerator MovePoint()
    {
        yield return new WaitForSeconds(12f);
        fAttackTime = 0;
        fMoveTime = 0;
        transform.position = new Vector3(92, 0, 0);
        StageMng.ins.bStagePause = false;
    }

    public void Shake()
    {
        CameraMng.ins.Shake(0.2f, 1f);
        EffectMng.ins.CreateEffect(transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), "Boom");
    }

    public void ParticPlay()
    {
        Fire.Play();
    }

    public void Down()
    {
        bDown = true;
        fMoveTime = 0;
    }

    public void End()
    {
        if(!GameMng.ins.bPlayerDead)
        {
            StageMng.ins.GameOver();
            UIMng.ins.ResultUI();
        }
    }
}
