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

    [Tooltip("BoardManagerのscript"), SerializeField]
    BoardManager scptBoardMng;

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

        //モード別で出現実行.
        switch (scptGameMng.gameMode) {

            case GameMode.TimeUp:
                StartCoroutine(EnmSpawnTimeUp());
                break;
        
            case GameMode.AllBreak:
                StartCoroutine(EnmSpawnAllBreak());
                break;
        }
    }

    /// <summary>
    /// ランダム生成処理(TimeUpモード)
    /// </summary>
    public IEnumerator EnmSpawnTimeUp()
    {
        //最初に何体か出す.
        for (int i = 0; i < Gl_Const.ENM_TIMEUP_INIT_CNT; i++) 
        {
            EnemySpawnExe();
            yield return new WaitForSeconds(0.1f);
        }

        //ゲーム終了するまでループ.
        while (!scptGameMng.gameOverFlag)
        {
            //最大出現数になってるなら待機.
            if (scptGameMng.GetEnemyCount() >= Gl_Const.ENM_TIMEUP_MAX_CNT)
            {
                yield return null;
                continue;
            }

            //遅延時間の抽選.
            float delay = UnityEngine.Random.Range(
                Gl_Const.ENM_TIMEUP_MIN_INTERVAL,
                Gl_Const.ENM_TIMEUP_MAX_INTERVAL
            );
            yield return new WaitForSeconds(delay);

            //敵を追加.
            EnemySpawnExe();
        }
    }

    /// <summary>
    /// ランダム生成処理(AllBreakモード)
    /// </summary>
    public IEnumerator EnmSpawnAllBreak()
    {
        yield return new WaitForSeconds(3);

        //最初に何体か出す.
        for (int i = 0; i < Gl_Const.ENM_ALLBREAK_MAX_CNT; i++)
        {
            EnemySpawnExe();
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 敵を出現.
    /// </summary>
    public void EnemySpawnExe()
    {
        //出現座標抽選.
        int rndX = UnityEngine.Random.Range(0, Gl_Const.BOARD_WID - 1);
        int rndY = UnityEngine.Random.Range(0, Gl_Const.BOARD_HEI - 1);

        //その座標が無マスなら.
        if (scptBoardMng.Board[rndX, rndY].type == BoardType.NONE)
        {
            //敵出現.
            var obj = Instantiate(prefabItem, prefabInObj.transform);
            //座標を抽選して配置.
            obj.transform.position = Gl_Func.BPosToWPos(new Vector2Int(rndX, rndY));
        }
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
