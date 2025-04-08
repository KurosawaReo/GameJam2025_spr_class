using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
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
        StartCoroutine(EnemyGeneration(Common.INTERVAL_ITEM_GEN));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ランダム生成処理
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator EnemyGeneration(float delay)
    {
        var (lb, rt) = Common.GetWorldWindowSize();

        while (true)
        {
            if (cnt >= Common.MAX_ITEM_NUM)
            {
                yield return new WaitForSeconds(delay);
                continue;

            }

            // 範囲指定
            float x = UnityEngine.Random.Range(lb.x + Common.MARGIN_LEFT, rt.x - Common.MARGIN_RIGHT);
            float y = UnityEngine.Random.Range(lb.y + Common.MARGIN_BOTTOM, rt.y - Common.MARGIN_TOP);

            transform.position = new Vector3(x, y, transform.position.z);

            cnt++;

            yield return new WaitForSeconds(delay);

        }
    }
}
