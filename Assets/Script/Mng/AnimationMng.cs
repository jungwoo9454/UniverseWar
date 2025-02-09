using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AnimationMng : MonoBehaviour
{
    static AnimationMng instance;
    public PlayableDirector AnimationMngPD;
    Dictionary<string,TimelineAsset> playable;

    private void Start()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AnimationMng>();

            if (instance == null)
            {
                GameObject soundObject = new GameObject("AnimationMng");

                instance = soundObject.AddComponent<AnimationMng>();
            }

            instance.LoadFile(ref instance.playable, "TimeLine/");
        }
    }

    public static AnimationMng ins
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AnimationMng>();

                if (instance == null)
                {
                    GameObject soundObject = new GameObject("AnimationMng");

                    instance = soundObject.AddComponent<AnimationMng>();
                    //DontDestroyOnLoad(soundObject);
                }

                instance.LoadFile(ref instance.playable, "TimeLine/");
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

    public void play(string name, DirectorUpdateMode mode)
    {
        AnimationMngPD.Play(playable[name]);
        AnimationMngPD.timeUpdateMode = mode;
    }

    public bool GetAniState(Animator ani, string name)
    {
        return ani.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
