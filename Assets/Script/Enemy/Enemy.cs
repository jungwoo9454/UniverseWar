using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [Header("Enemy Info Object")]
    [SerializeField]
    protected EnemyInfo EnemyCopyInfo;

    [Header("Enemy Stat Info")]
    public bool bUse;

    public new string name;
    public float fMaxHp;
    [Range(0,100)]
    public float fItemRespawn;
    public int nScore;
    public float fSpeed;

    public Transform[] MainMuzzle;

    public bool bInCam;

    float fRotClamp;

    protected SkinnedMeshRenderer EnemySkinRendere;


    public void CreateEnemy(Vector3 pos)
    {
        if(MyRigid == null)
        {
            MyRigid = GetComponent<Rigidbody>();
            MyTrans = GetComponent<Transform>();
            MyRenderer = GetComponent<MeshRenderer>();
            if (MyRenderer == null)
            {
                EnemySkinRendere = GetComponent<SkinnedMeshRenderer>();
                prevMat = EnemySkinRendere.material;
            }
            else
            {
                prevMat = MyRenderer.material;
            }
        }


        name = EnemyCopyInfo.name;
        fMaxHp = EnemyCopyInfo.fMaxHp;
        fItemRespawn = EnemyCopyInfo.fItemRespawn;
        nScore = EnemyCopyInfo.nScore;
        fSpeed = EnemyCopyInfo.fSpeed;
        fAttack = EnemyCopyInfo.fAttack;

        if (name == "Blade")
            SoundMng.ins.PlayEffect("Blade");

        MyTrans.position = pos;

        bUse = true;
        gameObject.SetActive(true);

        fHp = fMaxHp;

        Create();
    }

    protected void EnemyRot(float amount, float rotspeed)
    {

        if (MyRigid.velocity.y == 0)
        {
            if (fRotClamp > 0)
            {
                fRotClamp -= rotspeed * Time.deltaTime;
                if (fRotClamp < 0)
                    fRotClamp = 0;
            }
            else if (fRotClamp < 0)
            {
                fRotClamp += rotspeed * Time.deltaTime;
                if (fRotClamp > 0)
                    fRotClamp = 0;
            }
        }


        fRotClamp += MyRigid.velocity.normalized.y * rotspeed * Time.deltaTime;

        fRotClamp = Mathf.Clamp(fRotClamp, -1f, 1f);
        MyTrans.rotation = Quaternion.Euler(new Vector3(fRotClamp * amount, MyTrans.eulerAngles.y, MyTrans.eulerAngles.z));
    }


    protected override IEnumerator HitMatertialChange()
    {
        yield return new WaitForSeconds(0.085f);

        if (MyRenderer != null)
            MyRenderer.material = prevMat;
        else
            EnemySkinRendere.material = prevMat;
    }

    public void Kill()
    {
        if (DotDmg != null)
            StopCoroutine(DotDmg);
        gameObject.SetActive(false);
        bUse = false;
        if (MyRenderer != null)
            MyRenderer.material = prevMat;
        else
            EnemySkinRendere.material = prevMat;
    }

    public override void Dead()
    {

        SoundMng.ins.PlayEffect("EnemyDead");
        CameraMng.ins.Shake(0.18f, 0.3f);
        EffectMng.ins.CreateEffect(transform.position, "Boom");
        ObjectMng.ins.CreateItem(transform.position, (ITEM)Random.Range(0, (int)ITEM._MAX), fItemRespawn);
        //TODO 점수 표시
        //UIMng.ins.CreatetextEffect(transform.position, Color.white, new Color(255, 0, 255), nScore, null, new Vector2(1.5f, 1.5f));
        DataMng.ins.nCurScore += nScore;

        ObjectMng.ins.CreateGem(transform.position, EnemyCopyInfo.nGem);
        Kill();
    }

    public override void HitAni()
    {
        if (MyRenderer != null)
        {
            MyRenderer.material = RenderingMng.ins.HitMat;
        }
        else
        {
            EnemySkinRendere.material = RenderingMng.ins.HitMat;
        }
        if (hitAniCo != null)
            StopCoroutine(hitAniCo);
        hitAniCo = StartCoroutine(HitMatertialChange());
    }

    public virtual void Create()
    {

    }

    private void OnBecameInvisible()
    {
        bInCam = false;
    }

    private void OnBecameVisible()
    {
        bInCam = true;
    }
}
