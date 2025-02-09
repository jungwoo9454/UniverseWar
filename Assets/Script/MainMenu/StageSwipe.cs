using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSwipe : MonoBehaviour
{
    public GameObject scrollbar;
    float scrollpos = 0;
    float distance;
    float[] pos;
    Color[] colors;
    Image[] images;
    int posis;

    public void Next()
    {
        if (posis < pos.Length - 1)
        {
            posis++;
            scrollpos = pos[posis];
        }
    }

    public void Prev()
    {
        if (posis > 0)
        {
            posis--;
            scrollpos = pos[posis];
        }
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



        for(int i=0;i<transform.childCount;i++)
        {
            transform.GetChild(i).GetComponent<Stage>().nStageNumber = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            posis += (int)Input.mouseScrollDelta.y;
            posis = Mathf.Clamp(posis, 0, pos.Length - 1);
            scrollpos = pos[posis];
        }


        distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        for (int i = 0; i < pos.Length; i++)
        {
            images[i].color = new Color(colors[i].r, colors[i].g, colors[i].b, 0.5f);
            if (scrollpos < pos[i] + (distance / 2) && scrollpos > pos[i] - (distance / 2))
            {
                images[i].color = colors[i];
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 4.5f * Time.deltaTime);
                posis = i;
            }
        }


        for (int i = 0; i < pos.Length; i++)
        {
            if(scrollpos < pos[i] + (distance / 2) && scrollpos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1, 1), 0.1f);
                for (int j = 0; j < pos.Length; j++)
                {
                    if(j!=i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 4.5f * Time.deltaTime);
                    }
                }
            }
        }
    }

    public void ScaleReset()
    {
        pos = new float[transform.childCount];
        for (int i = 0; i < pos.Length; i++)
        {
            transform.GetChild(i).localScale = new Vector2(1, 1);
        }
    }
}
