using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAirplane : Enemy
{
    float fBulletLaunchTime;
    Vector3 Dir;

    private void Update()
    {
        fBulletLaunchTime += Time.deltaTime;

        if(fBulletLaunchTime > 2.25f)
        {
            SoundMng.ins.PlayEffect("EnemyBullet");
            fBulletLaunchTime = 0;
            StartCoroutine(DoubleShot());
        }
    }

    IEnumerator DoubleShot()
    {
        BulletMng.ins.CreateBullet(false, MainMuzzle[0].position, 174, 0, 30, fAttack);
        MainMuzzle[0].GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.27f);
        BulletMng.ins.CreateBullet(false, MainMuzzle[1].position, 184, 0, 30, fAttack);
        MainMuzzle[1].GetComponentInChildren<ParticleSystem>().Play();
    }

    private void FixedUpdate()
    {
        //if(Dir == Vector3.zero)
        //    Dir = new Vector3(-fSpeed * 0.3f, fSpeed, 0);

        //if (MyTrans.position.y > GameMng.ins.Up.position.y - 10)
        //    Dir = new Vector3(-fSpeed * 0.3f, -fSpeed, 0);
        //else if (MyTrans.position.y < GameMng.ins.Down.position.y + 10)
        //    Dir = new Vector3(-fSpeed * 0.3f, fSpeed, 0);

        Dir = new Vector3(-fSpeed, Mathf.Sin((transform.position.x / 1.2f) * 0.2f) * 17, 0);

        MyRigid.velocity = Dir.normalized * fSpeed;

        EnemyRot(24,4);
    }
}
