using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブラックアウトのアニメーション.
/// </summary>
public class NextStage : MonoBehaviour
{
    [SerializeField]
    public int NextSceneNum = 1; //この番号のシーンへ移動.

    [SerializeField]
    public bool onBool; //trueなら、SPACEを押すとアニメーション再生.
}
