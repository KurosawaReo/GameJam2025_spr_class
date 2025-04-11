using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BlackOutNext : MonoBehaviour
{
    BlackOutMove blackOutMove;
    NextStage nextStage;
    [SerializeField] SceneTransitions scptSceneTrans;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objParent = transform.parent.gameObject;
        blackOutMove = objParent.GetComponent<BlackOutMove>();

        objParent = transform.parent.parent.gameObject;
        nextStage = objParent.GetComponent<NextStage>();

        var obj = GameObject.Find("TitleBlockPanel");
    }

    public void BlackOutEvent()
    {
        blackOutMove.blackOutObjNum++;
        blackOutMove.BlackOutObjOn();
    }

    /// <summary>
    /// �����u���b�N�A�E�g���I���������s�����.
    /// </summary>
    public void BlackOutEnd()
    {
        blackOutMove.blackOutObjEndNum++;
        //Debug.Log("blackOutObjEndNum" + blackOutMove.blackOutObjEndNum + "blackOutObj.Length" + blackOutMove.blackOutObj.Length);

        //�S�Ẵu���b�N�A�E�g�u���b�N�������Ȃ�����.
        if (blackOutMove.blackOutObjEndNum >= blackOutMove.blackOutObj.Length)
        {
            scptSceneTrans.SceneLoad(nextStage.NextSceneNum); //�V�[���ړ�.
        }
    }
}
