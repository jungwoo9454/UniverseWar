﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    void Update()
    {
        transform.position = GameMng.ins.PlayerTrans.position;
    }
}
