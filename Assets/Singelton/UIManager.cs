using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class UIManager 
{
    static UIManager ins;
    
    public static UIManager Get()
    {
        if (ins==null)
        {
            ins = new UIManager();
        }
        return ins;
    }
    public LoginUI login;
    public GameObject server, joy, skill, task, talk, collectUI, npclist,skillBtnCircle, skillBtnShan, skillBtnBullet;
    public Image skillSimpleCDimg, skillCircleCDimg, skillShanCDimg, skillBulletCDimg;
    public Transform canvas;
    float skillSimpleCd = 0;
    float skillCircleCd = 0;
    float skillShanCd = 0;
    float skillBulletCd = 0;

    public  bool SkillBulletType = false;
    public void Update()
    {
        skillSimpleCd -= Time.deltaTime;
        skillCircleCd -= Time.deltaTime;
        skillShanCd -= Time.deltaTime;
        skillBulletCd -= Time.deltaTime;
        if (skillSimpleCDimg)
        {
            skillSimpleCDimg.fillAmount = skillSimpleCd / 2.24f;
        }
        if (skillCircleCDimg)
        {
            skillCircleCDimg.fillAmount = skillCircleCd / 2.24f;
        }
        if (skillShanCDimg)
        {
            skillShanCDimg.fillAmount = skillShanCd / 2.24f;
        }
        if (skillBulletCDimg)
        {
            skillBulletCDimg.fillAmount = skillBulletCd / 2.24f;
        }
    }
    public void Init()
    {
        canvas= GameObject.FindObjectOfType<Canvas>().transform;
        MessageCenter.Get().OnAddListen("SkillBullet", SkillBullet);
        MessageCenter.Get().OnAddListen("SkillShan", SkillShan);
    }

    private void SkillShan(object obj)
    {
        skillShanCd = 2.24f;
    }

    private void SkillBullet(object obj)
    {
        SkillBulletType = false;
        skillBulletCd = 2.24f;
    }

    public void OpenLoginPanel()
    {
        if (login==null)
        {
            GameObject clone=  GameObject.Instantiate(Resources.Load<GameObject>("UI/loginPanel"), canvas,false);
            login = clone.AddComponent<LoginUI>();
        }
        login.gameObject.SetActive(true);
    }
    public void OpenJoy()
    {
        if (joy==null)
        {
            joy = GameObject.Instantiate(Resources.Load<GameObject>("UI/joy"),canvas,false);
            joy.transform.GetChild(0).gameObject.AddComponent<Joycon>();
        }
        joy.gameObject.SetActive(true);
    }
    public void OpenSkillBtn()
    {
        if (skill==null)
        {
            skill = GameObject.Instantiate(Resources.Load<GameObject>("UI/skill"), canvas, false);
            skillSimpleCDimg = skill.transform.Find("Image").GetComponent<Image>();
            skill.GetComponent<Button>().onClick.AddListener(()=> {
                if (skillSimpleCd<=0)
                {
                    skillSimpleCd = 2.24f;
                    MessageCenter.Get().OnDisPatch("SkillSimple");
                    //MessageCenter.Get().OnDisPatch("SkillUse","fire",Vector3.one);
                }
                else
                {
                    Debug.Log("CD中");
                }
            }) ;
        }
        if (skillBtnCircle==null)
        {
            GameObject BtnSkill=GameObject.Instantiate(Resources.Load<GameObject>("UI/BtnSkill"), canvas, false);
            skillBtnCircle = BtnSkill.transform.Find("AOE").gameObject;
            skillBtnCircle.GetComponent<Button>().onClick.AddListener(()=> {
                if (skillCircleCd <= 0)
                {
                    skillCircleCd = 2.24f;
                    MessageCenter.Get().OnDisPatch("SkillCircle");
                    //MessageCenter.Get().OnDisPatch("SkillUse","fire",Vector3.one);
                }
                else
                {
                    Debug.Log("CD中");
                }
            });
            skillBtnShan = BtnSkill.transform.Find("Shan").gameObject;
            skillBtnShan.GetComponent<Button>().onClick.AddListener(() => {
                if (skillShanCd <= 0)
                {
                    //skillShanCd = 2.24f;
                    //MessageCenter.Get().OnDisPatch("SkillShan");
                    World.Get().hostPlayer.ShowShan();
                }
                else
                {
                    Debug.Log("CD中");
                }
            });
            skillBtnBullet = BtnSkill.transform.Find("Bullet").gameObject;
            bool flag = true;
            skillBtnBullet.GetComponent<Button>().onClick.AddListener(() => {
                if (skillBulletCd <= 0)
                {
                    if (SkillBulletType==false)
                    {
                        SkillBulletType = true;
                        World.Get().hostPlayer.ShowLine(true);
                    }
                }
                else
                {
                    Debug.Log("CD中");
                }
            });
            skillCircleCDimg = skillBtnCircle.transform.Find("Image").GetComponent<Image>(); 
            skillShanCDimg = skillBtnShan.transform.Find("Image").GetComponent<Image>();
            skillBulletCDimg = skillBtnBullet.transform.Find("Image").GetComponent<Image>();
        }
        skill.gameObject.SetActive(true);
    }
    public void CloseLoginPanel()
    {
        if (login!=null)
        {
            login.gameObject.SetActive(false);
        }
    }

    public void OpenTaskList()
    {
        if (task==null)
        {
            task = GameObject.Instantiate(Resources.Load<GameObject>("UI/TaskList"), canvas, false);
            task.AddComponent<TaskListCountroller>();
        }
        task.SetActive(true);
    }

    public void AddNewTask(TaskInfo data)
    {
        task.GetComponent<TaskListCountroller>().Add(data);
    }
    public void CompleteTask(TaskInfo data)
    {
        task.GetComponent<TaskListCountroller>().Complete(data);
    }


    public void CollectUI()
    {
        if (collectUI==null)
        {
            collectUI = GameObject.Instantiate(Resources.Load<GameObject>("UI/CollectUI"),canvas,false);
            collectUI.AddComponent<collectCountroller>();
        }
        collectUI.gameObject.SetActive(true);
    }

    public void CollectUIHide()
    {
        if (collectUI != null)
        {
            collectUI.gameObject.SetActive(false);
        }
    }
    
    public void OpenNpclist()
    {
        if (npclist==null)
        {
            npclist = GameObject.Instantiate(Resources.Load<GameObject>("UI/NPCSelect"),canvas,false);
            npclist.AddComponent<NpcSeleectManager>();
        }
        npclist.gameObject.SetActive(true);
        npclist.transform.position = Input.mousePosition;
    }
    public void OpenNpclist(Transform po)
    {
        if (npclist == null)
        {
            npclist = GameObject.Instantiate(Resources.Load<GameObject>("UI/NPCSelect"), canvas, false);
            npclist.AddComponent<NpcSeleectManager>();
        }
        npclist.gameObject.SetActive(true);
        npclist.transform.position = Camera.main.WorldToScreenPoint(po.position);
    }

    /*
     判断物体是否在屏幕上
     */
    public bool IsInView(Vector3 worldPos)
    {
        //获得游戏场景中主摄像机的Transfrom引用
        Transform camTransform = Camera.main.transform;
        //将传过来的世界坐标转化为游戏屏幕坐标
        Vector2 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        //将坐标进行规范化
        Vector3 dir = (worldPos - camTransform.position).normalized;
        //判断物体是否在相机前面
        float dot = Vector3.Dot(camTransform.forward, dir);


        if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            return true;
        else
            return false;
    }
}
