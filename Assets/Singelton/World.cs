using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World 
{
    static World ins;
    public static World Get()
    {
        if (ins == null)
        {
            ins = new World();
        }
        return ins;
    }
    /*
     todo 
    当前应该有 玩家 npc集合 敌人集合 采集物集合
     */
    public HostPlayer hostPlayer;//玩家
    public Dictionary<string, NPC> NpcDic = new Dictionary<string, NPC>();
    public Dictionary<string, Enemy> EnemyDic = new Dictionary<string, Enemy>();
    public Dictionary<string, Collect> CollectDic = new Dictionary<string, Collect>();
    public TaskInfo nowTask;
    public ObjectBase target;
    public Collect collectTarget;
    public void Init(Transform t1)
    {
        foreach (var data in DataManager.Get().targetData.modelList)
        {
            if (data.modelType==ModelType.Player)
            {
                hostPlayer = new HostPlayer(data,t1);
            }
            else if (data.modelType==ModelType.NPC)
            {
                NpcDic.Add(data.name,new NPC(data, t1));
            }
            else if (data.modelType == ModelType.Moster)
            {
                EnemyDic.Add(data.name, new Enemy(data, t1));
            }
            else if (data.modelType == ModelType.Collect)
            {
                CollectDic.Add(data.name, new Collect(data, t1));
            }
        }
    }

    internal void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out RaycastHit info))
            {
                if (info.collider.tag== "Moster")
                {
                    target = info.collider.GetComponent<EnemyScript>().mydata;
                }
            }
        }
    }
}
