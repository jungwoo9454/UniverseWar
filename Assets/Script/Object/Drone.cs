using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    Rigidbody MyRigid;
    SkinnedMeshRenderer MyRenderer;
    Enemy Target;
    Vector3 Dir;
    float fAngle;
    float fRotClamp;
    float rotSpeed = 14;

    bool bUse;
    bool bDead;


    private void Update()
    {
        if (transform.position.x > 80)
        {
            Dead();
        }

        if (!bDead)
        {
            Target = EnemyMng.ins.DistanceShortEnemy(transform.position);
            if (Target == null)
            {
                bDead = true;
            }
            else
            {
                if(!Target.gameObject.activeSelf)
                {
                    bDead = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if(!bDead)
        {
            if (Target != null)
            {
                fAngle = Mathf.Atan2(Target.transform.position.y - transform.position.y, Target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                Dir = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0) * 125;
                MyRigid.velocity = Dir;
                DroneRot();
            }
            else
            {
                Target = EnemyMng.ins.DistanceShortEnemy(transform.position);
                if (Target == null)
                    bDead = true;
            }
        }
        else
        {
            MyRigid.velocity = new Vector3(1,0,0) * 125;
        }
    }

    public void Dead()
    {
        bUse = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && !bDead)
        {
            if(other.GetComponent<Enemy>().bInCam)
            {
                Dead();
                other.GetComponent<Enemy>().Dead();
                CameraMng.ins.Shake(0.25f, 0.7f);
            }
        }
    }

    public void CreateDrone()
    {
        if(MyRigid == null)
        {
            MyRigid = GetComponent<Rigidbody>();
            MyRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        transform.position = new Vector3(-80, 0, 0);

        Target = EnemyMng.ins.DistanceShortEnemy(transform.position);

        bUse = true;
        gameObject.SetActive(true);
        bDead = false;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    void DroneRot()
    {
        if (MyRigid.velocity.y == 0)
        {
            if (fRotClamp > 0)
            {
                fRotClamp -= rotSpeed * Time.deltaTime;
                if (fRotClamp < 0)
                    fRotClamp = 0;
            }
            else if (fRotClamp < 0)
            {
                fRotClamp += rotSpeed * Time.deltaTime;
                if (fRotClamp > 0)
                    fRotClamp = 0;
            }
        }
        fRotClamp += MyRigid.velocity.normalized.y * rotSpeed * Time.deltaTime;

        fRotClamp = Mathf.Clamp(fRotClamp, -1f, 1f);
        transform.rotation = Quaternion.Euler(new Vector3(fRotClamp * 45, transform.eulerAngles.y, transform.eulerAngles.z));
    }

    public bool GetDead()
    {
        return bUse;
    }
}
