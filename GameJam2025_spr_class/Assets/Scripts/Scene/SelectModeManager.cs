using Gloval;
using UnityEngine;

public class SelectModeManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] SceneTransitions scptSceneTrans; //�V�[���ړ��p.

    DontDestroyObj scptDontDest;

    void Start()
    {
        //�擾.
        scptDontDest = GameObject.Find("DontDestroyObj").GetComponent<DontDestroyObj>();
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
}
