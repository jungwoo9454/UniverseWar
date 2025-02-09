using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Airplane", menuName = "Game/Airplane")]
public class AirPlaneInfo : ScriptableObject
{
    public Texture texture;
    public Material material;
    public GameObject Airplane;
    public Mesh mesh;
    public new string name;
    public float fHp;
    public float fMoveSpeed;
    public float fAttack;
    public float fBulletLaunchTime;

    public int nMainWeapon;
    public int nSubWeapon;

    public int nPrice;
    public bool bOwn;
}
