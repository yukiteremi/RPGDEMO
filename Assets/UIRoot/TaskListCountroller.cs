using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListCountroller : MonoBehaviour
{
    public GameObject contont;
    public ScrollRect rect;
    public List<TaskInfo> tasklist = new List<TaskInfo>();
    public List<Transform> list = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<ScrollRect>();
        contont = rect.content.gameObject;
    }

    public void Add(TaskInfo data)
    {
        tasklist.Add(data);
        GameObject Newtask = Resources.Load<GameObject>("UI/Task");
        Transform clone = Instantiate(Newtask, contont.transform, false).transform;
        list.Add(clone);
        clone.Find("nameText").GetComponent<Text>().text = data.taskName;
        clone.Find("subText").GetComponent<Text>().text = "任务描述：" + data.taskSub;
        clone.Find("progressText").GetComponent<Text>().text = "任务进度：" + data.taskProgressNow+"/"+data.taskProgressMax;

        MessageCenter.Get().OnAddListen("DataChange",(da)=> {
            if (clone!=null)
            {
                clone.Find("progressText").GetComponent<Text>().text = "任务进度：" + data.taskProgressNow + "/" + data.taskProgressMax;
            }
        });

        clone.GetComponent<Button>().onClick.AddListener(() => {
            World.Get().nowTask = data;
            if (data.isComplet)
            {
                World.Get().hostPlayer.player.moveTo(World.Get().NpcDic[data.taskFromNpcName].gameGo.transform);
            }
            else
            {
                if (World.Get().EnemyDic.TryGetValue(data.taskToObjName, out Enemy go))
                {
                    if (go.gameGo != null)
                    {
                        World.Get().hostPlayer.player.moveTo(go.gameGo.transform);
                        World.Get().target = go;
                    }
                    else
                    {
                        foreach (var item in World.Get().EnemyDic.Values)
                        {
                            if (item.gameGo != null)
                            {
                                World.Get().target = item;
                                World.Get().hostPlayer.player.moveTo(item.gameGo.transform);
                                break;
                            }
                        }
                    }

                }
                else if (World.Get().CollectDic.TryGetValue(data.taskToObjName, out Collect gos))
                {
                    if (gos.gameGo != null)
                    {
                        World.Get().hostPlayer.player.moveTo(gos.gameGo.transform);
                        World.Get().target = gos;
                    }
                    else
                    {
                        foreach (var item in World.Get().CollectDic.Values)
                        {
                            if (item.gameGo != null)
                            {
                                World.Get().target = item;
                                World.Get().hostPlayer.player.moveTo(item.gameGo.transform);
                                break;
                            }
                        }
                    }

                }
                else
                {
                    foreach (var item in World.Get().EnemyDic.Values)
                    {
                        if (item.gameGo.name.Contains(data.taskToObjName))
                        {
                            World.Get().target = item;
                            World.Get().hostPlayer.player.moveTo(item.gameGo.transform);
                            break;
                        }
                    }
                    foreach (var item in World.Get().CollectDic.Values)
                    {
                        if (item.gameGo.name.Contains(data.taskToObjName))
                        {
                            World.Get().target = item;
                            World.Get().hostPlayer.player.moveTo(item.gameGo.transform);
                            break;
                        }
                    }
                }
            }
        });
    }

    public void Complete(TaskInfo ta)
    {
        for (int i = 0; i < tasklist.Count; i++)
        {
            if (tasklist[i].taskName == ta.taskName)
            {
                tasklist.RemoveAt(i);
                Destroy(list[i].gameObject);
                list.RemoveAt(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
