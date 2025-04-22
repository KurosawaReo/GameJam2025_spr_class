using System;
using System.Collections;
using UnityEngine;
using Gloval;
using Unity.VisualScripting;

public class EnemyGenerator : MonoBehaviour
{
    [Tooltip("��������v���n�u"), SerializeField]
    GameObject prefabItem;

    [Tooltip("�v���n�u������I�u�W�F�N�g"), SerializeField]
    GameObject prefabInObj;

    [Tooltip("GameManager��script"), SerializeField]
    GameManager scptGameMng;

    [Tooltip("BoardManager��script"), SerializeField]
    BoardManager scptBoardMng;

    /// <summary>
    /// �����_����������(TimeUp���[�h)
    /// </summary>
    public IEnumerator EnmSpawnNormal()
    {
        yield return new WaitForSeconds(3); //3�b�̗P�\.

        //�Q�[���I�����Ă��Ȃ����.
        if (!scptGameMng.isGameEnd)
        {
            //�ŏ��ɉ��̂��o��.
            for (int i = 0; i < Gl_Const.ENM_TIMEUP_INIT_CNT; i++) 
            {
                EnemySpawnExe();
                yield return new WaitForSeconds(0.1f);
            }
        }
        //�Q�[���I������܂Ń��[�v.
        while (!scptGameMng.isGameEnd)
        {
            //�ő�o�����ɂȂ��Ă�Ȃ�ҋ@.
            if (scptGameMng.GetEnemyCount() >= Gl_Const.ENM_TIMEUP_MAX_CNT)
            {
                yield return null;
                continue;
            }

            //�x�����Ԃ̒��I.
            float delay = UnityEngine.Random.Range(
                Gl_Const.ENM_TIMEUP_MIN_INTERVAL,
                Gl_Const.ENM_TIMEUP_MAX_INTERVAL
            );
            yield return new WaitForSeconds(delay);

            //�G��ǉ�.
            EnemySpawnExe();
        }
    }

    /// <summary>
    /// �����_����������(AllBreak���[�h)
    /// </summary>
    public IEnumerator EnmSpawnAllBreak()
    {
        yield return new WaitForSeconds(3); //3�b�̗P�\.
     
        //�Q�[���I�����Ă��Ȃ����.
        if (!scptGameMng.isGameEnd)
        {
            //��C�ɏo��.
            for (int i = 0; i < Gl_Const.ENM_ALLBREAK_MAX_CNT; i++)
            {
                EnemySpawnExe();
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    /// <summary>
    /// �G���o��.
    /// </summary>
    public void EnemySpawnExe()
    {
        //�o�����W�𒊑I.
        Vector2Int rnd = Gl_Func.RndBPosOutside(30);

        //���̍��W�����}�X�Ȃ�.
        if (scptBoardMng.Board[rnd.x, rnd.y].type == BoardType.NONE)
        {
            //�G�o��.
            var obj = Instantiate(prefabItem, prefabInObj.transform);
            //���W�𒊑I���Ĕz�u.
            obj.transform.position = Gl_Func.BPosToWPos(new Vector2Int(rnd.x, rnd.y));
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
