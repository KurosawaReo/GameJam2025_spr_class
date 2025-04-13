using Gloval;
using UnityEngine;

public class SelectModeManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] SceneTransitions scptSceneTrans; //シーン移動用.

    [Header("- panel -")]
    [SerializeField] GameObject panelSelectMode;

    DontDestroyObj scptDontDest;

    void Start()
    {
        //取得.
        scptDontDest = GameObject.Find("DontDestroyObj").GetComponent<DontDestroyObj>();
    }

    /// <summary>
    /// モード選択を行う.
    /// </summary>
    public void SelectModeExe()
    {
        panelSelectMode.SetActive(true); //表示.
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

    /// <summary>
    /// AllFillモードボタンを押した時.
    /// </summary>
    public void PushAllFill()
    {
        scptDontDest.mode = GameMode.AllFill;
        scptSceneTrans.SceneLoad(2); //ゲームシーンへ.
    }
}
