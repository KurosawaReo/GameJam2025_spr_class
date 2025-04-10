using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutMove : MonoBehaviour
{
    [SerializeField]
    public GameObject[] blackOutObj;

    Animator animator;

    public int blackOutObjNum;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        blackOutObjNum = 0;
    }

    public void StartButtonPush()
    {
        if (blackOutObjNum == 0)
        {
            BlackOutObjOn();
        }
    }

    public void BlackOutObjOn()
    {
        if (blackOutObj.Length <= blackOutObjNum) return;
        animator = blackOutObj[blackOutObjNum].GetComponent<Animator>();
        animator.SetTrigger("BlackOutOnTrigger");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
