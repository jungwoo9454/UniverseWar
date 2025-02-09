using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int nGem;
    public int[] nHighScores;
}
