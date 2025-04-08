using Gloval;
using UnityEngine;

/// <summary>
/// 盤面データ.
/// </summary>
public class BoardData
{
    //private変数.
    private BoardArea  m_area;    //誰のエリアか.
    private GameObject m_areaObj; //陣地として設置するobjのデータ.

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
/// 盤面管理プログラム.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbBoardBack; //盤面の背景prefab.
    [SerializeField] GameObject prfbAreaPly;   //プレイヤー陣地prefab.
    [SerializeField] GameObject prfbAreaEnm;   //敵陣地prefab.
    [Space]
    [SerializeField] GameObject prfbInObj;     //prefabを入れるところ.

    //盤面データ.
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
    /// 盤面データの初期化.
    /// </summary>
    private void InitBoard()
    {
        //1マスずつ.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                board[x, y].area = BoardArea.NONE; //無に設定.
            }
        }
    }

    /// <summary>
    /// 盤面を生成する.
    /// </summary>
    private void BoardGenerate()
    {
        //1マスずつ.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                var obj = Instantiate(prfbBoardBack, prfbInObj.transform);

                GameObject obj = null; //prefab情報を入れる用.

                //データ別で生成.
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

                    default: Debug.LogError("[Error] 不正な値です。"); break;
                }

                //盤面上に設置.
                Gl_Func.PlaceOnBoard(obj, x, y);
            }
        }
    }
}
