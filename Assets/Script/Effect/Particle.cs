using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public ParticleSystem ps;
    public bool bUse;

    private void Update()
    {
        if(!ps.isPlaying)
        {
            bUse = false;
            gameObject.SetActive(false);
        }
    }

    public void CreateParticle(Vector3 pos)
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();

        transform.position = pos;
        bUse = true;
        gameObject.SetActive(true);

        ps.Play();
    }
}
