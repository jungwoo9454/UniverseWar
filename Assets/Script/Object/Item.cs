using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ITEM { HP,ATTACK,SHIELD,SUPPORT,_MAX,_NONE}

public class Item : MonoBehaviour
{
    public ParticleSystem CircleWave;
    Rigidbody MyRigid;
    bool bUse;
    bool bTimeOver;
    ITEM WhatItem;

    private void FixedUpdate()
    {
        if(!bTimeOver)
            MyRigid.velocity = new Vector3(-1, 0, 0);

        if (transform.position.y <= GameMng.ins.Down.position.y - 10)
            Kill();
    }

    public bool GetDead()
    {
        return bUse;
    }

    void Dead()
    {
        Kill();
        GameMng.ins.PlayerSkill.ThisItem = WhatItem;
        EffectMng.ins.CreateEffect(transform.position, "ItemEat");
        SoundMng.ins.PlayEffect("ItemEat");
        UIMng.ins.Itemimg.gameObject.SetActive(true);
        UIMng.ins.Itemimg.sprite = UIMng.ins.ItemIcons[(int)GameMng.ins.PlayerSkill.ThisItem];
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        bUse = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !bTimeOver)
        {
            if(GameMng.ins.PlayerSkill.ThisItem == ITEM._NONE)
            {
                Dead();
            }
        }
    }

    IEnumerator OverLifeTime()
    {
        yield return new WaitForSeconds(10f);
        MyRigid.useGravity = true;
        bTimeOver = true;
        MyRigid.AddForce(new Vector3(0, -4, 0));
    }

    public void CreateItem(Vector3 pos, ITEM eitem)
    {
        if(MyRigid == null)
        {
            MyRigid = GetComponent<Rigidbody>();
        }

        gameObject.SetActive(true);
        bUse = true;

        MyRigid.useGravity = false;
        bTimeOver = false;

        transform.position = pos;
        WhatItem = eitem;

        transform.GetChild(0).GetComponent<MeshRenderer>().material = GameMng.ins.ItemMat[(int)eitem];

        CircleWave.Play();

        StartCoroutine(OverLifeTime());
    }
}
