using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObj : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] SceneTransitions scptSceneTrans; //シーン移動用.

    public ModeName mode; //ゲームモード.

    /// <summary>
    /// TimeUpモードボタンを押した時.
    /// </summary>
    public void PushTimeUp()
    {
        mode = ModeName.TimeUp;
        scptSceneTrans.SceneLoad(2); //ゲームシーンへ.
    }

    /// <summary>
    /// AllBreakモードボタンを押した時.
    /// </summary>
    public void PushAllBreak()
    {
        mode = ModeName.AllBreak;
        scptSceneTrans.SceneLoad(2); //ゲームシーンへ.
    }
}
