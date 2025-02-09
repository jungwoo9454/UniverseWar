using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    RectTransform FrontJoyStick;
    RectTransform BackJoyStick;
    float fRadius;

    void Start()
    {
        BackJoyStick = GetComponent<RectTransform>();
        FrontJoyStick = transform.GetChild(0).GetComponent<RectTransform>();
        fRadius = BackJoyStick.rect.width * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)BackJoyStick.position;

        value = Vector2.ClampMagnitude(value, fRadius);

        FrontJoyStick.localPosition = value;

        GameMng.ins.PlayerControl.Move(value.normalized);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameMng.ins.PlayerControl.bButtonDown = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameMng.ins.PlayerControl.bButtonDown = true;
        FrontJoyStick.localPosition = Vector3.zero;
        GameMng.ins.PlayerControl.Move(Vector3.zero);
    }
}
