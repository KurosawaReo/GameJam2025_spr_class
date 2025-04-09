using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using static UnityEditor.Progress;

public class EnemyGenerator : MonoBehaviour
{
    
    [Tooltip("��������v���n�u"), SerializeField]
    public GameObject prefabItem;
    

    // �������鐔�̃J�E���g
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
    /// �����_����������
    /// </summary>
    public IEnumerator EnemyGeneration(float delay)
    {
        var (lb, rt) = Common.GetWorldWindowSize();

        while (true)
        {
            print("�����܂���");
            if (cnt >= Common.MAX_ITEM_NUM)
            {
                yield return new WaitForSeconds(delay);
                continue;

            }

            // �͈͎w��
            float x = UnityEngine.Random.Range(lb.x + Common.MARGIN_LEFT + 2, rt.x - Common.MARGIN_RIGHT - 2);
            float y = UnityEngine.Random.Range(lb.y + Common.MARGIN_BOTTOM + 2, rt.y - Common.MARGIN_TOP - 2);

            var prefab = prefabItem;

            var obj = Instantiate(prefab);

            obj.transform.position = new Vector3(x, y, obj.transform.position.z);

            cnt++;

            yield return new WaitForSeconds(delay);
            
            
        }
    }

    
}
