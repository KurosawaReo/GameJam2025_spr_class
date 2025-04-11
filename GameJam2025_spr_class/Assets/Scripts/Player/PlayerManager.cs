/*
   - PlayerManager.cs -
*/

using UnityEngine;
using Gloval;

public class PlayerData
{
    public Vector2Int beforeBPos { get; set; }
    public Vector2 inputVec { get; set; } //�Ō�ɑ��삵������.
    public float inputAng { get; set; } //�Ō�ɑ��삵���p�x.

    //������(�R���X�g���N�^)
    public PlayerData(Vector2Int _beforeBPos, Vector2 _inputVec, float _inputAng)
    {
        beforeBPos = _beforeBPos;
        inputVec   = _inputVec;
        inputAng   = _inputAng;
    }
}

/// <summary>
/// �v���C���[�̃��C���v���O����.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [Header("- camera -")]
    [SerializeField] Camera mainCamera;

    [Header("- script -")]
    [SerializeField] GameManager  scptGameMng;
    [SerializeField] BoardManager scptBrdMng;

    [Header("- value -")]
    [SerializeField] float moveSpeed = 3;

    ////�v���C���[�f�[�^.
    PlayerData player = new PlayerData(
        new Vector2Int(0, 0), //beforeBPos.
        new Vector2   (0, 1), //inputVec.
        0                     //inputAng.
    );

    void Update()
    {
        //�Q�[�����̂�.
        if (scptGameMng.startFlag && !scptGameMng.gameOverFlag)
        {
            InputMove();
            PlayerMove();
            CameraMove();
        }
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
            //�ړ�������ۑ�.
            player.inputVec = input.vec;
            player.inputAng = input.ang;
        }

        //���ړ�.
        pos += player.inputVec * moveSpeed * Time.deltaTime;
        //�Ֆʂ��O�ɏo�Ă�������W���C������.
        pos = Gl_Func.LimPosInBoard(pos);

        //����𔽉f.
        transform.position = pos;
        transform.eulerAngles = new Vector3(0, 0, player.inputAng);
    }

    /// <summary>
    /// �v���C���[�̈ړ�����.
    /// </summary>
    private void PlayerMove()
    {
        //�v���C���[�̂���board���W�擾.
        var bPos = Gl_Func.WPosToBPos(transform.position);

        //���W���ω�����(=�ړ������Ȃ�)
        if (player.beforeBPos != bPos)
        {
            player.beforeBPos = bPos; //���W�X�V.

            //������}�X�̃^�C�v�ʏ���.
            switch (scptBrdMng.Board[bPos.x, bPos.y].type)
            {
                case BoardType.PLAYER_AREA:
                {
                    scptBrdMng.SurroundTrail(); //�͂�����.

                    //�S�}�X���[�v.
                    for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
                        for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                            //�����ƍ��Ղ��G���A�Ŗ��߂�.
                            if (scptBrdMng.Board[x, y].type == BoardType.PLAYER_FOOT ||
                                scptBrdMng.Board[x, y].type == BoardType.PLAYER_TRAIL)
                            {
                                scptBrdMng.Board[x, y].type = BoardType.PLAYER_AREA;
                            }
                        }
                    }
                    break;
                }
                case BoardType.PLAYER_TRAIL:
                {
                    PlayerDeath();     //���S����.
                    break;
                }
                case BoardType.PLAYER_FOOT:
                case BoardType.NONE:
                {
                    PlayerTrail(bPos); //���Տ���.
                    break;
                }
            }
        }
    }

    /// <summary>
    /// �J�����ǐ�.
    /// </summary>
    private void CameraMove()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = mainCamera.transform.position.z;

        //�ړ�.
        mainCamera.transform.position = new Vector3(x, y, z);
    }

    /// <summary>
    /// �v���C���[�̍��Տ���.
    /// </summary>
    /// <param name="bPos">�{�[�h���W</param>
    private void PlayerTrail(Vector2Int bPos)
    {
        //�v���C���[���ӂ̃}�X���[�v.
        for (int y = bPos.y-7; y < bPos.y+7; y++) {
            for (int x = bPos.x-7; x < bPos.x+7; x++) {

                //�ՖʊO�Ȃ�X�L�b�v.
                if (!Gl_Func.IsInBoard(new Vector2Int(x, y)))
                {
                    continue;
                }

                //��������Ȃ��Ȃ�}�X��ς���.
                if(x < bPos.x - Gl_Const.PLAYER_TRAIL_SIZE ||
                   x > bPos.x + Gl_Const.PLAYER_TRAIL_SIZE ||
                   y < bPos.y - Gl_Const.PLAYER_TRAIL_SIZE ||
                   y > bPos.y + Gl_Const.PLAYER_TRAIL_SIZE
                ){
                    //���������Ղɒu��������.
                    if (scptBrdMng.Board[x, y].type == BoardType.PLAYER_FOOT)
                    {
                        scptBrdMng.Board[x, y].type = BoardType.PLAYER_TRAIL;
                    }
                }  
            }
        }

        //���͈̔͂ɑ����}�X��h��.
        for (int y = bPos.y-Gl_Const.PLAYER_TRAIL_SIZE; y < bPos.y+Gl_Const.PLAYER_TRAIL_SIZE; y++) {
            for (int x = bPos.x-Gl_Const.PLAYER_TRAIL_SIZE; x < bPos.x+Gl_Const.PLAYER_TRAIL_SIZE; x++) {

                //�ՖʊO�Ȃ�X�L�b�v.
                if (!Gl_Func.IsInBoard(new Vector2Int(x, y)))
                {
                    continue;
                }
                //���ړ������}�X�����Ȃ�.
                if (scptBrdMng.Board[x, y].type == BoardType.NONE)
                {
                    scptBrdMng.Board[x, y].type = BoardType.PLAYER_FOOT; //�����ɂ���.
                }
            }
        }

        //scptBrdMng.SurroundTrail(); //�͂�����.
    }

    /// <summary>
    /// �v���C���[���S.
    /// </summary>
    public void PlayerDeath()
    {
        //�S�}�X���[�v.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                //�����ƍ��Ղ𖳂ɒu��������.
                if (scptBrdMng.Board[x, y].type == BoardType.PLAYER_FOOT ||
                    scptBrdMng.Board[x, y].type == BoardType.PLAYER_TRAIL)
                {
                    scptBrdMng.Board[x, y].type = BoardType.NONE;
                }
            }
        }
        
        scptBrdMng.UpdateBoard(); //�ՖʍX�V.
        scptGameMng.PlayerDead(); //���S����.

        //������.
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0 ,0);
    }
}
