using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwipe : MonoBehaviour
{
    public int nNum;
    public GameObject scrollbar;
    public float scrollpos = 0;
    float distance;
    public float[] pos;
    Color[] colors;
    Image[] images;
    public int posis;


    public void Next()
    {
        //if(posis < pos.Length - 3)
        //{
        posis++;
        if (posis > pos.Length - 2)
        {
            posis = 1;
        }
        scrollpos = pos[posis];
        //}
    }

    public void Prev()
    {
        //if (posis > 1)
        //{
        //    posis--;
        //    scrollpos = pos[posis];
        //}

        posis--;
        if (posis < 1)
        {
            posis = pos.Length - 2;
        }

        scrollpos = pos[posis];
    }

    private void Awake()
    {
        pos = new float[transform.childCount];
        colors = new Color[pos.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = GetComponentsInChildren<Image>()[i].color;
        }

        images = GetComponentsInChildren<Image>();

        distance = 1f / (pos.Length - 3f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = (distance * i) - distance;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //if(Input.GetMouseButton(0))
        //{
        //    scrollpos = scrollbar.GetComponent<Scrollbar>().value;
        //}else
        //{
            for (int i = 0; i < pos.Length; i++)
            {
                images[i].color = new Color(colors[i].r, colors[i].g, colors[i].b, 0.6f);
                if (scrollpos < pos[i] + (distance / 2) && scrollpos > pos[i] - (distance / 2))
                {
                    images[i].color = colors[i];
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.15f);
                    //posis = i;
                }
            }
        //}


        switch (nNum)
        {
            case 0:
                DataMng.ins.nMainWeapon = posis;
                DataMng.ins.planeinfo[MenuUI.ins.nCurAirplane].nMainWeapon = posis;
                break;
            case 1:
                DataMng.ins.nSubWeapon = posis;
                DataMng.ins.planeinfo[MenuUI.ins.nCurAirplane].nSubWeapon = posis;
                break;
        }

    }

    public void SetPos(int nIndex)
    {
        if(pos.Length == 0)
        {
            pos = new float[transform.childCount];
            colors = new Color[pos.Length];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = GetComponentsInChildren<Image>()[i].color;
            }

            images = GetComponentsInChildren<Image>();

            distance = 1f / (pos.Length - 3f);
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = (distance * i) - distance;
            }
        }

        posis = nIndex;

        if (posis < 1)
        {
            posis = pos.Length - 2;
        }

        if (posis > pos.Length - 2)
        {
            posis = 1;
        }

        scrollpos = pos[posis];
    }
}
