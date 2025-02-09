using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerSkill : MonoBehaviour
{
    public ITEM ThisItem;

    public ParticleSystem psBarrier;
    public ParticleSystem psAttack;

    float fPrevAttack;
    float fPrevAttackTime;

    Coroutine AttackCo;
    Coroutine BarrierCo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            ItemUse();

        psAttack.transform.position = transform.position;
    }

    IEnumerator Attack()
    {
        fPrevAttack = GameMng.ins.Playerstat.fAttack;
        fPrevAttackTime = GameMng.ins.Playerattack.fOriginalLaunchTIme;
        GameMng.ins.Playerstat.fAttack *= 2f;
        GameMng.ins.Playerattack.fOriginalLaunchTIme *= 0.5f;
        SoundMng.ins.PlayEffect("AttackUp");

        if (psAttack.isPlaying)
            psAttack.time = 0;
        else
            psAttack.Play();

        yield return new WaitForSeconds(5);
        GameMng.ins.Playerstat.fAttack = fPrevAttack;
        GameMng.ins.Playerattack.fOriginalLaunchTIme = fPrevAttackTime;
    }

    IEnumerator Barrier()
    {
        GameMng.ins.Playerstat.bBarrier = true;
        SoundMng.ins.PlayEffect("ShieldOn");
        if (psBarrier.isPlaying)
            psBarrier.time = 0;
        else
            psBarrier.Play();

        yield return new WaitForSeconds(5.5f);
        SoundMng.ins.PlayEffect("ShieldOff");
        GameMng.ins.Playerstat.bBarrier = false;

    }

    void ATTACK()
    {
        if (AttackCo != null)
        {
            GameMng.ins.Playerstat.fAttack = fPrevAttack;
            StopCoroutine(AttackCo);
        }
        AttackCo = StartCoroutine(Attack());
    }

    void HP()
    {
        GameMng.ins.Playerstat.HpHeal((GameMng.ins.Playerstat.fMaxHp - GameMng.ins.Playerstat.fHp) * 0.4f);
    }

    void SHIELD()
    {
        if (BarrierCo != null)
            StopCoroutine(BarrierCo);

        BarrierCo = StartCoroutine(Barrier());
    }

    void SUPPORT()
    {
        StartCoroutine(DroneAni());
    }

    IEnumerator DroneAni()
    {
        AnimationMng.ins.play("DroneIntro", DirectorUpdateMode.GameTime);
        yield return new WaitForSeconds(2.4f);
        StartCoroutine(Drones(10));
    }

    IEnumerator Drones(int nCount)
    {
        ObjectMng.ins.CreateDrone();
        yield return new WaitForSeconds(0.6f);
        if(nCount > 0)
            StartCoroutine(Drones(nCount - 1));
    }

    public void ItemUse()
    {
        if(ThisItem == ITEM._NONE)
        {
            SoundMng.ins.PlayEffect("ItemNone");
            return;
        }

        if (ThisItem != ITEM._MAX)
        {
            UIMng.ins.Itemimg.gameObject.SetActive(false);
            SoundMng.ins.PlayEffect("itemUse");
            switch (ThisItem)
            {
                case ITEM.ATTACK:
                    ATTACK();
                    break;
                case ITEM.HP:
                    HP();
                    break;
                case ITEM.SHIELD:
                    SHIELD();
                    break;
                case ITEM.SUPPORT:
                    SUPPORT();
                    break;
            }
            ThisItem = ITEM._NONE;
        }
    }
}
