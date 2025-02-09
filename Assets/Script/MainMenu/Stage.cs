using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LEVEL { NORMAL,EASY,HARD}

public class Stage : MonoBehaviour
{
    [Header("Stage Info")]
    Image MyImg;
    public string Stagename;
    public LEVEL level;
    public int nStageNumber;

    private void Start()
    {
        MyImg = GetComponent<Image>();
        transform.GetChild(0).GetComponent<Text>().text = "STAGE " + (nStageNumber + 1).ToString();
        transform.GetChild(1).GetComponent<Text>().text = Stagename;
        transform.GetChild(2).GetComponent<Text>().text = MenuUI.ins.GetlevelString(level);
        transform.GetChild(4).GetComponent<Text>().text = "HIGH SCORE";
    }

    private void Update()
    {
        if (DataMng.ins.bStageLock[nStageNumber])
        {
            transform.GetChild(3).GetComponent<Text>().text = DataMng.ins.nHighScores[nStageNumber].ToString();
            transform.GetChild(0).GetComponent<Text>().text = "STAGE " + (nStageNumber + 1).ToString();
            transform.GetChild(1).GetComponent<Text>().text = Stagename;
            transform.GetChild(2).GetComponent<Text>().text = MenuUI.ins.GetlevelString(level);
            transform.GetChild(4).GetComponent<Text>().text = "HIGH SCORE";
        }
        else
        {
            MyImg.sprite = MenuUI.ins.StageLockImg;
            transform.GetChild(3).GetComponent<Text>().text = "";
            transform.GetChild(0).GetComponent<Text>().text = "";
            transform.GetChild(1).GetComponent<Text>().text = "";
            transform.GetChild(2).GetComponent<Text>().text = "";
            transform.GetChild(4).GetComponent<Text>().text = "";
        }
    }

    public void Select()
    {
        DataMng.ins.nCurStage = nStageNumber;
        DataMng.ins.CurStageName = Stagename;
        Scene.ins.FadeScene("Game");
    }
}

