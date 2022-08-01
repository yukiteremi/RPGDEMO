using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class collectCountroller : MonoBehaviour
{
    public Slider slide;
    public Button btn;
    bool starts=false;
    // Start is called before the first frame update
    void Start()
    {
        btn = gameObject.transform.Find("Button").GetComponent<Button>();
        slide = gameObject.transform.Find("Slider").GetComponent<Slider>();
        btn.onClick.AddListener(()=> {
            starts = true;
            slide.gameObject.SetActive(true);
            slide.value = 0;
        });
    }

    

    // Update is called once per frame
    void Update()
    {
        if (starts)
        {
            slide.value += 0.05f;
            if (slide.value>=1)
            {
                starts = false;
                slide.gameObject.SetActive(false);
                MessageCenter.Get().OnDisPatch("CollectComplete",0);
            }
        }
    }
}
