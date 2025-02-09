using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    bool bUse;
    int nGem;

    float fLifeTime;

    public bool GetDead()
    {
        return bUse;
    }

    void Dead()
    {
        ObjectMng.ins.Gemparticle.Play();
        DataMng.ins.nCurGem += nGem;
        gameObject.SetActive(false);
        bUse = false;
        SoundMng.ins.PlayEffect("Gem");
    }

    private void Update()
    {
        fLifeTime += Time.deltaTime * 0.5f;
        transform.position = Vector3.MoveTowards(transform.position, GameMng.ins.GemPoint.position, fLifeTime);

        float fAngle = Mathf.Atan2(GameMng.ins.GemPoint.position.y - transform.position.y,
                GameMng.ins.GemPoint.position.x - transform.position.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(-90 - Mathf.LerpAngle(0, fAngle, fLifeTime), -90, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Gem"))
        {
            Dead();
        }
    }

    public void CreateGem(Vector3 pos, int gem)
    {
        if(gem > 0)
        {
            fLifeTime = 0;
            bUse = true;
            gameObject.SetActive(true);

            nGem = gem;
            transform.position = pos;
        }
        else
        {
            bUse = false;
            gameObject.SetActive(false);
        }
    }
}
