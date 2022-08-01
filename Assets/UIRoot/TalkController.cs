using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController 
{
    static TalkController ins;
    public static TalkController Get()
    {
        if (ins==null)
        {
            ins = new TalkController();
        }
        return ins;
    }

    public GameObject talk;
    public Text nameText, contonText;
    public List<talkContont> talklist = new List<talkContont>();
    public int index = 0;
    public string messageName=string.Empty;
    public void OpenTalkPanel()
    {
        if (talk==null)
        {
            talk = GameObject.Instantiate(Resources.Load<GameObject>("UI/talk"),UIManager.Get().canvas,false);
            nameText = talk.transform.Find("nameText").GetComponent<Text>();
            contonText = talk.transform.Find("contontText").GetComponent<Text>();
            talk.GetComponent<Button>().onClick.AddListener(() =>
            {
                Reset();
            });
        }
        talk.gameObject.SetActive(true);
    }

    public void Reset()
    {
        if (index >= talklist.Count)
        {
            talk.gameObject.SetActive(false);
            MessageCenter.Get().OnDisPatch("TalkComplete", messageName);
        }
        else
        {
            if (talklist.Count > 0)
            {
                nameText.text = talklist[index].name;
                contonText.text = talklist[index].contont;
                index++;
            }
        }
    }

    public void StartTalk(List<talkContont> talks,string taskName)
    {
        talklist = talks;
        index = 0;
        messageName = taskName;
        Reset();
    }
}
