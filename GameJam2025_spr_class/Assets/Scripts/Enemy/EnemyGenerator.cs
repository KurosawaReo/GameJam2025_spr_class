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

    [Tooltip("BoardManager��script"), SerializeField]
    BoardManager scptBoardMng;

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
        StartCoroutine(EnemyGeneration());
    }

    /// <summary>
    /// �����_����������
    /// </summary>
    public IEnumerator EnemyGeneration()
    {
        //�ŏ��ɉ��̂��o��.
        for (int i = 0; i < Gl_Const.START_ENEMY_NUM; i++) 
        {
            EnemySpawnExe();
            yield return new WaitForSeconds(0.1f);
        }

        //�Q�[���I������܂Ń��[�v.
        while (!scptGameMng.gameOverFlag)
        {
            //�ő�o�����ɂȂ��Ă�Ȃ�ҋ@.
            if (scptGameMng.GetEnemyCount() >= Gl_Const.MAX_ENEMY_NUM)
            {
                yield return null;
                continue;
            }

            //�x�����Ԃ̒��I.
            float delay = UnityEngine.Random.Range(
                Gl_Const.ENEMY_SPAWN_MIN_INTERVAL,
                Gl_Const.ENEMY_SPAWN_MAX_INTERVAL
            );
            yield return new WaitForSeconds(delay);

            //�G��ǉ�.
            EnemySpawnExe();
        }
    }

    /// <summary>
    /// �G���o��.
    /// </summary>
    public void EnemySpawnExe()
    {
        //�o�����W���I.
        int rndX = UnityEngine.Random.Range(0, Gl_Const.BOARD_WID - 1);
        int rndY = UnityEngine.Random.Range(0, Gl_Const.BOARD_HEI - 1);

        //���̍��W�����}�X�Ȃ�.
        if (scptBoardMng.Board[rndX, rndY].type == BoardType.NONE)
        {
            //�G�o��.
            var obj = Instantiate(prefabItem, prefabInObj.transform);
            //���W�𒊑I���Ĕz�u.
            obj.transform.position = Gl_Func.BPosToWPos(new Vector2Int(rndX, rndY));
        }
    }

    /// <summary>
    /// �G�̃I�u�W�F�N�g���擾���鏈��
    /// </summary>
    public GameObject[] GetEnemyObjects()
    {
        // �^�O�uEnemy�v�����S�ẴI�u�W�F�N�g�̎擾
        GameObject[] Square = GameObject.FindGameObjectsWithTag("Enemy");

        return Square;
    }
}
