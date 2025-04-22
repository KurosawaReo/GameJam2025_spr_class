/*
   - PlayerManager.cs -
*/

using UnityEngine;
using Gloval;

public class PlayerData
{
    public Vector2Int beforeBPos { get; set; }  //1�O��board pos.
    public BoardType beforeBType { get; set; }  //1�O��board type.
    public Vector2 inputVec { get; set; }       //�Ō�ɑ��삵������.
    public float inputAng { get; set; }         //�Ō�ɑ��삵���p�x.

    //������(�R���X�g���N�^)
    public PlayerData(Vector2Int _beforeBPos, BoardType _beforeBType, Vector2 _inputVec, float _inputAng)
    {
        beforeBPos  = _beforeBPos;
        beforeBType = _beforeBType;
        inputVec    = _inputVec;
        inputAng    = _inputAng;
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

    //�v���C���[�f�[�^.
    PlayerData player;

    /// <summary>
    /// �v���C���[������.
    /// </summary>
    public void InitPlayer()
    {
        //�ʒu���Z�b�g.
        transform.position    = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        //�F���Z�b�g.
        GetComponent<SpriteRenderer>().color = Color.white;

        //�f�[�^���Z�b�g.
        player = new PlayerData(
            new Vector2Int(0, 0),  //beforeBPos.
            BoardType.PLAYER_AREA, //beforeBType.
            new Vector2(0, 1),     //inputVec.
            0                      //inputAng.
        );
    }

    /// <summary>
    /// �v���C���[�X�V.
    /// </summary>
    public void UpdatePlayer()
    {
        InputMove();
        PlayerMove();
        CameraMove();
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
            //������}�X�̃^�C�v�ʏ���.
            switch (scptBrdMng.Board[bPos.x, bPos.y].type)
            {
                case BoardType.PLAYER_AREA:
                    //�����w�n�ɓ������΂���Ȃ�.
                    if (player.beforeBType != BoardType.PLAYER_AREA)
                    {
                        PlayerTrail(bPos); //���Տ���.

                        //�S�}�X���[�v.
                        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
                            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                                //�����ƍ��Ղ��G���A�Ŗ��߂�.
                                if (scptBrdMng.Board[x, y].type == BoardType.PLAYER_FOOT ||
                                    scptBrdMng.Board[x, y].type == BoardType.PLAYER_TRAIL)
                                {
                                    scptBrdMng.Board[x, y].type = BoardType.PLAYER_AREA;
                                    scptGameMng.boardNoneCnt--; //�󂫃}�X-1.
                                }
                            }
                        }

                        scptBrdMng.SurroundTrail(); //�͂�����.
                        scptBrdMng.DrawBoard();     //�ՖʍX�V.
                    }
                    break;
                
                case BoardType.PLAYER_TRAIL:
                    PlayerDeath(); //���S����.
                    break;
                
                case BoardType.PLAYER_FOOT:
                case BoardType.NONE:
                    PlayerTrail(bPos);      //���Տ���.
                    scptBrdMng.DrawBoard(); //�ՖʍX�V.
                    break;
                
            }

            //���W��type����ۑ�����.
            player.beforeBPos  = bPos;
            player.beforeBType = scptBrdMng.Board[bPos.x, bPos.y].type;
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
    /// <param name="_bPos">�{�[�h���W</param>
    private void PlayerTrail(Vector2Int _bPos)
    {
        //�����y���̂��߃v���C���[�̎���̂�.
        for (int y = _bPos.y-3; y <= _bPos.y+3; y++) {
            for (int x = _bPos.x-3; x <= _bPos.x+3; x++) {

                //�ՖʊO�Ȃ�X�L�b�v.
                if (!Gl_Func.IsInBoard(new Vector2Int(x, y)))
                {
                    continue;
                }

                //������x�v���C���[���痣�ꂽ��.
                if(x < _bPos.x-1 || x > _bPos.x+1 ||
                   y < _bPos.y-1 || y > _bPos.y+1
                ){
                    //���������Ղɒu��������.
                    if (scptBrdMng.Board[x, y].type == BoardType.PLAYER_FOOT)
                    {
                        scptBrdMng.Board[x, y].type = BoardType.PLAYER_TRAIL;
                    }
                }
            }
        }

        var plyPos = transform.position;

        //�v���C���[�����4�}�X��.
        Vector2[] plyPos4 = new Vector2[4] { 
            new Vector2(plyPos.x+Gl_Const.SQUARE_SIZE/2, plyPos.y+Gl_Const.SQUARE_SIZE/2), 
            new Vector2(plyPos.x+Gl_Const.SQUARE_SIZE/2, plyPos.y-Gl_Const.SQUARE_SIZE/2), 
            new Vector2(plyPos.x-Gl_Const.SQUARE_SIZE/2, plyPos.y+Gl_Const.SQUARE_SIZE/2), 
            new Vector2(plyPos.x-Gl_Const.SQUARE_SIZE/2, plyPos.y-Gl_Const.SQUARE_SIZE/2)
        };

        //4�}�X�����[�v.
        foreach (var i in plyPos4)
        {
            var bPos = Gl_Func.WPosToBPos(i);

            //�ՖʊO�Ȃ�X�L�b�v.
            if (!Gl_Func.IsInBoard(new Vector2Int(bPos.x, bPos.y)))
            {
                continue;
            }
            //���ړ������}�X�����Ȃ�.
            if (scptBrdMng.Board[bPos.x, bPos.y].type == BoardType.NONE)
            {
                scptBrdMng.Board[bPos.x, bPos.y].type = BoardType.PLAYER_FOOT; //�����ɂ���.
            }
        }
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
        
        scptBrdMng.DrawBoard(); //�ՖʍX�V.
        scptGameMng.PlayerDead(); //���S����.

        //������.
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0 ,0);
    }
}
