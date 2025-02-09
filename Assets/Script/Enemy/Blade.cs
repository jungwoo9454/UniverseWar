using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : Enemy
{
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameMng.ins.PlayerTrans.position, 
            (Vector3.Distance(transform.position, GameMng.ins.PlayerTrans.position) * 0.08f) * fSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStat>().TakeDmg(fAttack);
            CameraMng.ins.Shake(0.25f, 0.85f);
            EffectMng.ins.MeteoHit();
            SoundMng.ins.PlayEffect("MeteoHit");
        }
    }
}
