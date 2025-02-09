using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Bullet Create Info")]
    public Transform Muzzle;
    public ParticleSystem MuzzleFlash;
    public LineRenderer Laser;
    public ParticleSystem LaserPs;
    public ParticleSystem LaserHitPs;
    public AudioSource LaserHit;
    public AudioSource LaerFire;
    public float fBulletLaunchTime;
    public float fBulletSpeed;
    public float fCreateBullet;

    public Color LasertexColor;

    public float fOriginalLaunchTIme;

    //

    float fAddAngle = 0;
    float fAddAngle2 = 0;

    int currentYInx = 0;

    float fAddAngles;

    float fNextLaunchTIme;

    float fLaserPower;
    float fLaserHitTIme;

    //

    public bool bThisMain;
    bool bAttackBtnDown;

    int nCurWeapon;

    private void Start()
    {
        fCreateBullet = 0;

        fOriginalLaunchTIme = fBulletLaunchTime;
    }
    void Update()
    {
        BulletLaunch();

        switch(nCurWeapon)
        {
            case 0:
                fBulletLaunchTime = fOriginalLaunchTIme * 0.7f;
                break;
            case 1:
                fBulletLaunchTime = fOriginalLaunchTIme * 0.7f;
                break;
            case 2:
                fBulletLaunchTime = fOriginalLaunchTIme * 0.2f;
                break;
            case 3:
                fBulletLaunchTime = 1f;
                break;
            case 4:
                fBulletLaunchTime = 0;
                break;
            default:
                fBulletLaunchTime = fOriginalLaunchTIme;
                break;
        }
    }

    public void WeaponChange()
    {
        bThisMain = !bThisMain;
        SoundMng.ins.PlayEffect("WeaponChange");
    }

    public void BulletFire()
    {
        if (fCreateBullet >= fBulletLaunchTime && !GameMng.ins.bPlayerDead)
        {
            if (nCurWeapon != 4)
                MuzzleFlash.Play();

            fCreateBullet = 0;
            switch (nCurWeapon)
            {
                case 0:
                    SoundMng.ins.PlayEffect("BulletLaunch");
                    BulletMng.ins.CreateBullet(true, Muzzle.position, 0, 0, fBulletSpeed * 1.2f, GameMng.ins.Playerstat.fAttack * 1.15f, BULLETCOLOR.ORANGE);
                    break;

                case 1:
                    SoundMng.ins.PlayEffect("BulletLaunch");
                    Vector3 posOffset = new Vector3(0.5f, 0, 0);
                    BulletMng.ins.CreateBullet(true, Muzzle.position - posOffset, -10, 0, fBulletSpeed, GameMng.ins.Playerstat.fAttack * 0.9f, BULLETCOLOR.ORANGE);
                    BulletMng.ins.CreateBullet(true, Muzzle.position, 0, 0, fBulletSpeed, GameMng.ins.Playerstat.fAttack, BULLETCOLOR.ORANGE);
                    BulletMng.ins.CreateBullet(true, Muzzle.position + posOffset, 10, 0, fBulletSpeed, GameMng.ins.Playerstat.fAttack * 0.9f, BULLETCOLOR.ORANGE);
                    break;

                case 2:
                    if (fNextLaunchTIme > 0.8f)
                    {
                        fAddAngles += 3.0f;

                        BulletMng.ins.CreateBullet(true, Muzzle.position, fAddAngles, 0, fBulletSpeed, GameMng.ins.Playerstat.fAttack, BULLETCOLOR.ORANGE);
                        BulletMng.ins.CreateBullet(true, Muzzle.position, -fAddAngles, 0, fBulletSpeed, GameMng.ins.Playerstat.fAttack, BULLETCOLOR.ORANGE);

                        if (fAddAngles > 21f)
                        {
                            fNextLaunchTIme = 0;
                            fAddAngles = 0;
                        }
                    }
                    break;

                case 3:
                    SoundMng.ins.PlayEffect("BulletLaunch");
                    CameraMng.ins.Shake(0.3f, 1f);
                    int n = Random.Range(15, 25);
                    for (int i = 0; i < n; i++)
                        BulletMng.ins.CreateBullet(true, Muzzle.position, Random.Range(15, -15), 7, Random.Range(fBulletSpeed * 0.8F, fBulletSpeed), GameMng.ins.Playerstat.fAttack * 1.1f, BULLETCOLOR.ORANGE);
                    break;

                case 4:
                    Laser.gameObject.SetActive(true);
                    Ray ray = new Ray(Laser.transform.position, Vector3.right);
                    RaycastHit hit;
                    if (!LaerFire.isPlaying)
                        LaerFire.Play();

                    if (Physics.Raycast(ray, out hit, 100, GameMng.ins.EnemyMask.value))
                    {

                        if (!LaserPs.isPlaying)
                            LaserPs.Play();
                        if (!LaserHit.isPlaying)
                            LaserHit.Play();
                        if (!LaserHitPs.isPlaying)
                            LaserHitPs.Play();

                        Laser.SetPosition(0, new Vector3(0, 0, 0));
                        Laser.SetPosition(1, new Vector3(hit.distance + 4.3f, 0, 0));
                        LaserHitPs.transform.position = hit.point;

                        fLaserPower += Time.deltaTime * 0.4f;
                        fLaserHitTIme += Time.deltaTime;

                        if (fLaserHitTIme >= 0.12f)
                        {
                            fLaserHitTIme = 0;
                            float dmg = ((GameMng.ins.Playerstat.fAttack * 0.3f) * (fLaserPower + 1));
                            hit.transform.GetComponent<Unit>().TakeDmg(dmg);
                        }
                    }
                    else
                    {
                        fLaserPower -= Time.deltaTime * 0.5f;
                        Laser.SetPosition(0, new Vector3(0, 0, 0));
                        Laser.SetPosition(1, new Vector3(500, 0, 0));
                        fLaserHitTIme = 0;

                        LaserHitPs.Stop();
                        LaserHit.Stop();
                    }
                    break;
            }
        }
    }

    void BulletLaunch()
    {
        Laser.gameObject.SetActive(false);
        fCreateBullet += Time.deltaTime;
        fNextLaunchTIme += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Tab))
            WeaponChange();

        if (bThisMain)
            nCurWeapon = DataMng.ins.nMainWeapon;
        else
            nCurWeapon = DataMng.ins.nSubWeapon;

        if(DataMng.ins.bMobile && bAttackBtnDown)
        {
            BulletFire();
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
                BulletFire();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            fLaserPower = 0;
            fLaserHitTIme = 0;
            LaerFire.Stop();
            LaserHitPs.Stop();
            LaserHit.Stop();
        }

        Laser.startWidth = (fLaserPower * 1.4f) + 0.7f;
        Laser.endWidth = (fLaserPower * 1.4f) + 0.45f;
        fLaserPower = Mathf.Clamp(fLaserPower, 0, 1.5f);
    }

    public void AttackOn() { bAttackBtnDown = true; }
    public void AttackOff()
    {
        bAttackBtnDown = false;
        fLaserPower = 0;
        fLaserHitTIme = 0;
        LaerFire.Stop();
        LaserHitPs.Stop();
        LaserHit.Stop();
    }
}
