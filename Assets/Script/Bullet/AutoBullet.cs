using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBullet : MonoBehaviour
{
    public Transform Muzzle;
    public float fBulletLaunchTime;
    public float fSpeed;
    public BULLETCOLOR bulletcolor;
    float fFireTime;

    private void Start()
    {
        if(Muzzle == null)
        {
            Muzzle.position = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        fFireTime += Time.deltaTime;
        if (fBulletLaunchTime <= fFireTime)
        {
            fFireTime = 0;
            BulletMng.ins.GodBullet(Muzzle.position, Muzzle.localEulerAngles, -Muzzle.forward, fSpeed, 5, bulletcolor);
            
        }
    }
}
