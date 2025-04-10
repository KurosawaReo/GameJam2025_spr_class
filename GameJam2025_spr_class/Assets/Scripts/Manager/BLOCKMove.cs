using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLOCKMove : MonoBehaviour
{
    [SerializeField]
    GameObject[] blockObjB;
    [SerializeField]
    GameObject[] blockObjL;
    [SerializeField]
    GameObject[] blockObjO;
    [SerializeField]
    GameObject[] blockObjC;
    [SerializeField]
    GameObject[] blockObjK;

    enum BLOCK
    {
        B,
        L,
        O,
        C,
        K,
    }

    BLOCK blockType;

    int randomInt;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Init()
    {
        blockType = BLOCK.B;
        SwichType();
    }

    public void AnimEnd()
    {
        SwichType();
    }

    void SwichType()
    {
        //Debug.Log("SwichType" + blockType);
        switch (blockType)
        {
            case BLOCK.B:
                RandomBlockTrigger(blockObjB);
                break;
            case BLOCK.L:
                RandomBlockTrigger(blockObjL);
                break;
            case BLOCK.O:
                RandomBlockTrigger(blockObjO);
                break;
            case BLOCK.C:
                RandomBlockTrigger(blockObjC);
                break;
            case BLOCK.K:
                RandomBlockTrigger(blockObjK);
                break;
        }

        blockType = (blockType >= BLOCK.K) ? BLOCK.B : blockType + 1;

        //if (blockType >= BLOCK.K)   {blockType = BLOCK.B;}
        //else                        {blockType++;}
    }

    void RandomBlockTrigger(GameObject[] gameObj)
    {
            randomInt = Random.Range(0, gameObj.Length);
            animator = gameObj[randomInt].GetComponent<Animator>();
            animator.SetTrigger("blockMoveTrigger");
    }
}
