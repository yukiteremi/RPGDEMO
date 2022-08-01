using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Get().Init(); //数据初始化
        DataManager.Get().SetTarget(0);
        UIManager.Get().Init(); //ui初始化
        UIManager.Get().OpenLoginPanel();
        MessageCenter.Get().OnAddListen("CloseButtonClick", WorldInit);
    }

    private void WorldInit(object obj)
    {
        World.Get().Init(transform);//世界初始化
        StartCoroutine(time());
    }

    IEnumerator time()
    {
        yield return new WaitForSeconds(0.1f);
        UIManager.Get().OpenJoy();
        UIManager.Get().OpenSkillBtn();
        UIManager.Get().OpenTaskList();
    }
    // Update is called once per frame
    void Update()
    {
        UIManager.Get().Update();
        World.Get().Update();
    }
}
