using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSationRot : MonoBehaviour
{
    public Vector3 vSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(vSpeed * Time.deltaTime);
    }
}
