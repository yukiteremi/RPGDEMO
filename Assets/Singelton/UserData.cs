using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData 
{
    public string userName;
    public string passWord;
}

public enum ModelType
{
    Null=0,
    Player,
    Moster,
    NPC,
    Collect,
    Other
}

public class ModelData
{
    public string name;
    public string modelName;
    public float posX;
    public float posY;
    public float posZ;
    public float X;
    public float Y;
    public float Z;
    public ModelType modelType;
    public List<task> tasklist = new List<task>();

}

public class task
{
    public string name;
    public string contont;
    public string progress;
    public bool isComplete;
}

public class MapData
{
    public string mapName;
    public List<ModelData> modelList = new List<ModelData>();
}

public class SkillJson
{
    public string name;
    public string path;
    public float time;
    public string type;
}

public class playerSkillData
{
    public string name;
    public List<skillShowType> list = new List<skillShowType>();

    public void RemoveSkill(string name)
    {
        foreach (var item in list)
        {
            if (item.myName == name)
            {
                list.Remove(item);
                break;
            }
        }
    }
}


public class skillShowType
{
    public string myName;
    public List<SkillJson> mydata = new List<SkillJson>();
    public void RemoveSkill(string name)
    {
        foreach (var item in mydata)
        {
            if (item.name == name)
            {
                mydata.Remove(item);
                break;
            }
        }
    }
}

public class SkillBase
{
    public string name;
    public string path;
    public float time;
    public string type;
    public virtual void DO()
    {

    }
}
public class AnimatorClipSkill : SkillBase
{
    public AnimationClip clip;
}
public class AudioClipSkill : SkillBase
{
    public AudioClip clip;
}
public class GameobjectClipSkill : SkillBase
{
    public GameObject clip;
}

public class TaskInfo{
    public string taskName;
    public string taskSub;
    public string taskFromNpcName;
    public string taskToObjName;
    public bool isComplet;
    public int taskProgressMax;
    public int taskProgressNow;
}

public class talkContont
{
    public string name;
    public string contont;

    public talkContont(string name, string contont)
    {
        this.name = name;
        this.contont = contont;
    }
}