using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Player Move Value")]
    public float fMoveSpeed;

    public float fRotationAmount;
    public float fRotSpeed;
    public bool bButtonDown;
    float fRotClamp;
    bool bMove;

    //Player Componenet
    Transform PlayerTrans;
    Rigidbody PlayerRigid;
    public ParticleSystem[] Flames = new ParticleSystem[2];
    public GameObject Electro;

    Coroutine Electroco;

    ////////////////////////////
    
    void Start()
    {
        bMove = true;

        PlayerTrans = GetComponent<Transform>();
        PlayerRigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Electro.transform.position = transform.position;
        Move();
        MoveRotation();
        Clipping();
    }

    void Clipping()
    {
        Vector3 vP = PlayerTrans.position;

        vP.x = Mathf.Clamp(vP.x, -57, 57);
        vP.y = Mathf.Clamp(vP.y, -30, 33);

        PlayerTrans.position = vP;
    }

    void Move()
    {
        if(bMove && !GameMng.ins.bPlayerDead && !DataMng.ins.bMobile)
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");

            PlayerRigid.velocity = new Vector3(hor, ver, 0) * fMoveSpeed;

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                if (!Flames[0].isPlaying || !Flames[1].isPlaying)
                {
                    Flames[0].Play();
                    Flames[1].Play();
                }
            }
            else
            {
                if (Flames[0].isPlaying || Flames[1].isPlaying)
                {
                    Flames[0].Stop();
                    Flames[1].Stop();
                }
            }
        }
    }

    public void Move(Vector3 v)
    {
        float rotSpeed = Time.deltaTime * fRotSpeed;

        PlayerRigid.velocity = v * fMoveSpeed;

        if(v.y > 0)
            fRotClamp += rotSpeed;
        else if(v.y < 0)
            fRotClamp -= rotSpeed;

        if (v.x != 0 || v.y != 0)
        {
            if (!Flames[0].isPlaying || !Flames[1].isPlaying)
            {
                Flames[0].Play();
                Flames[1].Play();
            }
        }
        else
        {
            if (Flames[0].isPlaying || Flames[1].isPlaying)
            {
                Flames[0].Stop();
                Flames[1].Stop();
            }
        }
    }

    void MoveRotation()
    {
        float rotSpeed = Time.deltaTime * fRotSpeed;

        if(!DataMng.ins.bMobile)
        {
            bButtonDown = true;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                fRotClamp += rotSpeed;
                bButtonDown = false;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                fRotClamp -= rotSpeed;
                bButtonDown = false;
            }

        }

        if (bButtonDown)
        {
            if (fRotClamp > 0)
            {
                if (0 < fRotClamp - rotSpeed)
                    fRotClamp -= rotSpeed;
                else
                    fRotClamp = 0;
            }
            else if (fRotClamp < 0)
            {
                if (0 > fRotClamp - rotSpeed)
                    fRotClamp += rotSpeed;
                else
                    fRotClamp = 0;
            }
        }

        fRotClamp = Mathf.Clamp(fRotClamp, -1, 1);
        PlayerTrans.rotation = Quaternion.Euler(new Vector3(fRotClamp * fRotationAmount, PlayerTrans.eulerAngles.y, PlayerTrans.eulerAngles.z));
    }

    IEnumerator ElectroBall()
    {
        SoundMng.ins.PlayEffect("PlayerElec");
        CameraMng.ins.Shake(1.5f, 0.65f);
        GameMng.ins.Playerstat.TakeDotDmg(5, 0.3f, 5);
        Electro.SetActive(true);
        bMove = false;
        PlayerRigid.velocity = Vector3.zero;
        yield return new WaitForSeconds(1.5f);
        Electro.SetActive(false);
        bMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet") && other.name == "ElectroBall(Clone)")
        {
            if (Electroco == null)
                Electroco = StartCoroutine(ElectroBall());
            else
            {
                StopCoroutine(Electroco);
                Electroco = StartCoroutine(ElectroBall());
            }
        }
    }
}
