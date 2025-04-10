using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BlackOutNext : MonoBehaviour
{
    BlackOutMove blackOutMove;
    NextStage nextStage;
    [SerializeField] SceneTransitions transitions;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objParent = transform.parent.gameObject;
        blackOutMove = objParent.GetComponent<BlackOutMove>();

        objParent = transform.parent.parent.gameObject;
        nextStage = objParent.GetComponent<NextStage>();

        var obj = GameObject.Find("TitleBlockPanel");
    }

    void BlackOutEvent()
    {
        blackOutMove.blackOutObjNum++;
        blackOutMove.BlackOutObjOn();
    }

    void BlackOutEnd()
    {
        blackOutMove.blackOutObjEndNum++;
        Debug.Log("blackOutObjEndNum" + blackOutMove.blackOutObjEndNum + "blackOutObj.Length" + blackOutMove.blackOutObj.Length);
        if (blackOutMove.blackOutObjEndNum >= blackOutMove.blackOutObj.Length)
        {
            transitions.SceneLoad(nextStage.StageNum);
        }    
    }
}
