using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetRot : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Sin(Time.time) * Mathf.Rad2Deg, 0));
        transform.Rotate(0, 35 * Time.deltaTime, 0);
    }
}
