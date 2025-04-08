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
    [SerializeField] GameObject prfbBoardArea; //�w�n�p��prefab.
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

                //�w�i�̐���.
                var obj1 = Instantiate(prfbBoardBack, prfbInObj.transform);
                //�w�n�̐���.
                var obj2 = Instantiate(prfbBoardArea, prfbInObj.transform);

                //�Ֆʏ�ɐݒu.
                Gl_Func.PlaceOnBoard(obj1, x, y);
                Gl_Func.PlaceOnBoard(obj2, x, y);
            }
        }
    }
}
