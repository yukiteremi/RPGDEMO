using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour
{

    public List<talkContont> talklist = new List<talkContont>();
    public List<talkContont> talklistcomplete = new List<talkContont>();
    public List<talkContont> collect = new List<talkContont>();
    public List<talkContont> collectcomplete = new List<talkContont>();
    public GameObject talk;
    public NPC myNpcData;
    bool flag = false;
    // Start is called before the first frame update
    void Start()
    {
        talklist.Add(new talkContont("忍者", "对话1"));
        talklist.Add(new talkContont("武士", "对话2"));
        talklist.Add(new talkContont("忍者", "对话3"));
        talklist.Add(new talkContont("忍者", "接收打怪任务1"));

        talklistcomplete.Add(new talkContont("武士", "提交任务"));
        talklistcomplete.Add(new talkContont("忍者", "完成任务1"));

        collect.Add(new talkContont("武士", "对话1"));
        collect.Add(new talkContont("忍者", "对话2"));
        collect.Add(new talkContont("忍者", "接收采集任务1"));

        collectcomplete.Add(new talkContont("武士", "提交任务"));
        collectcomplete.Add(new talkContont("忍者", "完成采集任务1"));
        MessageCenter.Get().OnAddListen("TalkComplete", add);
        MessageCenter.Get().OnAddListen("BtnOnclick", click);

    }

    private void click(object obj)
    {
        object[] ob = obj as object[];
        int num = int.Parse(ob[0].ToString());
        TalkController.Get().OpenTalkPanel();
        if (num==0)
        {
            if (World.Get().hostPlayer.taskdic.TryGetValue("打怪",out TaskInfo newData))
            {
                if (newData.isComplet)
                {
                    TalkController.Get().StartTalk(talklistcomplete, "打怪完成");
                }
            }
            else
            {
                TalkController.Get().StartTalk(talklist, "打怪");
            }
        }
        else
        {
            if (World.Get().hostPlayer.taskdic.TryGetValue("采集", out TaskInfo newData))
            {
                if (newData.isComplet)
                {
                    TalkController.Get().StartTalk(collectcomplete, "采集完成");
                }
            }
            else
            {
                TalkController.Get().StartTalk(collect, "采集");
            }
            
        }
    }

    private void add(object obj)
    {
        object[] ob = obj as object[];
        string str = ob[0].ToString();
        if (str=="打怪")
        {
            TaskInfo newTask = new TaskInfo();
            newTask.taskName = "打怪";
            newTask.isComplet = false;
            newTask.taskFromNpcName = gameObject.name;
            newTask.taskToObjName= "白虎王";
            newTask.taskProgressNow = 0;
            newTask.taskProgressMax = 1;
            newTask.taskSub = "打怪：白虎王！";

            World.Get().hostPlayer.AddTask(newTask);
        }
        else if (str == "采集")
        {
            TaskInfo newTask = new TaskInfo();
            newTask.taskName = "采集";
            newTask.isComplet = false;
            newTask.taskFromNpcName = gameObject.name;
            newTask.taskToObjName = "刀客 (1)";
            newTask.taskProgressNow = 0;
            newTask.taskProgressMax = 4;
            newTask.taskSub = "采集：刀客 (1)";
            World.Get().hostPlayer.AddTask(newTask);
        }
        else if (str == "打怪完成")
        {
            World.Get().hostPlayer.CompleteTask("打怪");
        }
        else if (str == "采集完成")
        {
            World.Get().hostPlayer.CompleteTask("采集");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
