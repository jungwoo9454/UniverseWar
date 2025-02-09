using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Game/Enemy")]
public class EnemyInfo : ScriptableObject
{
    public new string name;
    public float fMaxHp;
    public float fSpeed;
    public float fAttack;

    [Range(0.0f, 1.0f)]
    public float fItemRespawn;
    public int nGem;
    public int nScore;
}
