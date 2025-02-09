using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisoveAniControl : MonoBehaviour
{
    public float fSensitivity;
    public void ValueControl()
    {
        MenuUI.ins.Disolve.SetTexture("_MainTex", DataMng.ins.planeinfo[MenuUI.ins.nCurAirplane].texture);
        MenuUI.ins.AirPlane.transform.localScale = DataMng.ins.planeinfo[MenuUI.ins.nCurAirplane].Airplane.transform.localScale * 5;
        MenuUI.ins.AirPlane.GetComponent<MeshFilter>().mesh = DataMng.ins.planeinfo[MenuUI.ins.nCurAirplane].mesh;
        MenuUI.ins.AirPlane.GetComponent<MeshRenderer>().material = MenuUI.ins.Disolve;
        transform.rotation = Quaternion.Euler(new Vector3(0, -100, 0));
    }


    private void OnMouseDrag()
    {
        float XaxisRotation = Input.GetAxis("Mouse X") * fSensitivity;
        transform.RotateAround(transform.position, Vector3.down, XaxisRotation);
    }

}
