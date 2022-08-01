using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NpcSeleectManager : MonoBehaviour
{
    public Button btn1, btn2;
    // Start is called before the first frame update
    void Start()
    {
        btn1 = gameObject.GetComponent<ScrollRect>().content.transform.Find("Button").GetComponent<Button>();
        btn2 = gameObject.GetComponent<ScrollRect>().content.transform.Find("Button (1)").GetComponent<Button>();

        btn1.onClick.AddListener(()=> {
            MessageCenter.Get().OnDisPatch("BtnOnclick",0);
            gameObject.SetActive(false);
        });
        btn2.onClick.AddListener(() => {
            MessageCenter.Get().OnDisPatch("BtnOnclick", 1);
            gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
