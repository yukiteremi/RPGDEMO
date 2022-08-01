using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIlog : MonoBehaviour
{
    public static void Create(GameObject game,float demage)
    {
        GameObject hpbar = GameObject.Instantiate(Resources.Load<GameObject>("UI/uilog"), UIManager.Get().canvas, false);
        UIlog hplog = hpbar.AddComponent<UIlog>();
        hpbar.transform.position = game.transform.position;
        hplog.text = hpbar.GetComponent<Text>();
        if (demage>=0)
        {
            hplog.text.text = "+" + demage;
            hplog.text.color = Color.green;
        }
        else
        {
            hplog.text.text = "-" + demage;
            hplog.text.color = Color.red;
        }
    }
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up;
    }
}
