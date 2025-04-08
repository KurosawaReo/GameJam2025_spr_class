using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCreate : MonoBehaviour
{
    private static bool Loaded;

    [SerializeField]
    GameObject[] gameManagerPrefabs = null;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        //���łɃ��[�h�ς݂Ȃ�A��d�ɍ쐬���Ȃ�
        if (Loaded) return;
        Loaded = true;

        //�v���n�u���C���X�^���X�����āADontDestroyOnLoad
        foreach (var prefab in gameManagerPrefabs)
        {
            GameObject manager = Instantiate(prefab);
            DontDestroyOnLoad(manager);
        }
    }

}
