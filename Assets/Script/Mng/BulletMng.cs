using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 총알 목록
/// 0.방향탄(정해진 방향으로 감)
/// 1.n-Way탄 n개의 총알이 부채꼴 모양으로 나감(현재는 3개)
/// 2.원형탄
/// 3.Cos파를 이용한 총알 움직임
/// 4.-Cos파를 이용한 총알 움직임
/// 5.유도탄(끝까지 추격하는)
/// 6.유도탄(피하기 쉬운)
/// 7.랜덤탄(다양한 방향으로 나가는 탄)
/// 8.분열탄
/// 9.발사 할 때마다 각도가 바뀜 4개의 방향으로 발사함 
/// 10.발사할 때 마다 각도가 바뀌는 원형탄
/// 11.
/// </summary>

public class BulletMng : MonoBehaviour
{
    [Header("Bullet Parent Object")]
    public GameObject BulletOriginal;
    public GameObject ElectroBall;
    public GameObject SphereBullet;
    List<Bullet> BulletList = new List<Bullet>();


    private static BulletMng bulletMng;
    public static BulletMng ins
    {
        get
        {
            if (bulletMng == null)
            {
                bulletMng = FindObjectOfType<BulletMng>();

                if (bulletMng == null)
                {
                    GameObject bulletObj = new GameObject();
                    bulletObj.name = "BulletMng";
                    bulletMng = bulletObj.AddComponent<BulletMng>();
                }
            }
            return bulletMng;
        }
    }

    public void CreateElectroBall(bool player, Vector3 position, float fangle, int Pattern, float Speed, float Attack, BULLETCOLOR color = BULLETCOLOR.WHITE, float fLifeTime = 5)
    {
        bool findBullet = false;
        for (int i = 0; i < BulletList.Count; i++)
        {
            if (!BulletList[i].GetDead() && BulletList[i].name == "ElectroBall(Clone)")
            {
                BulletList[i].CreateBullet(player, position, fangle, Pattern, Speed, Attack, fLifeTime, color);
                BulletList[i].GetComponent<ParticleSystem>().Play();
;               findBullet = true;
                break;
            }
        }

        if (!findBullet)
        {
            Bullet playerBullet;
            playerBullet = Instantiate(ElectroBall).GetComponent<Bullet>();
            playerBullet.transform.parent = transform;
            playerBullet.CreateBullet(player, position, fangle, Pattern, Speed, Attack, fLifeTime, color);
            playerBullet.GetComponent<ParticleSystem>().Play();
            BulletList.Add(playerBullet);
        }
    }

    public void CreateBullet(bool player, Vector3 position, float fangle, int Pattern, float Speed, float Attack, BULLETCOLOR color = BULLETCOLOR.WHITE, float fLifeTime = 5, string BulletName = "Normal")
    {
        bool findBullet = false;
        for (int i = 0; i < BulletList.Count; i++)
        {
            if (!BulletList[i].GetDead() && BulletList[i].name == BulletName + "(Clone)")
            {
                BulletList[i].CreateBullet(player,position, fangle, Pattern, Speed, Attack, fLifeTime, color);
                findBullet = true;
                break;
            }
        }

        if (!findBullet)
        {
            Bullet playerBullet;
            if(BulletName == "Sphere")
                playerBullet = Instantiate(SphereBullet).GetComponent<Bullet>();
            else
                playerBullet = Instantiate(BulletOriginal).GetComponent<Bullet>();
            playerBullet.transform.parent = transform;
            playerBullet.CreateBullet(player, position, fangle, Pattern, Speed, Attack, fLifeTime, color);
            BulletList.Add(playerBullet);
        }
    }

    public void GodBullet(Vector3 position, Vector3 vangle, Vector3 direction, float Speed, float fLifeTime, BULLETCOLOR color)
    {
        bool findBullet = false;
        for (int i = 0; i < BulletList.Count; i++)
        {
            if (!BulletList[i].GetDead() && BulletList[i].name == "Normal(Clone)")
            {
                BulletList[i].GodBullet(position, vangle, direction, Speed, fLifeTime, color);
                findBullet = true;
                break;
            }
        }

        if (!findBullet)
        {
            Bullet playerBullet;
            playerBullet = Instantiate(BulletOriginal).GetComponent<Bullet>();
            playerBullet.transform.parent = transform;
            playerBullet.GodBullet(position, vangle, direction, Speed, fLifeTime, color);
            BulletList.Add(playerBullet);
        }
    }
}
