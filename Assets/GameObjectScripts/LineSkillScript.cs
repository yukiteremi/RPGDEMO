using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSkillScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit info;
        if (Physics.Raycast(ray,out info))
        {
            Vector3 pos = new Vector3(info.point.x, transform.position.y, info.point.z);
            transform.LookAt(pos);
            if (Input.GetMouseButtonDown(0))
            {
                if (UIManager.Get().SkillBulletType)
                {
                    MessageCenter.Get().OnDisPatch("SkillBullet", pos-transform.position);
                }
            }
        }
        
    }
}
