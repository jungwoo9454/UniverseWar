using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;

public class RenderingMng : MonoBehaviour
{
    public float fSkyBoxSpeed;
    [Header("Materials")]
    public Material HitMat;


    [Header("Bullet")]
    public Material RedBullet;
    public Material SkyBullet;
    public Material OrangeBullet;
    public Material GreenBullet;
    public Material YellowBullet;
    public Material WhiteBullet;
    public Material grayBullet;

    float fScroll;

    PostProcessVolume PV;
    PostProcessLayer PL;

    [Header("Depth Of Field")]
    public float fFocusDistance;
    public float fAperture;
    public float fFocalLength;

    [Header("Color Grading")]
    public float fSaturation;


    public Vignette vignette;
    public DepthOfField Dof;
    public ColorGrading CG;

    private void Start()
    {
        PV = FindObjectOfType<PostProcessVolume>();
        PL = FindObjectOfType<PostProcessLayer>();

        if(vignette == null)
        {
            vignette = PV.profile.GetSetting<Vignette>();
            CG = PV.profile.GetSetting<ColorGrading>();
            Dof = PV.profile.GetSetting<DepthOfField>();
        }
    }

    private static RenderingMng rendermng;
    public static RenderingMng ins
    {
        get
        {
            if (rendermng == null)
            {
                rendermng = FindObjectOfType<RenderingMng>();

                if (rendermng == null)
                {
                    GameObject renderobj = new GameObject();
                    renderobj.name = "RenderingMng";
                    rendermng = renderobj.AddComponent<RenderingMng>();
                }
            }
            return rendermng;
        }
    }

    private void Update()
    {
        fScroll += fSkyBoxSpeed * Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", fScroll);

        Dof.focusDistance.value = fFocusDistance;
        Dof.aperture.value = fAperture;
        Dof.focalLength.value = fFocalLength;

        if(GameMng.ins.bPlayerDead)
            CG.saturation.value = fSaturation;
    }
}
