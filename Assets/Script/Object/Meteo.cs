using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : Unit
{
    public Vector3 Rotation;

    Vector3 Dir;

    float fSpeed;
    float fDmg;

    bool bUse;

    void Update()
    {
        transform.Rotate(Rotation * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        MyRigid.velocity = Dir * fSpeed;
    }

    public bool GetDead()
    {
        return bUse;
    }

    public override void TakeDmg(float dmg)
    {
        //fHp -= dmg;
        //if (fHp <= 0)
        //{
        //    DataMng.ins.nCurScore += 20;
        //    CameraMng.ins.Shake(0.2f, 0.4f);
        //    Dead();
        //    return;
        //}
        //else
        //{
        //    HitAni();
        //    fSpeed *= 0.9f;
        //    transform.localScale *= 0.95f;
        //    EffectMng.ins.CreateEffect(transform.position, "MeteoHit");
        //}
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(10f);
        MyRenderer.material = prevMat;
        gameObject.SetActive(false);
        bUse = false;
    }

    public override void Dead()
    {
        SoundMng.ins.PlayEffect("EnemyDead");
        EffectMng.ins.CreateEffect(transform.position, "MeteoBoom");
        MyRenderer.material = prevMat;
        gameObject.SetActive(false);
        bUse = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Meteo_TriggerEnter");
            CameraMng.ins.Shake(0.5f, 1);
            other.GetComponent<PlayerStat>().TakeDmg(fDmg);
            EffectMng.ins.CreateEffect(GameMng.ins.PlayerTrans.position, "ElectricalSparks");
            EffectMng.ins.MeteoHit();
            SoundMng.ins.PlayEffect("MeteoHit");
            Dead();
        }

        if(other.CompareTag("Bullet"))
        {
            if(other.GetComponent<Bullet>().bPlayer)
            {
                other.GetComponent<Bullet>().Kill();
                fHp--;
                fDmg--;
                if (fHp <= 0)
                {
                    DataMng.ins.nCurScore += 35;
                    CameraMng.ins.Shake(0.15f, 0.6f);
                    ObjectMng.ins.CreateGem(transform.position, 1);
                    Dead();
                    return;
                }
                else
                {
                    HitAni();
                    fSpeed *= 0.9f;
                    transform.localScale *= 0.95f;
                    EffectMng.ins.CreateEffect(other.transform.position, "MeteoHit");
                }
            }
        }
    }

    public void CreateMeteo(Vector3 pos, float Rot, float Damage, float Speed, float Hp)
    {
        if (MyRigid == null)
        {
            MyRigid = GetComponent<Rigidbody>();
            MyRenderer = GetComponent<MeshRenderer>();
        }

        if(prevMat == null)
        {
            prevMat = MyRenderer.material;
        }

        fSpeed = Speed;
        fDmg = Damage;
        transform.position = pos;

        float xDir = Mathf.Cos(Rot * Mathf.Deg2Rad);
        float yDir = Mathf.Sin(Rot * Mathf.Deg2Rad);

        Dir.x = xDir;
        Dir.y = yDir;

        gameObject.SetActive(true);
        bUse = true;

        fHp = Hp;

        transform.localScale = new Vector3(5, 5, 5);

        StartCoroutine(Kill());
    }
}
