using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Unit
{
    [Header("Player Stat")]
    public float fMaxHp;
    public float fUpgrade;
    public bool bBarrier;
    public bool bPowerUp;
    public bool bGod;

    public ParticleSystem psHeal;

    private void Start()
    {
        MyRenderer = GetComponent<MeshRenderer>();
        fHp = fMaxHp;
    }

    public override void TakeDmg(float dmg)
    {
        if(!bBarrier)
        {
            if(!bGod)
                fHp -= dmg;
            UIMng.ins.CreatetextEffect(transform.position, Color.white, Color.red, dmg, null, new Vector2(1, 1));
            HitAni();

            if (fHp <= 0 && !GameMng.ins.bPlayerDead)
            {
                GameMng.ins.bPlayerDead = true;
                StageMng.ins.bStagePause = true;
                StageMng.ins.GameOver();
                UIMng.ins.ResultUI();
            }
        }
        else
        {
            UIMng.ins.CreatetextEffect(transform.position, Color.blue, new Color(255, 255, 255), 0, "방어", new Vector2(1.2f, 1.2f));
        }

        fHp = Mathf.Clamp(fHp, 0, fMaxHp);
    }

    public void HpHeal(float value)
    {
        fHp += value;

        fHp = Mathf.Clamp(fHp, 0, fMaxHp);

        EffectMng.ins.CreateEffect(transform.position, "Heal");
    }
}
