using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMng : MonoBehaviour
{
    [Header("Enemy Object List")]
    Dictionary<string, Enemy> EnemyObj = new Dictionary<string, Enemy>();
    List<Enemy> EnemyList = new List<Enemy>();

    private static EnemyMng instance;
    public static EnemyMng ins
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyMng>();
                if (instance == null)
                {
                    GameObject enemyobj = new GameObject("EnemyMng");

                    instance = enemyobj.AddComponent<EnemyMng>();
                    instance.LoadFile(ref instance.EnemyObj, "Enemy/");
                }
            }
            return instance;
        }
    }


    private void LoadFile<T>(ref Dictionary<string, T> a, string path) where T : Object
    {
        a = new Dictionary<string, T>();
        T[] particleSystems = Resources.LoadAll<T>(path);
        foreach (var particle in particleSystems)
        {
            a.Add(particle.name, particle);
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            SetDisable();
        }

        for (int i = 0; i < EnemyList.Count; i++)
        {
            if(EnemyList[i].bUse)
            {
                if (EnemyList[i].transform.position.x <= -80f)
                {
                    EnemyList[i].Kill();
                }
            }
        }
    }

    public Enemy DistanceShortEnemy(Vector3 pos)
    {
        float m = 10000000;
        Enemy e = null;
        foreach(var enemy in EnemyList)
        {
            if(enemy.bUse && enemy.bInCam && enemy.name != "Boss" && enemy.name != "Insect")
            {
                if((pos - enemy.transform.position).magnitude < m)
                {
                    m = (pos - enemy.transform.position).magnitude;
                    e = enemy;
                }
            }
        }
        return e;
    }

    public void CreateEnemy(string path, Vector3 pos)
    {
        if (EnemyObj[path] == null)
        {
            return;
        }

        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i].name == path && !EnemyList[i].bUse)
            {
                EnemyList[i].CreateEnemy(pos);
                return;
            }
        }

        Enemy obj;
        obj = Instantiate(EnemyObj[path]).GetComponent<Enemy>();
        obj.transform.parent = transform;
        obj.CreateEnemy(pos);
        EnemyList.Add(obj);
    }

    public void Load()
    {

    }

    public void SetDisable()
    {
        for(int i=0;i<EnemyList.Count;i++)
        {
            EnemyList[i].Kill();
        }
    }
}
