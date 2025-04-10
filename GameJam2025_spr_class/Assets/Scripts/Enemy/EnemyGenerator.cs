using System;
using System.Collections;
using UnityEngine;
using Gloval;

public class EnemyGenerator : MonoBehaviour
{
    [Tooltip("��������v���n�u"), SerializeField]
    public GameObject prefabItem;

    [Tooltip("�v���n�u������I�u�W�F�N�g"), SerializeField]
    GameObject prefabInObj;

    [Tooltip("GameManager��script"), SerializeField]
    GameManager scptGameMng;

    // �������鐔�̃J�E���g
    int cnt = 0;

    void Start()
    {
        StartCoroutine(WaitStart()); 
    }

    void Update()
    {
        
    }

    /// <summary>
    /// �X�^�[�g����܂őҋ@����p.
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitStart()
    {
        //�X�^�[�g���������܂Ń��[�v.
        while (!scptGameMng.startFlag)
        {
            yield return null;
        }

        //���s.
        StartCoroutine(EnemyGeneration(Gl_Const.INTERVAL_ITEM_GEN));
    }

    /// <summary>
    /// �����_����������
    /// </summary>
    public IEnumerator EnemyGeneration(float delay)
    {
        //�Q�[���I������܂Ń��[�v.
        while (!scptGameMng.gameOverFlag)
        {
            print("�����܂���");
            if (cnt >= Gl_Const.MAX_ENEMY_NUM)
            {
                // ���ɌĂяo���Ă���
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
