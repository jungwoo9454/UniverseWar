using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BULLETCOLOR { RED,ORANGE,YELLOW,GREEN,SKY,GRAY,WHITE}

public class Bullet : MonoBehaviour
{
    [Header("Bullet Pattern And Stat")]
    public int nPattern;
    public float fSpeed;
    public bool bPlayer;
    public bool bGod;
    float fTotalLifeTime;

    float fNoiseTime;
    float fAttack;

    Transform BulletTrans;
    Rigidbody BulletRigid;
    MeshRenderer BulletRenderer;

    Vector3 Dir;

    bool bUse;
    float fAngle;
    //Vector3 vAngle;

    BULLETCOLOR bulletcolor;


    private void Update()
    {
        if (bUse)
        {
            fTotalLifeTime += Time.deltaTime;
            Patterns();
        }
    }

    private void FixedUpdate()
    {
        BulletRigid.velocity = Dir;
        if(!bGod)
            BulletTrans.rotation = Quaternion.Euler(new Vector3(0, 0, fAngle - 90));

        if (transform.position.x >= GameMng.ins.Right.position.x + 5)
        {
            Kill();
        }
    }

    public bool GetDead()
    {
        return bUse;
    }

    public void Kill()
    {
        EffectMng.ins.CreateEffect(new Vector3(BulletTrans.position.x, BulletTrans.position.y, -10), GetColorString(bulletcolor) + "BulletDead");
        bUse = false;
        gameObject.SetActive(false);
    }

    public void Dead()
    {
        switch (nPattern)
        {
            case 8:
                BulletMng.ins.CreateBullet(bPlayer, transform.position, 45, 0, fSpeed * 1.1f, fAttack * 0.3f, BULLETCOLOR.YELLOW);
                BulletMng.ins.CreateBullet(bPlayer, transform.position, 135, 0, fSpeed * 1.1f, fAttack * 0.3f, BULLETCOLOR.YELLOW);
                BulletMng.ins.CreateBullet(bPlayer, transform.position, 220, 0, fSpeed * 1.1f, fAttack * 0.3f, BULLETCOLOR.YELLOW);
                BulletMng.ins.CreateBullet(bPlayer, transform.position, 315, 0, fSpeed * 1.1f, fAttack * 0.3f, BULLETCOLOR.YELLOW);
                break;
        }

        Kill();
    }

    public string GetColorString(BULLETCOLOR bULLETCOLOR)
    {
        switch (bULLETCOLOR)
        {
            case BULLETCOLOR.GRAY:
                return "Gray";

            case BULLETCOLOR.GREEN:
                return "Green";

            case BULLETCOLOR.ORANGE:
                return "Orange";

            case BULLETCOLOR.RED:
                return "Red";

            case BULLETCOLOR.SKY:
                return "Sky";

            case BULLETCOLOR.WHITE:
                return "White";

            case BULLETCOLOR.YELLOW:
                return "Yellow";
        }

        return "";
    }

    void Patterns()
    {
        switch (nPattern)
        {
            case 3:
                Dir = new Vector3(1.1f, Mathf.Cos(fTotalLifeTime * 3) * 0.6f, 0) * fSpeed;
                BulletRigid.velocity = Dir;
                break;
            case 4:
                Dir = new Vector3(1.1f, -Mathf.Cos(fTotalLifeTime * 3) * 0.6f, 0) * fSpeed;
                BulletRigid.velocity = Dir;
                break;
            case 5:
                fAngle = Mathf.Atan2(GameMng.ins.PlayerTrans.position.y - BulletTrans.position.y,
                                GameMng.ins.PlayerTrans.position.x - BulletTrans.position.x) * Mathf.Rad2Deg;
                Dir = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0) * fSpeed;
                break;
            case 6:
                fAngle = Mathf.Atan2(GameMng.ins.PlayerTrans.position.y - BulletTrans.position.y,
                                GameMng.ins.PlayerTrans.position.x - BulletTrans.position.x) * Mathf.Rad2Deg;
                Dir = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0) * fSpeed;
                break;
            case 9:
                if(fTotalLifeTime >= 0.85f)
                {
                    fNoiseTime += Time.deltaTime;
                    if (fNoiseTime >= 0.35f)
                    {
                        fNoiseTime = 0;
                        Vector3 vRandom = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
                        Dir = vRandom.normalized * fSpeed;
                    }
                }
                break;
        }


    }

    IEnumerator LifeTimeOver(float liftime)
    {
        yield return new WaitForSeconds(liftime);
        Dead();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!bGod)
        {
            if (bPlayer)
            {
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<Enemy>().TakeDmg(fAttack);
                    Kill();
                }
            }
            else
            {
                if (other.CompareTag("Player"))
                {
                    other.GetComponent<PlayerStat>().TakeDmg(fAttack);
                    Kill();
                }
            }
        }
    }

    public void CreateBullet(bool player, Vector3 position, float fangle, int Pattern, float Speed, float Attack, float fLifeTime ,BULLETCOLOR color)
    {
        if (BulletTrans == null)
        {
            BulletTrans = GetComponent<Transform>();
            BulletRigid = GetComponent<Rigidbody>();
            BulletRenderer = GetComponent<MeshRenderer>();
        }

        fTotalLifeTime = 0;

        nPattern = Pattern;
        fSpeed = Speed;

        fAngle = fangle;
        BulletTrans.position = position;

        float xDir = Mathf.Cos(fAngle * Mathf.Deg2Rad);
        float yDir = Mathf.Sin(fAngle * Mathf.Deg2Rad);

        Dir = new Vector3(xDir, yDir, 0) * fSpeed;

        bGod = false;


        bPlayer = player;
        bUse = true;

        fAttack = Attack;

        gameObject.SetActive(true);

        SetColor(color);

        BulletTrans.rotation = Quaternion.Euler(new Vector3(0, 0, fAngle - 90));

        StartCoroutine(LifeTimeOver(fLifeTime));
    }

    public void GodBullet(Vector3 position, Vector3 vangle, Vector3 direction, float Speed, float fLifeTime, BULLETCOLOR color)
    {
        if (BulletTrans == null)
        {
            BulletTrans = GetComponent<Transform>();
            BulletRigid = GetComponent<Rigidbody>();
            BulletRenderer = GetComponent<MeshRenderer>();
        }

        fTotalLifeTime = 0;

        nPattern = 0;
        fSpeed = Speed;

        BulletTrans.position = position;

        transform.Rotate(new Vector3(vangle.x, vangle.y, vangle.z+90));

        Dir = direction * fSpeed;

        //Vector3 vAngle = Vector3.zero;
        //vAngle = transform.rotation * Vector3.right;

         //Vector3 vAngle = vangle.normalized;
         //Debug.Log(vangle + "/" + vangle.normalized);

         //transform.rotation = Quaternion.Euler(vangle);

         //Dir = vAngle * fSpeed;

         bPlayer = false;
        bGod = true;
        bUse = true;

        fAttack = 0;

        gameObject.SetActive(true);

        SetColor(color);

        StartCoroutine(LifeTimeOver(fLifeTime));
    }

    void ColorSet()
    {
        if(BulletRenderer != null)
        {
            switch (bulletcolor)
            {
                case BULLETCOLOR.GRAY:
                    BulletRenderer.material = RenderingMng.ins.grayBullet;
                    break;
                case BULLETCOLOR.GREEN:
                    BulletRenderer.material = RenderingMng.ins.GreenBullet;
                    break;
                case BULLETCOLOR.ORANGE:
                    BulletRenderer.material = RenderingMng.ins.OrangeBullet;
                    break;
                case BULLETCOLOR.RED:
                    BulletRenderer.material = RenderingMng.ins.RedBullet;
                    break;
                case BULLETCOLOR.SKY:
                    BulletRenderer.material = RenderingMng.ins.SkyBullet;
                    break;
                case BULLETCOLOR.WHITE:
                    BulletRenderer.material = RenderingMng.ins.WhiteBullet;
                    break;
                case BULLETCOLOR.YELLOW:
                    BulletRenderer.material = RenderingMng.ins.YellowBullet;
                    break;
            }
        }
    }

    public void SetColor(BULLETCOLOR color)
    {
        bulletcolor = color;
        ColorSet();
    }
}
