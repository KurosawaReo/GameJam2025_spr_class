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
        //すでにロード済みなら、二重に作成しない
        if (Loaded) return;
        Loaded = true;

        if (gameManagerPrefabs != null)
        {
            //プレハブをインスタンス化して、DontDestroyOnLoad
            foreach (var prefab in gameManagerPrefabs)
            {
                GameObject manager = Instantiate(prefab);
                DontDestroyOnLoad(manager);
            }
        }
    }

}
