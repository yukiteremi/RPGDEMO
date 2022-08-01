using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector3 lerp;
    // Start is called before the first frame update
    void Start()
    {
        //lerp = Vector3.one -new Vector3(0,1,0);
    }
    public void Init(Vector3 lerp)
    {
        this.lerp = lerp;
        StartCoroutine(time());
    }
    IEnumerator time()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
    IEnumerator des()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (lerp!=Vector3.zero)
        {
            transform.Translate(lerp.normalized * Time.deltaTime*15);
        }
        foreach (var item in World.Get().EnemyDic.Values)
        {
            if (item.gameGo!=null)
            {
                if (Vector3.Distance(item.gameGo.transform.position,transform.position)<=0.9f)
                {
                    //Debug.Log("碰撞到了"+item.gameGo.name);
                    item.Hited(2f);
                    StopAllCoroutines();
                    gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}
