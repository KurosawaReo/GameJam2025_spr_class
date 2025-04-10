using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAnimEvent : MonoBehaviour
{
    BLOCKMove blockMove;

    Animator animator;

    void Start()
    {
        Init();
    }

    void DebugInit()
    {
        
    }

    void Init()
    {
        var obj = GameObject.Find("TitleBlockPanel");
        blockMove = obj.GetComponent<BLOCKMove>();
    }

    void MoveEvent()
    {
        animator = GetComponent<Animator>();
        blockMove.AnimEnd();
    }
}
