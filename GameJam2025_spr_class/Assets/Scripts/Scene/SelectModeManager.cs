using Gloval;
using UnityEngine;

public class SelectModeManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] SceneTransitions scptSceneTrans; //�V�[���ړ��p.

    [Header("- panel -")]
    [SerializeField] GameObject panelSelectMode;

    DontDestroyObj scptDontDest;

    void Start()
    {
        //�擾.
        scptDontDest = GameObject.Find("DontDestroyObj").GetComponent<DontDestroyObj>();
    }

    /// <summary>
    /// ���[�h�I�����s��.
    /// </summary>
    public void SelectModeExe()
    {
        panelSelectMode.SetActive(true); //�\��.
    }

    /// <summary>
    /// TimeUp���[�h�{�^������������.
    /// </summary>
    public void PushTimeUp()
    {
        scptDontDest.mode = GameMode.TimeUp;
        scptSceneTrans.SceneLoad(2); //�Q�[���V�[����.
    }

    /// <summary>
    /// AllBreak���[�h�{�^������������.
    /// </summary>
    public void PushAllBreak()
    {
        scptDontDest.mode = GameMode.AllBreak;
        scptSceneTrans.SceneLoad(2); //�Q�[���V�[����.
    }

    /// <summary>
    /// AllFill���[�h�{�^������������.
    /// </summary>
    public void PushAllFill()
    {
        scptDontDest.mode = GameMode.AllFill;
        scptSceneTrans.SceneLoad(2); //�Q�[���V�[����.
    }
}
