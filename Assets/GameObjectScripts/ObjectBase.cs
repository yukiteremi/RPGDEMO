using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class ObjectBase 
{
    public UIHUD hp;
    public GameObject gameGo;
    public BaseInfo datas;

    public ObjectBase(ModelData data,Transform tr)
    {
        datas = new BaseInfo();
        datas.maxhp = 1000;
        datas.nowhp = 1000;

        GameObject clone = Resources.Load<GameObject>("PlayerModel/" + data.modelName);
        gameGo = GameObject.Instantiate(clone, tr, false);
        gameGo.transform.position = new Vector3(data.posX, data.posY, data.posZ);
        gameGo.transform.eulerAngles = new Vector3(data.X, data.Y, data.Z);
        gameGo.name = data.name;
        gameGo.tag = data.modelType.ToString();
        hp = UIHUD.Create(gameGo.gameObject, datas.maxhp);
        
    }

   
}

public class HostPlayer:ObjectBase
{
    public Player player;
    public Dictionary<string, TaskInfo> taskdic = new Dictionary<string, TaskInfo>();
    public GameObject skillShan,line;
    public bool ShanIsOn;
    public HostPlayer(ModelData data, Transform tr) : base(data, tr)
    {
        hp.Init(data.name, 0);
        player = gameGo.AddComponent<Player>();
        player.Init(this);
        skillShan = GameObject.Instantiate(Resources.Load<GameObject>("Indicator"), gameGo.transform, false);
        skillShan.transform.position =new Vector3(0, -0.15f, 0);
        skillShan.SetActive(false);
        line = GameObject.Instantiate(Resources.Load<GameObject>("LineSkill"), gameGo.transform, false);
        line.transform.localPosition = Vector3.zero;
        line.AddComponent<LineSkillScript>();
        line.SetActive(false);
        MessageCenter.Get().OnAddListen("MonsterDie",MonsterDie);
        MessageCenter.Get().OnAddListen("CollectComplete", CollectIsComplete);
        MessageCenter.Get().OnAddListen("DestroyCollect",DestroyCollect);
        MessageCenter.Get().OnAddListen("SkillUse",SkillUse);
        MessageCenter.Get().OnAddListen("SkillBullet", SkillBullet);
        MessageCenter.Get().OnAddListen("SkillShan", SkillShan);
        MessageCenter.Get().OnAddListen("SkillCircle", SkillCircle);
        
    }

    private void SkillCircle(object obj)
    {
        if (World.Get().target.gameGo!=null)
        {
            if (World.Get().target is Enemy)
            {
                if (Vector3.Distance(gameGo.transform.position, World.Get().target.gameGo.transform.position)<=20)
                {
                    SkillUseByName(World.Get().target.gameGo.name, World.Get().target.gameGo.transform.position);
                    Enemy ene = World.Get().target as Enemy;
                    ene.Hited(2f);
                    SkillUseByName("fire", ene.gameGo.transform.position);
                    foreach (var item in World.Get().EnemyDic.Values)
                    {
                        if (item != ene)
                        {
                            if (Vector3.Distance(item.gameGo.transform.position, ene.gameGo.transform.position) <= 3)
                            {
                                item.Hited(2f);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("距离不够");
                }
            }
            else
            {
                Debug.Log("请选择目标");
            }
        }
        else
        {
            Debug.Log("请选择目标");
        }
        
    }

    private void SkillShan(object obj)
    {
        foreach (var item in World.Get().EnemyDic.Values)
        {
            if (Vector3.Angle(gameGo.transform.forward,item.gameGo.transform.position-gameGo.transform.position)<=35)
            {
                if (Vector3.Distance(gameGo.transform.position,item.gameGo.transform.position)<=10)
                {
                    item.Hited(2f);
                }
            }
        }
    }

    public void ShowShan()
    {
        ShanIsOn = true;
        skillShan.SetActive(ShanIsOn);
    }
    public void HideShan()
    {
        ShanIsOn = false;
        skillShan.SetActive(ShanIsOn);
    }
    private void SkillBullet(object obj)
    {
        object[] ob = obj as object[];
        Vector3 lerp = (Vector3)ob[0];
        GameObject bullet = GameObject.Instantiate(Resources.Load<GameObject>("Bullet"));
        bullet.transform.position = gameGo.transform.position+lerp.normalized;
        SkillBullet("bullet", bullet.transform);
        BulletScript script = bullet.AddComponent<BulletScript>();
        script.Init(lerp);
        ShowLine(false);

    }

    public void ShowLine(bool flag)
    {
        line.SetActive(flag);
    }

    private void SkillUse(object obj)
    {
        object[] ob = obj as object[];
        string str = ob[0].ToString();
        Vector3 pos = (Vector3)ob[1];
    }

    private void DestroyCollect(object obj)
    {
        Collect c1 = World.Get().target as Collect;
        c1.destr();
        World.Get().CollectDic.Remove(World.Get().target.gameGo.name);
        World.Get().target = null;
    }

    private void CollectIsComplete(object obj)
    {
        Collect c1= World.Get().collectTarget;
        c1.CollectOn();
        foreach (var item in taskdic)
        {
            if (item.Value.taskToObjName == "刀客 (1)")
            {
                item.Value.taskProgressNow++;
                MessageCenter.Get().OnDisPatch("DataChange", 0);
                if (item.Value.taskProgressNow == item.Value.taskProgressMax)
                {
                    item.Value.isComplet = true;
                }
            }
        }
    }

    private void MonsterDie(object obj)
    {
        foreach (var item in taskdic)
        {
            if (item.Value.taskToObjName=="白虎王")
            {
                item.Value.taskProgressNow++;
                MessageCenter.Get().OnDisPatch("DataChange", 0);
                if (item.Value.taskProgressNow== item.Value.taskProgressMax)
                {
                    item.Value.isComplet = true;
                }
            }
        }
        
    }

    public override string ToString()
    {
        return player.name;
    }
    public void AddTask(TaskInfo info)
    {
        taskdic.Add(info.taskName,info);
        UIManager.Get().AddNewTask(info);
    }
    public void CompleteTask(string name)
    {
        UIManager.Get().CompleteTask(taskdic[name]);
        taskdic.Remove(name);
    }

    public void SkillBullet(string name,Transform tran)
    {
        foreach (var item in DataManager.Get().skilldata.list)
        {
            if (item.myName == name)
            {
                foreach (var data in item.mydata)
                {
                    if (data.type == "动画")
                    {
                        AnimatorClipSkill skill = new AnimatorClipSkill();
                        skill.time = data.time;
                        skill.clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(data.path);
                        player.AniAdd(skill.clip, skill.time);
                    }
                    else if (data.type == "声音")
                    {
                        AudioClipSkill skill = new AudioClipSkill();
                        skill.time = data.time;
                        skill.clip = AssetDatabase.LoadAssetAtPath<AudioClip>(data.path);
                        player.bgmAdd(skill.clip, skill.time);
                    }
                    else if (data.type == "特效")
                    {
                        GameObject clip = AssetDatabase.LoadAssetAtPath<GameObject>(data.path);
                        player.EffAdd(clip, data.time, Vector3.zero, tran);
                    }
                }
            }
        }
    }
    /*todo
        人物的任务添加与删除功能
        人物的初始化
        人物
     */
    public void SkillUseByName(string skillName,Vector3 pos)
    {
        foreach (var item in DataManager.Get().skilldata.list)
        {
            if (item.myName == skillName)
            {
                foreach (var data in item.mydata)
                {
                    if (data.type == "动画")
                    {
                        AnimatorClipSkill skill = new AnimatorClipSkill();
                        skill.time = data.time;
                        skill.clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(data.path);
                        player.AniAdd(skill.clip, skill.time);
                    }
                    else if (data.type == "声音")
                    {
                        AudioClipSkill skill = new AudioClipSkill();
                        skill.time = data.time;
                        skill.clip = AssetDatabase.LoadAssetAtPath<AudioClip>(data.path);
                        player.bgmAdd(skill.clip, skill.time);
                    }
                    else if (data.type == "特效")
                    {
                        GameObject clip = AssetDatabase.LoadAssetAtPath<GameObject>(data.path);
                        if (pos.x!=Mathf.Infinity)
                        {
                           player.EffAdd(clip, data.time, pos);
                        }
                        else
                        {
                            player.EffAdd(clip, data.time);
                        }
                    }
                }
            }
        }
    }
    public void SkillUseByName(string skillName, Vector3 pos,Transform transform)
    {
        foreach (var item in DataManager.Get().skilldata.list)
        {
            if (item.myName == skillName)
            {
                foreach (var data in item.mydata)
                {
                    if (data.type == "动画")
                    {
                        AnimatorClipSkill skill = new AnimatorClipSkill();
                        skill.time = data.time;
                        skill.clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(data.path);
                        player.AniAdd(skill.clip, skill.time);
                    }
                    else if (data.type == "声音")
                    {
                        AudioClipSkill skill = new AudioClipSkill();
                        skill.time = data.time;
                        skill.clip = AssetDatabase.LoadAssetAtPath<AudioClip>(data.path);
                        player.bgmAdd(skill.clip, skill.time);
                    }
                    else if (data.type == "特效")
                    {
                        GameObject clip = AssetDatabase.LoadAssetAtPath<GameObject>(data.path);
                        if (pos.x != Mathf.Infinity)
                        {
                            player.EffAdd(clip, data.time, pos, transform);
                        }
                        else
                        {
                            player.EffAdd(clip, data.time);
                        }
                    }
                }
            }
        }
    }
}

public class Enemy : ObjectBase
{
    public EnemyScript Script;
    FSMSystem system;

    public Enemy(ModelData data, Transform tr) : base(data, tr)
    {
        hp.Init(data.name, 0);
        Script= gameGo.AddComponent<EnemyScript>();
        Script.mydata = this;
        system = new FSMSystem();
        system.AddState(FSMtype.walk, new walkState(system));
        system.AddState(FSMtype.Pursue, new PursueState(system));
        system.AddState(FSMtype.attack, new AttackState(system));
        system.AddState(FSMtype.die, new DieState(system));
        system.AddState(FSMtype.hited, new hitedState(system));
        system.SetNowState(FSMtype.walk);
        Script.system = system;
    }
    public void Hited(float waitTime)
    {
        float num = Random.Range(90, 100);
        datas.nowhp -= num;
        system.SetNowState(FSMtype.hited);
        system.SetData(waitTime);
        hp.HPchange(datas.nowhp, datas.maxhp, -num);
    }
}

public class NPC : ObjectBase
{
    NpcScript npc;
    public NPC(ModelData data, Transform tr) : base(data, tr)
    {
        npc = gameGo.AddComponent<NpcScript>();
        hp.Init(data.name, 1);
    }
}

public class Collect : ObjectBase
{
    CollectScript collect;
    int nowCollect = 4;
    public Collect(ModelData data, Transform tr) : base(data, tr)
    {
        hp.Init(data.name, 2);
        hp.CollectInit(nowCollect);
        collect =gameGo.AddComponent<CollectScript>();
    }
    public void destr()
    {
        GameObject.Destroy(gameGo.gameObject);
    }

    public void CollectOn()
    {
        Debug.Log("?");
        nowCollect--;
        hp.CollectChange();
    }
}


public class BaseInfo
{
    public float maxhp;
    public float nowhp;
}