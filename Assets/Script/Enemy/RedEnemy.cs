using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemy : Enemy
{
    Vector3 dir;
    float fBulletTime;

    // Update is called once per frame
    void Update()
    {
        fBulletTime += Time.deltaTime;

        if(fBulletTime >= 2 && bInCam)
        {
            SoundMng.ins.PlayEffect("EnemyBullet");
            fBulletTime = 0;
            BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, 180, 0, 50, fAttack);
        }

        EnemyRot(24, 4);
    }

    private void FixedUpdate()
    {
        dir = new Vector3(-fSpeed, 0, 0);

        MyRigid.velocity = dir;
    }
}
