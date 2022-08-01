using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Joycon : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public bool isMove;
    public Vector2 pos, startPos;
    public Animator ani;
    public void OnBeginDrag(PointerEventData eventData)
    {
        isMove = true;
        World.Get().hostPlayer.player.autoMove = false;
        World.Get().hostPlayer.player.MoveOn(isMove);
        startPos = this.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            pos = eventData.position - startPos;
            transform.position = Vector2.ClampMagnitude(pos, 70) + startPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isMove = false;
        World.Get().hostPlayer.player.MoveOn(isMove);
        transform.localPosition = Vector2.zero;
    }
    public Vector3 norV3;
    private void Start()
    {
        norV3 = Camera.main.transform.position - World.Get().hostPlayer.gameGo.transform.position;
    }
    private void Update()
    {
        if (isMove)
        {
            World.Get().hostPlayer.gameGo.transform.LookAt(new Vector3(pos.x, 0, pos.y) + World.Get().hostPlayer.gameGo.transform.position);
            World.Get().hostPlayer.gameGo.transform.Translate(new Vector3(0, 0, Time.deltaTime * 5f));
                Camera.main.transform.position = norV3 + World.Get().hostPlayer.gameGo.transform.position;
        }
    }
}
