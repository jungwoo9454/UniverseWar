using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMng : MonoBehaviour
{
    [Header("Meteo Parent Object")]
    public GameObject[] MeteoOriginal;
    List<Meteo> MeteoList = new List<Meteo>();

    public GameObject ItemOriginal;
    List<Item> ItemList = new List<Item>();

    public GameObject DroneOriginal;
    List<Drone> DroneList = new List<Drone>();

    public GameObject GemOriginal;
    List<Gem> GemList = new List<Gem>();

    public ParticleSystem Gemparticle;

    float fTime;

    private static ObjectMng objectMng;
    public static ObjectMng ins
    {
        get
        {
            if (objectMng == null)
            {
                objectMng = FindObjectOfType<ObjectMng>();

                if (objectMng == null)
                {
                    GameObject objectobj = new GameObject();
                    objectobj.name = "ObjectMng";
                    objectMng = objectobj.AddComponent<ObjectMng>();
                }
            }
            return objectMng;
        }
    }

    public void CreateMeteo(Vector3 pos, float Rot, float Damage, float Speed, float Hp = 5)
    {
        bool findMeteo = false;
        for (int i = 0; i < MeteoList.Count; i++)
        {
            if (!MeteoList[i].GetDead())
            {
                MeteoList[i].CreateMeteo(pos, Rot, Damage, Speed, Hp);
                findMeteo = true;
                break;
            }
        }

        if (!findMeteo)
        {
            Meteo meteo;
            meteo = Instantiate(MeteoOriginal[Random.Range(0, MeteoOriginal.Length)]).GetComponent<Meteo>();
            meteo.transform.parent = transform;
            meteo.CreateMeteo(pos, Rot, Damage, Speed, Hp);
            MeteoList.Add(meteo);
        }
    }

    public void CreateDrone()
    {
        bool findMeteo = false;
        for (int i = 0; i < DroneList.Count; i++)
        {
            if (!DroneList[i].GetDead())
            {
                DroneList[i].CreateDrone();
                findMeteo = true;
                break;
            }
        }

        if (!findMeteo)
        {
            Drone drone;
            drone = Instantiate(DroneOriginal).GetComponent<Drone>();
            drone.transform.parent = transform;
            drone.CreateDrone();
            DroneList.Add(drone);
        }
    }

    public void CreateItem(Vector3 pos, ITEM WhatItem, float Pct)
    {
        if(Pct >= Random.Range(0.0f,1.0f))
        {
            bool findItem = false;
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (!ItemList[i].GetDead())
                {
                    ItemList[i].CreateItem(pos, WhatItem);
                    findItem = true;
                    break;
                }
            }

            if (!findItem)
            {
                Item item;
                item = Instantiate(ItemOriginal).GetComponent<Item>();
                item.transform.parent = transform;
                item.CreateItem(pos, WhatItem);
                ItemList.Add(item);
            }
        }
    }

    public void CreateGem(Vector3 pos, int nGem)
    {
        bool findGem = false;
        for (int i = 0; i < GemList.Count; i++)
        {
            if (!GemList[i].GetDead())
            {
                GemList[i].CreateGem(pos, nGem);
                findGem = true;
                break;
            }
        }

        if (!findGem)
        {
            Gem gem;
            gem = Instantiate(GemOriginal).GetComponent<Gem>();
            gem.transform.parent = transform;
            gem.CreateGem(pos, nGem);
            GemList.Add(gem);
        }
    }

    private void Update()
    {
        fTime += Time.deltaTime;

        if(fTime > 1.0f && !StageMng.ins.bStagePause)
        {
            fTime = 0;
            if(Random.Range(0,100) > 65)
            {
                CreateMeteo(new Vector3(70, Random.Range(-35, 35), 0), 180, 12, 30);
            }
        }
    }
}
