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
        StartCoroutine(EnemyGeneration());
    }

    /// <summary>
    /// ランダム生成処理
    /// </summary>
    public IEnumerator EnemyGeneration()
    {
        //最初に何体か出す.
        for (int i = 0; i < Gl_Const.START_ENEMY_NUM; i++) 
        {
            EnemySpawnExe();
            yield return new WaitForSeconds(0.1f);
        }

        //ゲーム終了するまでループ.
        while (!scptGameMng.gameOverFlag)
        {
            //最大出現数になってるなら待機.
            if (scptGameMng.GetEnemyCount() >= Gl_Const.MAX_ENEMY_NUM)
            {
                yield return null;
                continue;
            }

            //遅延時間の抽選.
            float delay = UnityEngine.Random.Range(
                Gl_Const.ENEMY_SPAWN_MIN_INTERVAL,
                Gl_Const.ENEMY_SPAWN_MAX_INTERVAL
            );
            yield return new WaitForSeconds(delay);

            //敵を追加.
            EnemySpawnExe();
        }
    }

    /// <summary>
    /// 敵を出現.
    /// </summary>
    public void EnemySpawnExe()
    {
        //敵出現.
        var obj = Instantiate(prefabItem, prefabInObj.transform);
        //座標を抽選して配置.
        obj.transform.position = Gl_Func.RandEnemySpawnPos();
    }

    /// <summary>
    /// 敵のオブジェクトを取得する処理
    /// </summary>
    public GameObject[] GetEnemyObjects()
    {
        // タグ「Enemy」を持つ全てのオブジェクトの取得
        GameObject[] Square = GameObject.FindGameObjectsWithTag("Enemy");

        return Square;
    }
}
