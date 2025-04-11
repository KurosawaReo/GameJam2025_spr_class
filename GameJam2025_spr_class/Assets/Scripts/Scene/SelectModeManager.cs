using Gloval;
using UnityEngine;

public class SelectModeManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] SceneTransitions scptSceneTrans; //シーン移動用.

    DontDestroyObj scptDontDest;

    void Start()
    {
        //取得.
        scptDontDest = GameObject.Find("DontDestroyObj").GetComponent<DontDestroyObj>();
    }

    /// <summary>
    /// TimeUpモードボタンを押した時.
    /// </summary>
    public void PushTimeUp()
    {
        scptDontDest.mode = GameMode.TimeUp;
        scptSceneTrans.SceneLoad(2); //ゲームシーンへ.
    }

    /// <summary>
    /// AllBreakモードボタンを押した時.
    /// </summary>
    public void PushAllBreak()
    {
        scptDontDest.mode = GameMode.AllBreak;
        scptSceneTrans.SceneLoad(2); //ゲームシーンへ.
    }
}
