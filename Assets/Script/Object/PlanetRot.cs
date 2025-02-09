using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRot : MonoBehaviour
{
    public float fRevolveSpeed;
    public float fRotationSpeed;

    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.down, fRevolveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, fRotationSpeed * Time.deltaTime);
    }
}
