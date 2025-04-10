using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gloval;
using static UnityEditor.Progress;

public class EnemyGenerator : MonoBehaviour
{
    
    [Tooltip("生成するプレハブ"), SerializeField]
    public GameObject prefabItem;

    // 生成する数のカウント
    int cnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyGeneration(Gl_Const.INTERVAL_ITEM_GEN));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ランダム生成処理
    /// </summary>
    public IEnumerator EnemyGeneration(float delay)
    {
        var (lb, rt) = Gl_Func.GetWorldWindowSize();

        while (true)
        {
            print("動きました");
            if (cnt >= Gl_Const.MAX_ITEM_NUM)
            {
                // 仮に呼び出している
                //GetEnemyObjects();

                yield return new WaitForSeconds(delay);
                continue;

            }
            
            // 範囲指定
            float x = UnityEngine.Random.Range(lb.x + Gl_Const.MARGIN_LEFT + 2, rt.x - Gl_Const.MARGIN_RIGHT - 2);
            float y = UnityEngine.Random.Range(lb.y + Gl_Const.MARGIN_BOTTOM + 2, rt.y - Gl_Const.MARGIN_TOP - 2);

            var prefab = prefabItem;

            var obj = Instantiate(prefab, transform);

            obj.transform.position = new Vector3(x, y, obj.transform.position.z);

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
