/*
   - PlayerManager.cs -
*/

using UnityEngine;
using Gloval;

public class PlayerData 
{
    public Vector2Int bPos { get; set; }

    //������(�R���X�g���N�^)
    public PlayerData(Vector2Int _bPos)
    {
        bPos = _bPos;
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
    //PlayerData player = new PlayerData(
    //    new Vector2Int(0, 0)  //boardPos.
    //);

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
        var bpos = Gl_Func.WPosToBPos(transform.position);

        //���݃}�X�����Ȃ�.
        if(scptBrdMng.Board[bpos.x, bpos.y].type == BoardType.NONE)
        {
            scptBrdMng.Board[bpos.x, bpos.y].type = BoardType.PLAYER_TRAIL; //���Ղɂ���.
            scptBrdMng.SurroundTrail(); //�͂�����.
        }
    }

    /// <summary>
    /// �v���C���[���S.
    /// </summary>
    public void PlayerDeath()
    {

    }
}
