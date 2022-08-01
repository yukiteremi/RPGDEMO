using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System;

public class LoginUI : MonoBehaviour
{
    public InputField NameField,PassField;
    public Button loginBtn, regesterBtn;

    private void close(object obj)
    {
        object[] o1= obj as object[];
        if (int.Parse(o1[0].ToString())==0)
        {
            return;
        }
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        NameField = transform.Find("InputField").GetComponent<InputField>();
        PassField = transform.Find("InputField (1)").GetComponent<InputField>();
        loginBtn = transform.Find("Button").GetComponent<Button>();
        regesterBtn = transform.Find("Button (1)").GetComponent<Button>();

        MessageCenter.Get().OnAddListen("CloseButtonClick",close);
        Debug.Log("?");
        loginBtn.onClick.AddListener(()=> {
            if (NameField.text.Length>0 && PassField.text.Length>0)
            {
                bool result=  DataManager.Get().Config(NameField.text, PassField.text);
                if (result)
                {
                    DataManager.Get().nowtype = 1;
                    TipsController.Get().Open("登录成功！");
                }
                else
                {
                    TipsController.Get().Open("用户名密码错误！");
                    return;
                }
            }
            else
            {
                TipsController.Get().Open("请输入用户名或者密码！");
                return;
            }
        });

        regesterBtn.onClick.AddListener(()=> {
            if (NameField.text.Length > 0 && PassField.text.Length > 0)
            {
                bool result = DataManager.Get().Config(NameField.text);
                if (result)
                {
                    TipsController.Get().Open("注册成功！");
                    UserData data = new UserData();
                    data.userName = NameField.text;
                    data.passWord = PassField.text;
                    DataManager.Get().Add(data);
                    DataManager.Get().SaveJson();
                }
                else
                {
                    TipsController.Get().Open("用户已经存在！");
                    return;
                }
            }
            else
            {
                TipsController.Get().Open("请输入用户名或者密码！");
                return;
            }
        });
    }

    

    // Update is called once per frame
    void Update()
    {
       
    }
}
