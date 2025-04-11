using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutMove : MonoBehaviour
{
    [SerializeField]
    public GameObject[] blackOutObj;

    NextStage nextStage;

    Animator animator;

    public int blackOutObjNum;  //動き出してるのは何個目か

    public int blackOutObjEndNum;            //動き終えたのは何個目か

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        blackOutObjNum = 0;

        GameObject objParent = transform.parent.gameObject;
        nextStage = objParent.GetComponent<NextStage>();
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
        ButtonOn();
    }

    void ButtonOn()
    {
        if(nextStage.onBool == true)
        if(Input.GetKeyDown(KeyCode.Space))
        {
            BlackOutObjOn();
        }

        // コントローラー
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            BlackOutObjOn();
        }
    }
}
