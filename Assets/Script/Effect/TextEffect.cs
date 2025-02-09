using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    bool bUse = false;
    bool bReset = false;
    float fAlpha;
    Vector2 dir;
    Rigidbody2D TextRigid;
    RectTransform Texttrans;
    Text textEffect;
    Outline outline;

    public bool GetUse()
    {
        return bUse;
    }

    void Update()
    {
        if(bUse)
        {
            if(TextRigid.velocity.y <= 0)
            {
                fAlpha -= Time.deltaTime * 2f;

                Color aColor = textEffect.color;
                Color oColor = outline.effectColor;
                aColor.a = fAlpha;
                oColor.a = fAlpha;
                textEffect.color = aColor;
                outline.effectColor = oColor;
                if (fAlpha <= 0f)
                {
                    bUse = false;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(!bReset)
        {
            bReset = true;
            TextRigid.velocity = dir;
        }
    }


    public void CreateTextEffect(Vector3 pos, float amount, Color color, Color outColor, string text, Vector2 scale)
    {
        if(Texttrans == null || TextRigid == null || textEffect== null)
        {
            Texttrans = GetComponent<RectTransform>();
            TextRigid = GetComponent<Rigidbody2D>();
            textEffect = GetComponent<Text>();
            outline = GetComponent<Outline>();
        }

        Texttrans.localScale = scale;

        if (outColor == new Color(255,0,255))
            GetComponent<Outline>().enabled = false;
        else
        {
            GetComponent<Outline>().effectColor = outColor;
            GetComponent<Outline>().enabled = true;
        }

        Vector3 AddPos;

        AddPos.x = Random.Range(-3f, 3f);
        AddPos.y = Random.Range(1f, 3f);

        fAlpha = 1.2f;

        dir = new Vector2(2f, 4.5f);

        Texttrans.position = pos + new Vector3(AddPos.x, AddPos.y, 0);
        TextRigid.velocity = dir;

        if(text == null)
            textEffect.text = ((int)amount).ToString();
        else
            textEffect.text = text;

        textEffect.color = color;

        bUse = true;
        bReset = false;
        gameObject.SetActive(true);

    }
}
