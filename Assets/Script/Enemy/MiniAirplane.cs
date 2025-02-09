using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAirplane : Enemy
{
    Vector3 dir;
    float fBulletTime;

    // Update is called once per frame
    void Update()
    {
        fBulletTime += Time.deltaTime;

        if (fBulletTime >= 1.8f && bInCam && transform.position.x > GameMng.ins.PlayerTrans.position.x)
        {
            SoundMng.ins.PlayEffect("EnemyBullet");
            fBulletTime = 0;
            float fAngle = Mathf.Atan2(GameMng.ins.PlayerTrans.position.y - MainMuzzle[0].position.y,
                            GameMng.ins.PlayerTrans.position.x - MainMuzzle[0].position.x) * Mathf.Rad2Deg;
            BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, fAngle, 0, 40, fAttack, BULLETCOLOR.RED, 5);
        }
    }

    private void FixedUpdate()
    {
        dir = new Vector3(-fSpeed, 0, 0);
        MyRigid.velocity = dir;
    }
}
