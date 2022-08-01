using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public Transform effectPoint;
    public AnimationClip ani;
    AnimatorOverrideController animatorC;

    public Queue<AnimationClip> AniQue = new Queue<AnimationClip>();
    public Queue<GameObject> GoQue = new Queue<GameObject>();
    public Queue<float> AniTimeQue = new Queue<float>();
    public Queue<float> GoTimeQue = new Queue<float>();


    public Queue<Vector3> posQue = new Queue<Vector3>();
    public Queue<Transform> transQue = new Queue<Transform>();

    public bool autoMove = false;
    public bool nextquestion = false;
    public HostPlayer playerhost;
    Transform target;
    Vector3 norV3;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        effectPoint = transform.Find("effectPoint");
        animatorC = new AnimatorOverrideController();
        animatorC.runtimeAnimatorController = animator.runtimeAnimatorController;
        animator.runtimeAnimatorController = animatorC;
        norV3 = Camera.main.transform.position - transform.position;
        StartCoroutine(Ani());
        StartCoroutine(GO());
        MessageCenter.Get().OnAddListen("SkillSimple",SimpleSkill);
        
    }

    private void SimpleSkill(object obj)
    {
        animator.SetTrigger("Attack");
        if (World.Get().target!=null)
        {
            if (World.Get().target is Enemy)
            {
                if (Vector3.Distance(transform.position,World.Get().target.gameGo.transform.position)<=2)
                {
                    Enemy ene = World.Get().target as Enemy;
                    ene.Hited(2f);
                }
            }
        }
    }

    public void Init(HostPlayer playerhost)
    {
        this.playerhost = playerhost;
    }    
    /*
      animatorC["RunAttF"] = AniQue.Dequeue();
      animator.SetTrigger("Start");
     */
    public void moveTo(Transform trans)
    {
        autoMove = true;
        target = trans;
    }

    public void MoveOn(bool flag)
    {
        animator.SetBool("Move",flag);
    }

    public void AniAdd(AnimationClip clip,float time)
    {
        AniQue.Enqueue(clip);
        AniTimeQue.Enqueue(time);
    }
    public void EffAdd(GameObject clip, float time)
    {
        GoQue.Enqueue(clip);
        GoTimeQue.Enqueue(time);
        posQue.Enqueue(effectPoint.position);
    }
    public void EffAdd(GameObject clip, float time,Vector3 pos)
    {
        GoQue.Enqueue(clip);
        GoTimeQue.Enqueue(time);
        posQue.Enqueue(pos);
    }
    public void EffAdd(GameObject clip, float time, Vector3 pos,Transform trans)
    {
        GoQue.Enqueue(clip);
        GoTimeQue.Enqueue(time);
        posQue.Enqueue(pos);
        transQue.Enqueue(trans);
    }
    public void bgmAdd(AudioClip clip, float time)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void StopAll()
    {
        audioSource.Stop();
    }

    
    IEnumerator Ani()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (AniTimeQue.Count > 0)
            {
                yield return new WaitForSeconds(AniTimeQue.Dequeue());
                if (AniQue.Count>0)
                {
                    animatorC["RunAttF"] = AniQue.Dequeue();
                    animator.SetTrigger("Start");
                }
            }
        }
    }
    IEnumerator GO()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (GoTimeQue.Count > 0)
            {
                yield return new WaitForSeconds(GoTimeQue.Dequeue());
                if (GoQue.Count > 0)
                {
                    GameObject eff = Instantiate(GoQue.Dequeue());
                    if (transQue.Count>0)
                    {
                        eff.transform.SetParent(transQue.Dequeue());
                        eff.transform.LookAt(eff.transform.parent.forward);
                    }
                    eff.transform.localPosition = posQue.Dequeue();
                    Destroy(eff, 5);
                }
            }
        }
    }

    private void Update()
    {
        if (playerhost.ShanIsOn)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out RaycastHit info))
            {
                transform.LookAt(new Vector3(info.point.x, playerhost.skillShan.transform.position.y, info.point.z));
                playerhost.skillShan.transform.LookAt(new Vector3(info.point.x, playerhost.skillShan.transform.position.y, info.point.z));
            }
            if (Input.GetMouseButtonDown(0))
            {
                playerhost.SkillUseByName("shan",Vector3.zero,effectPoint.transform);
                playerhost.HideShan();
                MessageCenter.Get().OnDisPatch("SkillShan");
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit info))
            {
                if (info.collider.tag == "NPC")
                {
                    if (Vector3.Distance(transform.position, info.collider.transform.position) <= 4)
                    {
                        UIManager.Get().OpenNpclist();
                    }
                }
            }
        }
        if (autoMove)
        {
            MoveOn(true);
            gameObject.transform.LookAt(new Vector3(target.position.x,transform.position.y,target.position.z));
            gameObject.transform.Translate(Vector3.forward * 3 * Time.deltaTime);
            Camera.main.transform.position = norV3 + transform.position;
            if (Vector3.Distance(transform.position, target.position) <= 1)
            {
                autoMove = false;
                MoveOn(false);
                if (target.tag == "NPC")
                {
                    //MessageCenter.Get().OnDisPatch("AutoMoveComplete",0);
                    UIManager.Get().OpenNpclist(target);
                }
            }
        }
        bool flags = true;
        foreach (var item in World.Get().CollectDic.Values)
        {
            if (Vector3.Distance(transform.position,item.gameGo.transform.position)<=3)
            {
                UIManager.Get().CollectUI();
                World.Get().collectTarget = item;
                flags = false;
                return;
            }
        }
        if (flags)
        {
            UIManager.Get().CollectUIHide();
        }
    }
}
