using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FSMtype
{
    normal, //普通
    walk,   //巡逻
    warning,//警戒
    Pursue, //追击
    attack, //攻击
    die,    //死亡
    hited   //
}
public class FSMSystem
{
    public Dictionary<FSMtype, FSMState> stateDic = new Dictionary<FSMtype, FSMState>();
    FSMState NowState;

    public void AddState(FSMtype type, FSMState state)
    {
        if (!stateDic.ContainsKey(type))
        {
            stateDic.Add(type, state);
        }
    }

    public void SetNowState(FSMtype type)
    {
        if (stateDic.ContainsKey(type))
        {
            NowState = stateDic[type];
        }
    }
    public void Update(GameObject game)
    {
        NowState.Do(game);
        NowState.Check(game);
    }
    public void SetData(float num)
    {
        if (NowState is hitedState)
        {
            hitedState now = NowState as hitedState;
            now.waitTime = num;
        }
    }
}
public class FSMState
{
    public FSMSystem system;
    public FSMState(FSMSystem sys)
    {
        system = sys;
    }

    public virtual void Do(GameObject gameObject)
    {

    }
    public virtual void Check(GameObject gameObject)
    {

    }
}
public class walkState : FSMState
{
    public List<Vector3> targetList = new List<Vector3>();
    Vector3 target;
    int len = 0;

    public walkState(FSMSystem sys) : base(sys)
    {
        GameObject path = GameObject.Find("Path");
        for (int i = 0; i < path.transform.childCount; i++)
        {
            targetList.Add(path.transform.GetChild(i).transform.position);
        }
        target = targetList[len];
    }

    public override void Do(GameObject gameObject)
    {
        gameObject.GetComponent<Animator>().SetBool("Move", true);
        gameObject.transform.LookAt(target);
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 3);
    }
    public override void Check(GameObject gameObject)
    {

        if (Vector3.Distance(gameObject.transform.position, World.Get().hostPlayer.gameGo.transform.position) <= 5)
        {
            system.SetNowState(FSMtype.Pursue);
        }
        if (Vector3.Distance(gameObject.transform.position, target) < 0.5f)
        {
            len++;
            target = targetList[len % targetList.Count];
        }

    }
}

public class PursueState : FSMState
{

    public PursueState(FSMSystem sys) : base(sys)
    {

    }

    public override void Do(GameObject gameObject)
    {
        if (gameObject.GetComponent<EnemyScript>().flag)
        {
            return;
        }
        if (Vector3.Distance(gameObject.transform.position, World.Get().hostPlayer.gameGo.transform.position) < 1)
        {
            gameObject.GetComponent<Animator>().SetBool("Move", false);
            return;
        }
        gameObject.GetComponent<Animator>().SetBool("Move", true);
        gameObject.transform.LookAt(World.Get().hostPlayer.gameGo.transform);
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 3);
    }

    public override void Check(GameObject gameObject)
    {
        if (Vector3.Distance(gameObject.transform.position, World.Get().hostPlayer.gameGo.transform.position) > 5)
        {
            system.SetNowState(FSMtype.walk);
        }
        if (Vector3.Distance(gameObject.transform.position, World.Get().hostPlayer.gameGo.transform.position) <= 1)
        {
            gameObject.GetComponent<Animator>().SetBool("Move", false);
            system.SetNowState(FSMtype.attack);
        }
    }

}


public class AttackState : FSMState
{
    float attackTime = 0;
    public AttackState(FSMSystem sys) : base(sys)
    {

    }

    public override void Do(GameObject gameObject)
    {
        attackTime -= Time.deltaTime;
        if (attackTime<=0)
        {
            attackTime = 3;
            Debug.Log("attack");
        }
    }

    public override void Check(GameObject gameObject)
    {
        if (Vector3.Distance(gameObject.transform.position, World.Get().hostPlayer.gameGo.transform.position) > 1)
        {
            system.SetNowState(FSMtype.Pursue);
        }
    }
}


public class hitedState : FSMState
{
    public float waitTime = 5;
    public hitedState(FSMSystem sys) : base(sys)
    {

    }

    public override void Do(GameObject gameObject)
    {
        gameObject.GetComponent<Animator>().SetBool("hitted", true);
        gameObject.GetComponent<Animator>().SetBool("Move", false);
        waitTime -= Time.deltaTime;
    }

    public override void Check(GameObject gameObject)
    {
        if (gameObject.GetComponent<EnemyScript>().mydata.datas.nowhp <= 0)
        {
            system.SetNowState(FSMtype.die);
        }
        if (waitTime <= 0)
        {
            waitTime = 5;
            gameObject.GetComponent<Animator>().SetBool("hitted", false);
            system.SetNowState(FSMtype.walk);
        }
    }

}

public class DieState : FSMState
{
    float time = 3;
    public DieState(FSMSystem sys) : base(sys)
    {

    }

    public override void Do(GameObject gameObject)
    {
        time -= Time.deltaTime;
        GameObject g1 = gameObject.transform.Find("Bip001/h_tigerlord").gameObject;
        gameObject.GetComponent<Animator>().SetBool("die", true);
        if (time <= 0)
        {
            g1.GetComponent<SkinnedMeshRenderer>().material.color = new Color(
            g1.GetComponent<SkinnedMeshRenderer>().material.color.r,
             g1.GetComponent<SkinnedMeshRenderer>().material.color.g,
              g1.GetComponent<SkinnedMeshRenderer>().material.color.b,
               g1.GetComponent<SkinnedMeshRenderer>().material.color.a - 0.005f
            );
        }
    }

    public override void Check(GameObject gameObject)
    {
        GameObject g1 = gameObject.transform.Find("Bip001/h_tigerlord").gameObject;
        if (g1.GetComponent<SkinnedMeshRenderer>().material.color.a <= 0)
        {
            MessageCenter.Get().OnDisPatch("MonsterDie", gameObject.name);
            foreach (var item in World.Get().EnemyDic)
            {
                if (item.Value.gameGo==gameObject)
                {
                    World.Get().EnemyDic.Remove(item.Key);
                    break;
                }
            }
            GameObject.Destroy(gameObject);
            DataManager.Get().nowtype = 2;
            //TipsController.Get().Open("游戏结束");
            //World.Get().host.Change("打怪");
        }
    }
}
