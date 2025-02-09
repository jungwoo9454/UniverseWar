using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : Enemy
{
    Animator MyAni;
    Vector3 dir;
    float fBulletTime;
    float fAttackTime;

    float fMoveTime;


    float fLifeTime;
    bool bAttack;

    // Update is called once per frame
    void Update()
    {
        if (MyAni == null)
            MyAni = GetComponent<Animator>();

        fBulletTime += Time.deltaTime;
        fLifeTime += Time.deltaTime;

        if(fLifeTime >= 14f)
        {
            if (fBulletTime >= 5.5f)
            {
                bAttack = true;
                fAttackTime += Time.deltaTime;

                if (fAttackTime >= 0.25f)
                {
                    if (!AnimationMng.ins.GetAniState(MyAni, "Attack"))
                    {
                        switch (Random.Range(0, 3))
                        {
                            case 0:
                                EnemyMng.ins.CreateEnemy("RedEnemy", new Vector3(75, 16, 0));
                                EnemyMng.ins.CreateEnemy("RedEnemy", new Vector3(75, -16, 0));
                                break;
                            case 1:
                                EnemyMng.ins.CreateEnemy("BlueAirplane", new Vector3(75, 15, 0));
                                EnemyMng.ins.CreateEnemy("RedAirplane", new Vector3(75, 0, 0));
                                break;
                            case 2:
                                EnemyMng.ins.CreateEnemy("MiniAirplane", new Vector3(75, 16, 0));
                                EnemyMng.ins.CreateEnemy("MiniAirplane", new Vector3(75, -16, 0));
                                break;
                        }
                        MyAni.SetTrigger("Attack");
                        SoundMng.ins.PlayEffect("InsectFire");
                    }

                    fAttackTime = 0;
                    int n = Random.Range(10, 15);
                    for (int i = 0; i < n; i++)
                    {
                        BulletMng.ins.CreateBullet(false, MainMuzzle[0].position + new Vector3(0, Random.Range(-1, 1), 0), Random.Range(170, 190), 0, Random.Range(35, 45), fAttack, BULLETCOLOR.GREEN, 5, "Sphere");
                    }
                }

                if (fBulletTime >= 6.25f)
                {
                    bAttack = false;
                    fBulletTime = 0;
                    fAttackTime = 0;
                }
            }
        }
    }

    public override void Dead()
    {
        base.Dead();

        if (!GameMng.ins.bPlayerDead)
        {
            StageMng.ins.GameOver();
            UIMng.ins.ResultUI();
        }
    }

    private void FixedUpdate()
    {
        fMoveTime += Time.deltaTime;

        if (fLifeTime >= 14f)
        {
            if (fMoveTime >= 1.4f)
            {
                fMoveTime = 0;

                if (transform.position.x > 32)
                {
                    if (Random.Range(0, 100) > 50)
                    {
                        if (transform.position.y > 20)
                            dir = new Vector3(-fSpeed, -fSpeed, 0);
                        else
                            dir = new Vector3(-fSpeed, fSpeed, 0);
                    }
                    else
                    {
                        if (transform.position.y < -20)
                            dir = new Vector3(-fSpeed, fSpeed, 0);
                        else
                            dir = new Vector3(-fSpeed, -fSpeed, 0);
                    }
                }
                else
                {
                    if (Random.Range(0, 100) > 50)
                    {
                        if (transform.position.y > 20)
                            dir = new Vector3(0, -fSpeed, 0);
                        else
                            dir = new Vector3(0, fSpeed, 0);
                    }
                    else
                    {
                        if (transform.position.y < -20)
                            dir = new Vector3(0, fSpeed, 0);
                        else
                            dir = new Vector3(0, -fSpeed, 0);
                    }
                }
            }
        }
        else
        {
            if (fLifeTime >= 11.0f)
                dir = new Vector3(-fSpeed * 1.2f, 0, 0);
        }

        if (bAttack)
        {
            dir = Vector3.zero;
        }

        MyRigid.velocity = dir;
    }

    public override void Create()
    {
        fLifeTime = 0;
        StartCoroutine(PosSet());
    }

    IEnumerator PosSet()
    {
        yield return new WaitForSeconds(10);
        transform.position = new Vector3(80, 0, 0);
        StageMng.ins.bStagePause = false;
    }
}
