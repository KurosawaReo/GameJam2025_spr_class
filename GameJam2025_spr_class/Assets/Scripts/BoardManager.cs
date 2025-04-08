using Gloval;
using UnityEngine;

/// <summary>
/// �Ֆʃf�[�^.
/// </summary>
public class BoardData
{
    //private�ϐ�.
    private BoardArea  m_area;    //�N�̃G���A��.
    private GameObject m_areaObj; //�w�n�Ƃ��Đݒu����obj�̃f�[�^.

    //set, get.
    public BoardArea area {
        get => m_area; 
        set => m_area = value; 
    }
    public GameObject areaObj
    {
        get => m_areaObj;
        set => m_areaObj = value;
    }
}

/// <summary>
/// �ՖʊǗ��v���O����.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbBoardBack; //�Ֆʂ̔w�iprefab.
    [SerializeField] GameObject prfbAreaPly;   //�v���C���[�w�nprefab.
    [SerializeField] GameObject prfbAreaEnm;   //�G�w�nprefab.
    [Space]
    [SerializeField] GameObject prfbInObj;     //prefab������Ƃ���.

    //�Ֆʃf�[�^.
    BoardData[,] board = new BoardData[Gl_Const.BOARD_WID, Gl_Const.BOARD_HEI];

    void Start()
    {
        InitBoard();
        BoardGenerate();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// �Ֆʃf�[�^�̏�����.
    /// </summary>
    private void InitBoard()
    {
        //1�}�X����.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                board[x, y].area = BoardArea.NONE; //���ɐݒ�.
            }
        }
    }

    /// <summary>
    /// �Ֆʂ𐶐�����.
    /// </summary>
    private void BoardGenerate()
    {
        //1�}�X����.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                var obj = Instantiate(prfbBoardBack, prfbInObj.transform);

                GameObject obj = null; //prefab��������p.

                //�f�[�^�ʂŐ���.
                switch (board[x, y].area) {
                    case BoardArea.NONE:
                        obj = Instantiate(prfbBoardBack, prfbInObj.transform);
                        break;
                    case BoardArea.PLAYER:
                        obj = Instantiate(prfbAreaPly, prfbInObj.transform);
                        break;
                    case BoardArea.ENEMY:
                        obj = Instantiate(prfbAreaEnm, prfbInObj.transform);
                        break;

                    default: Debug.LogError("[Error] �s���Ȓl�ł��B"); break;
                }

                //�Ֆʏ�ɐݒu.
                Gl_Func.PlaceOnBoard(obj, x, y);
            }
        }
    }
}
