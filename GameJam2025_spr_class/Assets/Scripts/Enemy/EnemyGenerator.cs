using System;
using System.Collections;
using UnityEngine;
using Gloval;
using Unity.VisualScripting;

public class EnemyGenerator : MonoBehaviour
{
    [Tooltip("生成するプレハブ"), SerializeField]
    GameObject prefabItem;

    [Tooltip("プレハブを入れるオブジェクト"), SerializeField]
    GameObject prefabInObj;

    [Tooltip("GameManagerのscript"), SerializeField]
    GameManager scptGameMng;

    [Tooltip("BoardManagerのscript"), SerializeField]
    BoardManager scptBoardMng;

    /// <summary>
    /// ランダム生成処理(TimeUpモード)
    /// </summary>
    public IEnumerator EnmSpawnNormal()
    {
        yield return new WaitForSeconds(3); //3秒の猶予.

        //ゲーム終了していなければ.
        if (!scptGameMng.isGameEnd)
        {
            //最初に何体か出す.
            for (int i = 0; i < Gl_Const.ENM_TIMEUP_INIT_CNT; i++) 
            {
                EnemySpawnExe();
                yield return new WaitForSeconds(0.1f);
            }
        }
        //ゲーム終了するまでループ.
        while (!scptGameMng.isGameEnd)
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
        yield return new WaitForSeconds(3); //3秒の猶予.
     
        //ゲーム終了していなければ.
        if (!scptGameMng.isGameEnd)
        {
            //一気に出す.
            for (int i = 0; i < Gl_Const.ENM_ALLBREAK_MAX_CNT; i++)
            {
                EnemySpawnExe();
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    /// <summary>
    /// 敵を出現.
    /// </summary>
    public void EnemySpawnExe()
    {
        //出現座標を抽選.
        Vector2Int rnd = Gl_Func.RndBPosOutside(30);

        //その座標が無マスなら.
        if (scptBoardMng.Board[rnd.x, rnd.y].type == BoardType.NONE)
        {
            //敵出現.
            var obj = Instantiate(prefabItem, prefabInObj.transform);
            //座標を抽選して配置.
            obj.transform.position = Gl_Func.BPosToWPos(new Vector2Int(rnd.x, rnd.y));
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
