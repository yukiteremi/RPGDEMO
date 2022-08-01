using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public FSMSystem system;
    public Enemy mydata;
    public bool flag = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (system != null)
        {
            system.Update(gameObject);
        }
    }
}
