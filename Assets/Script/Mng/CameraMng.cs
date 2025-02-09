using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMng : MonoBehaviour
{
    [Header("Main Camera")]
    public Camera MainCamera;
    public Transform CamerHold;

    Vector3 Center;
    Vector2 prevRot;

    private static CameraMng cameramng;
    public static CameraMng ins
    {
        get
        {
            if (cameramng == null)
            {
                cameramng = FindObjectOfType<CameraMng>();

                if (cameramng == null)
                {
                    GameObject Cameraobj = new GameObject();
                    Cameraobj.name = "CameraMng";
                    cameramng = Cameraobj.AddComponent<CameraMng>();
                }
            }
            return cameramng;
        }
    }

    public IEnumerator ShakeCoru(float duration, float magnitude)
    {
        Vector3 OriginalPos = MainCamera.transform.localPosition;

        float elSpeed = 0.0f;

        while (elSpeed <= duration)
        {
            float x = Random.Range(-1.0f, 1.0f) * magnitude;
            float y = Random.Range(-1.0f, 1.0f) * magnitude;

            MainCamera.transform.localPosition = new Vector3(x, y, OriginalPos.z);

            elSpeed += Time.deltaTime;

            yield return null;
        }

        MainCamera.transform.localPosition = new Vector3(0,0,0);
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoru(duration, magnitude));
    }

    private void Update()
    {
        Center = new Vector3(-GameMng.ins.PlayerTrans.position.x - 40, -GameMng.ins.PlayerTrans.position.y, 0) / 25;
        CamerHold.transform.RotateAround(Center, Vector3.left, prevRot.y - Center.y);
        CamerHold.transform.RotateAround(Center, Vector3.up, prevRot.x - Center.x);

        prevRot.x = Center.x;
        prevRot.y = Center.y;

    }
}
