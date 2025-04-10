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
        //すでにロード済みなら、二重に作成しない
        if (Loaded) return;
        Loaded = true;

        if (dontDestroyObjs != null)
        {
            //プレハブをインスタンス化して、DontDestroyOnLoad
            foreach (var objs in dontDestroyObjs)
            {
                GameObject prefab = Instantiate(objs);
                DontDestroyOnLoad(prefab);
            }
        }
    }

}
