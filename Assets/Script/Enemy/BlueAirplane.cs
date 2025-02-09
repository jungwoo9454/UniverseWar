using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAirplane : Enemy
{
    float fBulletLaunch;
    float fWavelaunch;
    int nCount;
    private void Update()
    {
        fBulletLaunch += Time.deltaTime;
        if(fBulletLaunch >= 3.7f)
        {
            fWavelaunch += Time.deltaTime;
            if(fWavelaunch >= 0.2f)
            {
                SoundMng.ins.PlayEffect("EnemyBullet");
                nCount++;
                fWavelaunch = 0;
                BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, 180, 4, -40, fAttack);
                BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, 180, 3, -40, fAttack);
            }

            if(nCount >= 8)
            {
                fBulletLaunch = 0;
                fWavelaunch = 0;
                nCount = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        MyRigid.velocity = new Vector3(-fSpeed, 0, 0);
    }
}
