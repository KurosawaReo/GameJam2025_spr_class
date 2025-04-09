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

    void Start()
    {

    }

    void Update()
    {
        InputMove();
        PlyTrail();
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

            var limX = Gl_Const.BOARD_WID * Gl_Const.SQUARE_SIZE / 2;
            var limY = Gl_Const.BOARD_HEI * Gl_Const.SQUARE_SIZE / 2;

            //���̈ړ����x(�����͂��̂܂�)
            if (Mathf.Abs(pos.x) > limX)
            {
                pos.x = Gl_Func.GetNumSign(pos.x) * limX;
            }
            //�c�̈ړ����x(�����͂��̂܂�)
            if (Mathf.Abs(pos.y) > limY)
            {
                pos.y = Gl_Func.GetNumSign(pos.y) * limY;
            }

            //����𔽉f.
            transform.position = pos;
            transform.eulerAngles = new Vector3(0, 0, input.ang);
        }
    }

    /// <summary>
    /// �v���C���[�̍��Ղ��c������.
    /// </summary>
    private void PlyTrail()
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
}
