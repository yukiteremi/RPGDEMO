using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter 
{
    static MessageCenter ins;
    public static MessageCenter Get()
    {
        if (ins == null)
        {
            ins = new MessageCenter();
        }
        return ins;
    }

    Dictionary<string, Action<object>> dic = new Dictionary<string, Action<object>>();

    public void OnAddListen(string id, Action<object> obj)
    {
        if (dic.ContainsKey(id))
        {
            dic[id] += obj;
        }
        else
        {
            dic.Add(id, obj);
        }
    }
    public void RemoveAllListen(string id)
    {
        if (dic.ContainsKey(id))
        {
            dic.Remove(id);
        }
    }
    public void RemoveListen(string id, Action<object> obj)
    {
        if (dic.ContainsKey(id))
        {
            dic[id] -= obj;
            if (dic[id] == null)
            {
                dic.Remove(id);
            }
        }
    }

    public void OnDisPatch(string id, params object[] obj)
    {
        if (dic.ContainsKey(id))
        {
            dic[id](obj);
        }
        else
        {
            Debug.Log("消息" + id + "未注册");
        }
    }
}
