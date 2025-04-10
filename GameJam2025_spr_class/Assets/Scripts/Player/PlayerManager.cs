/*
   - PlayerManager.cs -
*/

using UnityEngine;
using Gloval;

public class PlayerData
{
    public Vector2Int beforeBPos { get; set; }

    //������(�R���X�g���N�^)
    public PlayerData(Vector2Int _beforeBPos)
    {
        beforeBPos = _beforeBPos;
    }
}

/// <summary>
/// �v���C���[�̃��C���v���O����.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] BoardManager scptBrdMng;

    [Header("- value -")]
    [SerializeField] float moveSpeed;

    ////�v���C���[�f�[�^.
    PlayerData player = new PlayerData(
        new Vector2Int(0, 0)  //beforeBPos.
    );

    void Update()
    {
        InputMove();
        PlayerTrail();
    }

    /// <summary>
    /// �ړ�����.
    /// </summary>
    private void InputMove()
    {
        //���݈ʒu���擾.
        Vector2 pos = transform.position;
        //����̎擾.
        var input = Gl_Func.InputKey4dir();

        //���삪�����.
        if (input.vec != Vector2.zero)
        {
            //���ړ�.
            pos += input.vec * moveSpeed * Time.deltaTime;
            //�Ֆʂ��O�ɏo�Ă�������W���C������.
            pos = Gl_Func.LimPosInBoard(pos);

            //����𔽉f.
            transform.position = pos;
            transform.eulerAngles = new Vector3(0, 0, input.ang);
        }
    }

    /// <summary>
    /// �v���C���[�̍��Ղ��c������.
    /// </summary>
    private void PlayerTrail()
    {
        //�v���C���[�̂���board���W�擾.
        var bPos = Gl_Func.WPosToBPos(transform.position);

        //���W���ω�����(=�ړ������Ȃ�)
        if (player.beforeBPos != bPos)
        {
            player.beforeBPos = bPos; //���W�X�V.

            //�w�n�̒��ɂ����.
            if (scptBrdMng.Board[bPos.x, bPos.y].type == BoardType.PLAYER_AREA)
            {
                scptBrdMng.FillTrail(); //���Ղ��G���A�ɂ���.
            }
            //�w�n�ɂ��Ȃ���.
            else
            {
                //�v���C���[�̈ʒu�𒆐S�Ƀ��[�v.
                for (int i = -Gl_Const.PLAYER_TRAIL_SIZE / 2; i < Gl_Const.PLAYER_TRAIL_SIZE / 2; i++)
                {
                    for (int j = -Gl_Const.PLAYER_TRAIL_SIZE / 2; j < Gl_Const.PLAYER_TRAIL_SIZE / 2; j++)
                    {
                        var tmpBPos = bPos + new Vector2Int(i, j); //���W���ړ�.

                        //�ՖʊO�Ȃ�X�L�b�v.
                        if (!Gl_Func.IsInBoard(tmpBPos))
                        {
                            continue;
                        }
                        //���ړ������}�X�����Ȃ�.
                        if (scptBrdMng.Board[tmpBPos.x, tmpBPos.y].type == BoardType.NONE)
                        {
                            scptBrdMng.Board[tmpBPos.x, tmpBPos.y].type = BoardType.PLAYER_TRAIL; //���Ղɂ���.
                        }
                    }
                }

                scptBrdMng.SurroundTrail(); //�͂�����.
            }
        }
    }

    /// <summary>
    /// �v���C���[���S.
    /// </summary>
    public void PlayerDeath()
    {

    }
}
