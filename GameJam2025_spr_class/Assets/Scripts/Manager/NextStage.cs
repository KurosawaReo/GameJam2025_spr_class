using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブラックアウトのアニメーション.
/// </summary>
public class NextStage : MonoBehaviour
{
    [SerializeField]
    public int StageNum = 1;

    [SerializeField]
    public bool onBool; //trueなら、SPACEを押すとアニメーション再生.
}
