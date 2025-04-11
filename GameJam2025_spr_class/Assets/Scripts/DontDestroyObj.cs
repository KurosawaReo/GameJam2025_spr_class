using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObj : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] SceneTransitions scptSceneTrans; //�V�[���ړ��p.

    public ModeName mode; //�Q�[�����[�h.

    /// <summary>
    /// TimeUp���[�h�{�^������������.
    /// </summary>
    public void PushTimeUp()
    {
        mode = ModeName.TimeUp;
        scptSceneTrans.SceneLoad(2); //�Q�[���V�[����.
    }

    /// <summary>
    /// AllBreak���[�h�{�^������������.
    /// </summary>
    public void PushAllBreak()
    {
        mode = ModeName.AllBreak;
        scptSceneTrans.SceneLoad(2); //�Q�[���V�[����.
    }
}
