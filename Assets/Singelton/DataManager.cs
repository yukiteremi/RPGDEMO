using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
public class DataManager 
{
    static DataManager ins;
    public static DataManager Get()
    {
        if (ins == null)
        {
            ins = new DataManager();
        }
        return ins;
    }
    public int nowtype=0;
    /*
     地图数据  包括副本1 副本2配置
     */
    List<MapData> maplist = new List<MapData>();
    /*
      角色账号数据  用户密码
     */
    List<UserData> userDatas = new List<UserData>();
    /*
     角色技能数据
     */
    public playerSkillData skilldata = new playerSkillData();
    public MapData targetData;
    public void Init()
    {
        if (File.Exists(Application.dataPath + "/Json/userData.json"))
        {
            userDatas = JsonConvert.DeserializeObject<List<UserData>>(File.ReadAllText(Application.dataPath + "/Json/userData.json"));
        }
        string path = Application.dataPath + "/Json/map.json";
        if (File.Exists(path))
        {
            maplist = JsonConvert.DeserializeObject<List<MapData>>(File.ReadAllText(path));
        }
        string chapath = Application.dataPath + "/Json/刀客.json";
        if (File.Exists(path))
        {
            skilldata = JsonConvert.DeserializeObject<playerSkillData>(File.ReadAllText(chapath));
        }
    }
    public void SetTarget(int num)
    {
        targetData = maplist[num];
    }
    public void Add(UserData data)
    {
        userDatas.Add(data);
    }
    public bool Config(string name, string pass)
    {
        foreach (var item in userDatas)
        {
            if (item.userName == name && item.passWord == pass)
            {
                return true;
            }
        }
        return false;
    }
    public bool Config(string name)
    {
        foreach (var item in userDatas)
        {
            if (item.userName == name)
            {
                return false;
            }
        }
        return true;
    }
    //保存账号密码
    public void SaveJson()
    {
        File.WriteAllText(Application.dataPath + "/Json/userData.json", JsonConvert.SerializeObject(userDatas));
    }
}


