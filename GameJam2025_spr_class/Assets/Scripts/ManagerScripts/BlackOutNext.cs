using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutNext : MonoBehaviour
{
    BlackOutMove blackOutMove;
    SceneTransitions transitions;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objParent = transform.parent.gameObject;
        blackOutMove = objParent.GetComponent<BlackOutMove>();

        var obj = GameObject.Find("TitleBlockPanel");
    }

    void BlackOutEvent()
    {
        blackOutMove.blackOutObjNum++;
        blackOutMove.BlackOutObjOn();
    }

    void BlackOutEnd()
    {
        if(blackOutMove.blackOutObjNum >= blackOutMove.blackOutObj.Length)
        {
            //SceneTransitions.SceneLoad();
        }    
    }
}
