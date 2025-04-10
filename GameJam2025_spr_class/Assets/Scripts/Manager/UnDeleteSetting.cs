using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class UnDeleteSetting : MonoBehaviour
{
    private static bool Loaded;

    [Header("- object -")]
    [SerializeField] GameObject[] dontDestroyObjs = null;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        //���łɃ��[�h�ς݂Ȃ�A��d�ɍ쐬���Ȃ�
        if (Loaded) return;
        Loaded = true;

        if (dontDestroyObjs != null)
        {
            //�v���n�u���C���X�^���X�����āADontDestroyOnLoad
            foreach (var objs in dontDestroyObjs)
            {
                GameObject prefab = Instantiate(objs);
                DontDestroyOnLoad(prefab);
            }
        }
    }

}
