using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gloval;
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
        StartCoroutine(EnemyGeneration(Gl_Const.INTERVAL_ITEM_GEN));
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
        var (lb, rt) = Gl_Func.GetWorldWindowSize();

        while (true)
        {
            print("�����܂���");
            if (cnt >= Gl_Const.MAX_ITEM_NUM)
            {
                // ���ɌĂяo���Ă���
                //GetEnemyObjects();

                yield return new WaitForSeconds(delay);
                continue;

            }
            
            // �͈͎w��
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
    /// �G�̃I�u�W�F�N�g���擾���鏈��
    /// </summary>
    public GameObject[] GetEnemyObjects()
    {
        // �^�O�uEnemy�v�����S�ẴI�u�W�F�N�g�̎擾
        GameObject[] Square = GameObject.FindGameObjectsWithTag("Enemy");

        // �擾�����G�I�u�W�F�N�g�̖��O��\��
        //foreach (GameObject enemy in Square)
        //{
        //    Debug.Log("�V�[�����̓G: " + enemy.name);
        //}

        return Square;
    }

    
}
