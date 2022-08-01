using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipsController
{
    static TipsController ins;
    public static TipsController Get()
    {
        if (ins == null)
        {
            ins = new TipsController();
        }
        return ins;
    }

    Button btn;
    Text text;
    GameObject tip;

    public void Open(string str)
    {
        if (tip == null)
        {
            GameObject clone = Resources.Load<GameObject>("UI/Tip");
            tip = GameObject.Instantiate(clone,UIManager.Get().canvas,false);
            text = tip.transform.Find("ShowText").GetComponent<Text>();
            btn = tip.transform.Find("Close").GetComponent<Button>();
            btn.onClick.AddListener(()=> {
                tip.SetActive(false);
                MessageCenter.Get().OnDisPatch("CloseButtonClick", DataManager.Get().nowtype);
            });
        }
        tip.SetActive(true);
        text.text = str;
    }

    
    

}
