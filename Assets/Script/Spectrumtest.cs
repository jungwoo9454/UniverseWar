using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spectrumtest : MonoBehaviour
{
    new AudioSource audio;
    public GameObject Bar;
    RectTransform[] Bars;

    public float[] samples = new float[64];

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        Bars = new RectTransform[Bar.transform.childCount];
        for (int i = 0; i < Bar.transform.childCount; i++)
        {
            Bars[i] = Bar.transform.GetChild(i).GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        audio.GetSpectrumData(samples, 0, FFTWindow.Triangle);
        for (int i = 0; i < Bars.Length; i++)
        {
            Bars[i].sizeDelta = new Vector2(5, samples[i] * 650);
        }
    }
}
