using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectMng : MonoBehaviour
{
    static EffectMng instance = null;
    Dictionary<string, Particle> effects;

    List<Particle> particleList = new List<Particle>();

    public static EffectMng ins
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectMng>();
                if (instance == null)
                {
                    GameObject effectobj = new GameObject("EffectMng");

                    instance = effectobj.AddComponent<EffectMng>();
                    instance.LoadFile(ref instance.effects, "Effect/Ps/");
                    DontDestroyOnLoad(effectobj);
                }
            }
            return instance;
        }
    }

    private void LoadFile<T>(ref Dictionary<string, T> a, string path) where T : Object
    {
        a = new Dictionary<string, T>();
        T[] particleSystems = Resources.LoadAll<T>(path);
        foreach (var particle in particleSystems)
        {
            a.Add(particle.name, particle);
        }
    }

    public void CreateEffect(Vector3 pos, string path)
    {
        if (effects[path] == null)
            return;

        for (int i = 0; i < particleList.Count; i++)
        {
            if (particleList[i].name == path+"(Clone)" && !particleList[i].bUse)
            {
                particleList[i].CreateParticle(pos);
                return;
            }
        }

        Particle obj;
        obj = Instantiate(effects[path]).GetComponent<Particle>();
        obj.transform.parent = transform;
        obj.CreateParticle(pos);
        particleList.Add(obj);
    }

    public void MeteoHit()
    {
        RenderingMng.ins.vignette.active = true;
        RenderingMng.ins.vignette.intensity.value = 0;
        StartCoroutine(MeteoHitAddCo(12));
    }

    IEnumerator MeteoHitAddCo(int n)
    {
        RenderingMng.ins.vignette.intensity.value += Time.deltaTime * 2.5f;
        yield return new WaitForSeconds(Time.deltaTime);
        if (n > 0)
            StartCoroutine(MeteoHitAddCo(n - 1));
        else
            StartCoroutine(MeteoHitMinCo(12));
    }

    IEnumerator MeteoHitMinCo(int n)
    {
        RenderingMng.ins.vignette.intensity.value -= Time.deltaTime * 2.5f;
        yield return new WaitForSeconds(Time.deltaTime);
        if (n > 0)
            StartCoroutine(MeteoHitMinCo(n - 1));
        else
            RenderingMng.ins.vignette.active = false;
    }
}
