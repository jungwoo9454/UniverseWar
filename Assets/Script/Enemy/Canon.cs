using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : Enemy
{
    Vector3 Dir;
    float fCreateBulletTime;
    float fRandomReload;
    int nRandom;

    private void Update()
    {
        fCreateBulletTime += Time.deltaTime;
        fRandomReload += Time.deltaTime;
        switch (nRandom)
        {
            case 0:
                if(fCreateBulletTime >= 1.25f)
                {
                    SoundMng.ins.PlayEffect("EnemyBullet");
                    fCreateBulletTime = 0;
                    BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, 180, 8, 15, fAttack, BULLETCOLOR.YELLOW, Random.Range(3, 5));
                }
                break;
            case 1:
                if (fCreateBulletTime >= 0.7f)
                {
                    SoundMng.ins.PlayEffect("EnemyBullet");
                    fCreateBulletTime = 0;
                    float fAngle = Mathf.Atan2(GameMng.ins.PlayerTrans.position.y - MainMuzzle[0].position.y,
                                    GameMng.ins.PlayerTrans.position.x - MainMuzzle[0].position.x) * Mathf.Rad2Deg;
                    BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, fAngle, 0, 20, fAttack, BULLETCOLOR.RED, 10);
                }
                break;
            case 2:
                if (fCreateBulletTime >= 1.5f)
                {
                    SoundMng.ins.PlayEffect("EnemyBullet");
                    fCreateBulletTime = 0;
                    BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, 170, 0, 25, fAttack*0.7f, BULLETCOLOR.WHITE, 8);
                    BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, 180, 0, 25, fAttack*0.7f, BULLETCOLOR.WHITE, 8);
                    BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, 190, 0, 25, fAttack*0.7f, BULLETCOLOR.WHITE, 8);
                }
                break;
        }

        if(fRandomReload >= 4f)
        {
            fRandomReload = 0;
            fCreateBulletTime = 0;
            nRandom = Random.Range(0, 3);
        }
    }

    private void FixedUpdate()
    {
        if(MyTrans.position.x >= GameMng.ins.Right.position.x - 35)
            Dir = new Vector3(-fSpeed, 0, 0);
        else
            Dir = new Vector3(0, 0, 0);

        MyRigid.velocity = Dir;
    }
}
