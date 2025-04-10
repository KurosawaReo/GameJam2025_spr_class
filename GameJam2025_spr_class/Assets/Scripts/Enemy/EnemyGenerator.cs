using System;
using System.Collections;
using UnityEngine;
using Gloval;

public class EnemyGenerator : MonoBehaviour
{
    [Tooltip("生成するプレハブ"), SerializeField]
    public GameObject prefabItem;

    [Tooltip("プレハブを入れるオブジェクト"), SerializeField]
    GameObject prefabInObj;

    [Tooltip("GameManagerのscript"), SerializeField]
    GameManager scptGameMng;

    // 生成する数のカウント
    int cnt = 0;

    void Start()
    {
        StartCoroutine(WaitStart()); 
    }

    void Update()
    {
        
    }

    /// <summary>
    /// スタートするまで待機する用.
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitStart()
    {
        //スタートを押されるまでループ.
        while (!scptGameMng.startFlag)
        {
            yield return null;
        }

        //実行.
        StartCoroutine(EnemyGeneration(Gl_Const.INTERVAL_ITEM_GEN));
    }

    /// <summary>
    /// ランダム生成処理
    /// </summary>
    public IEnumerator EnemyGeneration(float delay)
    {
        //ゲーム終了するまでループ.
        while (!scptGameMng.gameOverFlag)
        {
            print("動きました");
            if (cnt >= Gl_Const.MAX_ENEMY_NUM)
            {
                // 仮に呼び出している
                //GetEnemyObjects();

                yield return new WaitForSeconds(delay);
                continue;

            }
            
            var prefab = prefabItem;

            var obj = Instantiate(prefab, prefabInObj.transform);

            obj.transform.position = Gl_Func.RandEnemySpawnPos();

            cnt++;

            yield return new WaitForSeconds(delay);
        }
    }

    /// <summary>
    /// 敵のオブジェクトを取得する処理
    /// </summary>
    public GameObject[] GetEnemyObjects()
    {
        // タグ「Enemy」を持つ全てのオブジェクトの取得
        GameObject[] Square = GameObject.FindGameObjectsWithTag("Enemy");

        // 取得した敵オブジェクトの名前を表示
        //foreach (GameObject enemy in Square)
        //{
        //    Debug.Log("シーン内の敵: " + enemy.name);
        //}

        return Square;
    }
}
