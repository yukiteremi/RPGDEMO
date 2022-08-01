using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHUD : MonoBehaviour
{
    public static UIHUD Create(GameObject game,float maxHp)
    {
        GameObject hpbar = GameObject.Instantiate(Resources.Load<GameObject>("UI/PlayerHUD"),UIManager.Get().canvas,false);
        UIHUD hud= hpbar.AddComponent<UIHUD>();
        hud.targetGo = game;
        hud.hpbar = hpbar.transform.Find("hpbar").GetComponent<Slider>();
        hud.mpbar = hpbar.transform.Find("mpbar").GetComponent<Slider>();
        hud.nameText = hpbar.transform.Find("Text").GetComponent<Text>();
        hud.maxhp = maxHp;
        hud.nowHp = maxHp;
        hud.collectList = hpbar.transform.Find("Collect");
        return hud;
    }
    public GameObject targetGo;
    public Slider hpbar, mpbar;
    public Text nameText;
    public Transform collectList;
    public float maxhp, nowHp;
    int nums = 0;
    public List<GameObject> list = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CollectInit(int num)
    {
        GameObject clone = Resources.Load<GameObject>("UI/collectImage");
        for (int i = 0; i < num; i++)
        {
            list.Add(GameObject.Instantiate(clone,collectList,false));
        }
    }

    

    public void CollectChange()
    {
        GameObject.Destroy(list[list.Count-1].gameObject);
        list.RemoveAt(list.Count-1);
        if (list.Count==0)
        {
            MessageCenter.Get().OnDisPatch("DestroyCollect",0);
        }
    }

    public void Init(string UIname,int flag)
    {
        nameText.text = UIname;
        nameText.gameObject.SetActive(true);
        if (flag==0) //怪
        {
            collectList.gameObject.SetActive(false);
            hpbar.gameObject.SetActive(true);
            mpbar.gameObject.SetActive(true);
        }
        else if(flag == 1) //npc
        {
            collectList.gameObject.SetActive(false);
            hpbar.gameObject.SetActive(false);
            mpbar.gameObject.SetActive(false);
        }
        else  //采集物
        {
            collectList.gameObject.SetActive(true);
            hpbar.gameObject.SetActive(false);
            mpbar.gameObject.SetActive(false);
        }
        nums = flag;
    }

    public void HPchange(float nowHp,float maxHp,float demage)
    {
        this.nowHp = nowHp;
        this.maxhp = maxHp;
        hpbar.value = nowHp / maxHp;
        UIlog.Create(gameObject,demage);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetGo)
        {
            if (UIManager.Get().IsInView(targetGo.transform.position))
            {
                transform.position = Camera.main.WorldToScreenPoint(targetGo.transform.position + Vector3.up * 2);
                Init(nameText.text, nums);
            }
            else
            {
                nameText.gameObject.SetActive(false);
                collectList.gameObject.SetActive(false);
                hpbar.gameObject.SetActive(false);
                mpbar.gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
