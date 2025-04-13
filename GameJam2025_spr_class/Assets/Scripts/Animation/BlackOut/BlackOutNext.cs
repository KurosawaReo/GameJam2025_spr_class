using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutNext : MonoBehaviour
{
    BlackOutMove blackOutMove;
    //NextStage nextStage;

    [Header("- script -")]
    [SerializeField] SceneTransitions  scptSceneTrans;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objParent = transform.parent.gameObject;
        blackOutMove = objParent.GetComponent<BlackOutMove>();

        objParent = transform.parent.parent.gameObject;
        //nextStage = objParent.GetComponent<NextStage>();

        var obj = GameObject.Find("TitleBlockPanel");
    }

    public void BlackOutEvent()
    {
        blackOutMove.blackOutObjNum++;
        blackOutMove.BlackOutObjOn();
    }

    /// <summary>
    /// 多分ブラックアウトが終わったら実行される.
    /// </summary>
    public void BlackOutEnd()
    {
        blackOutMove.blackOutObjEndNum++;
        //Debug.Log("blackOutObjEndNum" + blackOutMove.blackOutObjEndNum + "blackOutObj.Length" + blackOutMove.blackOutObj.Length);

        //全てのブラックアウトブロックが黒くなったら.
        if (blackOutMove.blackOutObjEndNum >= blackOutMove.blackOutObj.Length)
        {
            var objSelModeMng  = GameObject.Find("SelectModeManager");
            var scptSelModeMng = objSelModeMng.GetComponent<SelectModeManager>();

            scptSelModeMng.SelectModeExe(); //モード選択へ.
        }
    }
}
