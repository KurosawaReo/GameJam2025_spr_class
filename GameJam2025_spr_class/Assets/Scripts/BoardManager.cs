/*
   - BoardManager.cs -
   �ŏ��ɔՖʂ���鎞�̂݁A�S�}�X��prefab�𐶐���
   ����ȍ~�́A�X�V������x��prefab�̉摜�������ւ���.
*/
using Gloval;
using UnityEngine;

/// <summary>
/// �Ֆʂ̊e�}�X�ɐݒ肷��f�[�^.
/// </summary>
public class BoardData
{
    //private�ϐ�.
    private BoardType      m_type;   //���̃}�X��.
    private SpriteRenderer m_typeSR; //�}�X�ɔz�u����sprite(�摜���)

    //set, get.
    public BoardType type 
    {
        get => m_type;
        set => m_type = value; 
    }
    public SpriteRenderer typeSR
    {
        get => m_typeSR;
        set => m_typeSR = value;
    }
}

/// <summary>
/// �Ֆʂ̊Ǘ��v���O����.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbBoardBack; //�}�X�̔w�i�p.
    [SerializeField] GameObject prfbBoardType; //�}�X�̎�ޗp.
    [Space]
    [SerializeField] GameObject prfbInObj;     //prefab�����鏊.

    [Header("- image -")]
    [SerializeField] Sprite imgPlyTrail;
    [SerializeField] Sprite imgPlyArea;
    [SerializeField] Sprite imgEnm;

    //�Ֆʂ̊e�}�X�f�[�^.
    BoardData[,] board = new BoardData[Gl_Const.BOARD_WID, Gl_Const.BOARD_HEI];

    void Start()
    {
        InitBoard();
        BoardGenerate();
    }

    void Update()
    {
        UpdateBoard();
    }

    /// <summary>
    /// �Ֆʃf�[�^�̏�����.
    /// </summary>
    private void InitBoard()
    {
        //1�}�X����.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                board[x, y] = new BoardData();
                board[x, y].type = BoardType.NONE; //���ɐݒ�.
            }
        }
        board[0, 1].type = BoardType.PLAYER_TRAIL;
        board[0, 2].type = BoardType.PLAYER_AREA; 
    }

    /// <summary>
    /// �Ֆʂ𐶐�����.
    /// </summary>
    private void BoardGenerate()
    {
        //1�}�X����.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                var objBack = Instantiate(prfbBoardBack, prfbInObj.transform);
                var objType = Instantiate(prfbBoardType, prfbInObj.transform);

                //�e�}�X��sprite�����L�^.
                board[x, y].typeSR = objType.GetComponent<SpriteRenderer>();

                //�Ֆʏ�ɐݒu.
                Gl_Func.PlaceOnBoard(objBack, x, y);
                Gl_Func.PlaceOnBoard(objType, x, y);
            }
        }
    }

    /// <summary>
    /// �Ֆʂ̍X�V.
    /// </summary>
    private void UpdateBoard()
    {
        //1�}�X����.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                //��ޕʂŉ摜���Z�b�g.
                switch (board[x, y].type) 
                { 
                    case BoardType.NONE:
                        board[x, y].typeSR.sprite = null; 
                        break;
                    case BoardType.PLAYER_TRAIL:
                        board[x, y].typeSR.sprite = imgPlyTrail; 
                        break;
                    case BoardType.PLAYER_AREA:
                        board[x, y].typeSR.sprite = imgPlyArea; 
                        break;
                    case BoardType.ENEMY:
                        board[x, y].typeSR.sprite = imgEnm;
                        break;
                }
            }
        }
    }
}
