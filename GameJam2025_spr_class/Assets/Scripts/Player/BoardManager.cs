/*
   - BoardManager.cs -
   最初に盤面を作る時のみ、全マスにprefabを生成し
   それ以降は、更新がある度にprefabの画像を差し替える.
*/
using System.Collections.Generic; //ListやQueueを使うために必要.
using Gloval;
using UnityEngine;

/// <summary>
/// 盤面の各マスに設定するデータ.
/// </summary>
public class BoardData
{
    public BoardType      type   { get; set; } //何のマスか.
    public SpriteRenderer typeSR { get; set; } //マスに配置するsprite(画像情報)
}

/// <summary>
/// 盤面の管理プログラム.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] GameManager scptGameMng;

    [Header("- prefab -")]
    [SerializeField] GameObject prfbSqrBack; //square back:マスの背景用.
    [SerializeField] GameObject prfbSqrType; //square type:マスの種類用.
    [Space]
    [SerializeField] GameObject prfbInObj;   //prefabを入れる所.

    [Header("- image -")]
    [SerializeField] Sprite imgPlyTrail;
    [SerializeField] Sprite imgPlyArea;

    //盤面の各マスデータ.
    BoardData[,] board = new BoardData[Gl_Const.BOARD_WID, Gl_Const.BOARD_HEI];
    public BoardData[,] Board 
    { 
        get => board; 
        set => board = value; 
    }

    Color areaColor; //エリア色のRGB.


    void Start()
    {
        RandAreaColor();

        InitBoard();
        BoardGenerate();
    }

    void Update()
    {
        //ゲーム中のみ.
        if (scptGameMng.startFlag && !scptGameMng.gameOverFlag)
        {
            UpdateBoard();
        }
    }

    /// <summary>
    /// 色の抽選.
    /// </summary>
    private void RandAreaColor()
    {
        //明るめの色の中で抽選.
        float r = Random.Range(0.5f, 1f);
        float g = Random.Range(0.5f, 1f);
        float b = Random.Range(0.5f, 1f);

        areaColor = new Color(r, g, b);
    }

    /// <summary>
    /// 盤面データの初期化.
    /// </summary>
    private void InitBoard()
    {
        //全マスループ.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                board[x, y] = new BoardData();
                board[x, y].type = BoardType.NONE; //無に設定.
            }
        }

        //中央の範囲.
        int stY = Gl_Const.BOARD_HEI/2 - Gl_Const.INIT_AREA_SIZE;
        int edY = Gl_Const.BOARD_HEI/2 + Gl_Const.INIT_AREA_SIZE;
        int stX = Gl_Const.BOARD_WID/2 - Gl_Const.INIT_AREA_SIZE;
        int edX = Gl_Const.BOARD_WID/2 + Gl_Const.INIT_AREA_SIZE;
        //中央あたりは初期陣地にする.
        for (int y = stY; y < edY; y++) {
            for (int x = stX; x < edX; x++) {

                board[x, y].type = BoardType.PLAYER_AREA;
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

                var objBack = Instantiate(prfbSqrBack, prfbInObj.transform);
                var objType = Instantiate(prfbSqrType, prfbInObj.transform);

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
    public void UpdateBoard()
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
                        board[x, y].typeSR.color  = Color.white;
                        break;

                    case BoardType.PLAYER_AREA:
                        board[x, y].typeSR.sprite = imgPlyArea; 
                        board[x, y].typeSR.color  = areaColor;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// プレイヤー痕跡で囲う処理.
    /// </summary>
    public void SurroundTrail()
    {
        //このqueueに入った座標のマスを調べ尽くす.
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        //訪れたマスの記録用.
        bool[,] isVisit = new bool[Gl_Const.BOARD_WID, Gl_Const.BOARD_HEI];
        //囲うのに成功したかどうか.
        bool isSurround = false;

        //boardの上端と下端.
        for (int x = 0; x < Gl_Const.BOARD_WID; x++)
        {
            TryEnqueue(queue, x, 0,                    isVisit);
            TryEnqueue(queue, x, Gl_Const.BOARD_HEI-1, isVisit);
        }
        //boardの左端と右端.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++)
        {
            TryEnqueue(queue, 0,                    y, isVisit);
            TryEnqueue(queue, Gl_Const.BOARD_WID-1, y, isVisit);
        }

        //未探索マスがなくなるまでループ.
        while (queue.Count > 0)
        {
            var pos = queue.Dequeue(); //未探索マスの座標を1つ取り出す.

            //4方向(上下左右)ループ.
            foreach (var dir in Gl_Func.GetVector4dir()) 
            {
                var newPos = pos + dir; //隣のマスに移動.

                //盤面の中にいる and 訪れたことがないなら.
                if (Gl_Func.IsInBoard(newPos) && !isVisit[newPos.x, newPos.y])
                {
                    TryEnqueue(queue, newPos.x, newPos.y, isVisit); //次の探索候補マス.
                }
            }
        }

        //全マスループ.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                //訪れてない無のマスがあるなら.
                if (!isVisit[x, y] && board[x, y].type == BoardType.NONE)
                {
                    board[x, y].type = BoardType.PLAYER_AREA; //エリアで埋める.
                    isSurround = true;
                }
            }
        }

        //囲うのに成功したなら.
        if (isSurround)
        {
            //全マスループ.
            for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
                for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                    //足元と痕跡をエリアで埋める.
                    if (board[x, y].type == BoardType.PLAYER_FOOT ||
                        board[x, y].type == BoardType.PLAYER_TRAIL)
                    {
                        board[x, y].type = BoardType.PLAYER_AREA;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 探索候補マス.
    /// 何もないマスならqueueに追加.
    /// </summary>
    private void TryEnqueue(Queue<Vector2Int> _queue, int _x, int _y, bool[,] _isVisit)
    {
        //まだ何もないなら.
        if (board[_x, _y].type == BoardType.NONE)
        {
            _queue.Enqueue(new Vector2Int(_x, _y)); //探索するマスに追加.
            _isVisit[_x, _y] = true;                //ここは訪れ済.
        }
    }
}
