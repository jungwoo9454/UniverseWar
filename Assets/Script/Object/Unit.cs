using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float fHp;
    public float fAttack;
    protected Rigidbody MyRigid;
    protected Transform MyTrans;
    protected MeshRenderer MyRenderer;

    public Material prevMat;
    protected Coroutine hitAniCo;
    protected Coroutine DotDmg;

    public virtual void Dead()
    {

    }

    public virtual void TakeDmg(float dmg)
    {
        fHp -= dmg;
        UIMng.ins.CreatetextEffect(transform.position, Color.white, Color.red, dmg, null, new Vector2(1, 1));
        HitAni();

        if (fHp <= 0)
        {
            Dead();
        }
    }

    public virtual void HitAni()
    {
        if (MyRenderer != null)
        {
            MyRenderer.material = RenderingMng.ins.HitMat;
        }

        if (hitAniCo != null)
            StopCoroutine(hitAniCo);
        hitAniCo = StartCoroutine(HitMatertialChange());
    }

    public void TakeDotDmg(int takeAmount, float time, float attack)
    {
        StartCoroutine(TakeDotDmgCour(takeAmount, time, attack));
    }

    protected IEnumerator TakeDotDmgCour(int takeAmount, float time, float attack)
    {
        yield return new WaitForSeconds(time);
        takeAmount--;
        TakeDmg(attack);

        if (takeAmount > 0)
            DotDmg = StartCoroutine(TakeDotDmgCour(takeAmount, time, attack));
    }

    protected virtual IEnumerator HitMatertialChange()
    {
        yield return new WaitForSeconds(0.085f);

        if (MyRenderer != null)
        {
            MyRenderer.material = prevMat;
        }
    }
}
