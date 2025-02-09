using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundMng : MonoBehaviour
{
    static SoundMng instance = null;
    AudioSource backgroundAudio;
    AudioSource effectAudio;
    Dictionary<string, AudioClip> backgrounds;
    Dictionary<string, AudioClip> effects;

    public static SoundMng ins
    {
        get
        {
            if (instance == null)
            {
                GameObject soundObject = new GameObject("SoundManager");

                instance = soundObject.AddComponent<SoundMng>();
                instance.backgroundAudio = soundObject.AddComponent<AudioSource>();
                instance.effectAudio = soundObject.AddComponent<AudioSource>();
                instance.LoadFile(ref instance.effects, "Sound/Effect/");
                instance.LoadFile(ref instance.backgrounds, "Sound/Background/");
                DontDestroyOnLoad(soundObject);
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

    private void LateUpdate()
    {
        SetBackgroundVolume(DataMng.ins.fMusicVol);
        SetEffectVolume(DataMng.ins.fVFXVol);
    }

    public bool IsPlayBackGround()
    {
        return backgroundAudio.isPlaying;
    }

    public void PlayEffect(string name)
    {
        effectAudio.PlayOneShot(effects[name]);
    }
    public void SetEffectVolume(float scale)
    {
        effectAudio.volume = scale;
    }
    public void PlayBackground(string name)
    {
        backgroundAudio.Stop();
        backgroundAudio.loop = true;
        backgroundAudio.clip = backgrounds[name];
        backgroundAudio.Play();
    }
    public void StopBackground()
    {
        backgroundAudio.Stop();
    }
    public void SetBackgroundVolume(float scale)
    {
        backgroundAudio.volume = scale;
    }

    public void backGroundPause()
    {
        backgroundAudio.Pause();
    }

    public void EffectPause()
    {
        effectAudio.Pause();
    }

    public void backGroundUnPause()
    {
        backgroundAudio.UnPause();
    }

    public void EffectUnPause()
    {
        effectAudio.UnPause();
    }
}