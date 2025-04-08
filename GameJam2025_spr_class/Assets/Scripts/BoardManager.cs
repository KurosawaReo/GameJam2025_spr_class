/*
   - BoardManager.cs -
   最初に盤面を作る時のみ、全マスにprefabを生成し
   それ以降は、更新がある度にprefabの画像を差し替える.
*/
using Gloval;
using UnityEngine;

/// <summary>
/// 盤面の各マスに設定するデータ.
/// </summary>
public class BoardData
{
    //private変数.
    private BoardType      m_type;   //何のマスか.
    private SpriteRenderer m_typeSR; //マスに配置するsprite(画像情報)

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
/// 盤面の管理プログラム.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbBoardBack; //マスの背景用.
    [SerializeField] GameObject prfbBoardType; //マスの種類用.
    [Space]
    [SerializeField] GameObject prfbInObj;     //prefabを入れる所.

    [Header("- image -")]
    [SerializeField] Sprite imgPlyTrail;
    [SerializeField] Sprite imgPlyArea;
    [SerializeField] Sprite imgEnm;

    //盤面の各マスデータ.
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
    /// 盤面データの初期化.
    /// </summary>
    private void InitBoard()
    {
        //1マスずつ.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                board[x, y] = new BoardData();
                board[x, y].type = BoardType.NONE; //無に設定.
            }
        }
        board[0, 1].type = BoardType.PLAYER_TRAIL;
        board[0, 2].type = BoardType.PLAYER_AREA; 
    }

    /// <summary>
    /// 盤面を生成する.
    /// </summary>
    private void BoardGenerate()
    {
        //1マスずつ.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                var objBack = Instantiate(prfbBoardBack, prfbInObj.transform);
                var objType = Instantiate(prfbBoardType, prfbInObj.transform);

                //各マスのsprite情報を記録.
                board[x, y].typeSR = objType.GetComponent<SpriteRenderer>();

                //盤面上に設置.
                Gl_Func.PlaceOnBoard(objBack, x, y);
                Gl_Func.PlaceOnBoard(objType, x, y);
            }
        }
    }

    /// <summary>
    /// 盤面の更新.
    /// </summary>
    private void UpdateBoard()
    {
        //1マスずつ.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                //種類別で画像をセット.
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
