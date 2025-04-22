/*
   - PlayerManager.cs -
*/

using UnityEngine;
using Gloval;

public class PlayerData
{
    public Vector2Int beforeBPos { get; set; }  //1つ前のboard pos.
    public BoardType beforeBType { get; set; }  //1つ前のboard type.
    public Vector2 inputVec { get; set; }       //最後に操作した方向.
    public float inputAng { get; set; }         //最後に操作した角度.

    //初期化(コンストラクタ)
    public PlayerData(Vector2Int _beforeBPos, BoardType _beforeBType, Vector2 _inputVec, float _inputAng)
    {
        beforeBPos  = _beforeBPos;
        beforeBType = _beforeBType;
        inputVec    = _inputVec;
        inputAng    = _inputAng;
    }
}

/// <summary>
/// プレイヤーのメインプログラム.
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

    //プレイヤーデータ.
    PlayerData player;

    /// <summary>
    /// プレイヤー初期化.
    /// </summary>
    public void InitPlayer()
    {
        //位置リセット.
        transform.position    = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        //色リセット.
        GetComponent<SpriteRenderer>().color = Color.white;

        //データリセット.
        player = new PlayerData(
            new Vector2Int(0, 0),  //beforeBPos.
            BoardType.PLAYER_AREA, //beforeBType.
            new Vector2(0, 1),     //inputVec.
            0                      //inputAng.
        );
    }

    /// <summary>
    /// プレイヤー更新.
    /// </summary>
    public void UpdatePlayer()
    {
        InputMove();
        PlayerMove();
        CameraMove();
    }

    /// <summary>
    /// 移動操作.
    /// </summary>
    private void InputMove()
    {
        //現在位置を取得.
        Vector2 pos = transform.position;

        //操作の取得.
        var input = Gl_Func.InputKey4dir();
        //操作があれば.
        if (input.vec != Vector2.zero)
        {
            //移動方向を保存.
            player.inputVec = input.vec;
            player.inputAng = input.ang;
        }

        //仮移動.
        pos += player.inputVec * moveSpeed * Time.deltaTime;
        //盤面より外に出ていたら座標を修正する.
        pos = Gl_Func.LimPosInBoard(pos);

        //操作を反映.
        transform.position = pos;
        transform.eulerAngles = new Vector3(0, 0, player.inputAng);
    }

    /// <summary>
    /// プレイヤーの移動処理.
    /// </summary>
    private void PlayerMove()
    {
        //プレイヤーのいるboard座標取得.
        var bPos = Gl_Func.WPosToBPos(transform.position);

        //座標が変化した(=移動したなら)
        if (player.beforeBPos != bPos)
        {
            //今いるマスのタイプ別処理.
            switch (scptBrdMng.Board[bPos.x, bPos.y].type)
            {
                case BoardType.PLAYER_AREA:
                    //もし陣地に入ったばかりなら.
                    if (player.beforeBType != BoardType.PLAYER_AREA)
                    {
                        PlayerTrail(bPos); //痕跡処理.

                        //全マスループ.
                        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
                            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                                //足元と痕跡をエリアで埋める.
                                if (scptBrdMng.Board[x, y].type == BoardType.PLAYER_FOOT ||
                                    scptBrdMng.Board[x, y].type == BoardType.PLAYER_TRAIL)
                                {
                                    scptBrdMng.Board[x, y].type = BoardType.PLAYER_AREA;
                                    scptGameMng.boardNoneCnt--; //空きマス-1.
                                }
                            }
                        }

                        scptBrdMng.SurroundTrail(); //囲う処理.
                        scptBrdMng.DrawBoard();     //盤面更新.
                    }
                    break;
                
                case BoardType.PLAYER_TRAIL:
                    PlayerDeath(); //死亡処理.
                    break;
                
                case BoardType.PLAYER_FOOT:
                case BoardType.NONE:
                    PlayerTrail(bPos);      //痕跡処理.
                    scptBrdMng.DrawBoard(); //盤面更新.
                    break;
                
            }

            //座標とtype情報を保存する.
            player.beforeBPos  = bPos;
            player.beforeBType = scptBrdMng.Board[bPos.x, bPos.y].type;
        }
    }

    /// <summary>
    /// カメラ追跡.
    /// </summary>
    private void CameraMove()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = mainCamera.transform.position.z;

        //移動.
        mainCamera.transform.position = new Vector3(x, y, z);
    }

    /// <summary>
    /// プレイヤーの痕跡処理.
    /// </summary>
    /// <param name="_bPos">ボード座標</param>
    private void PlayerTrail(Vector2Int _bPos)
    {
        //処理軽減のためプレイヤーの周りのみ.
        for (int y = _bPos.y-3; y <= _bPos.y+3; y++) {
            for (int x = _bPos.x-3; x <= _bPos.x+3; x++) {

                //盤面外ならスキップ.
                if (!Gl_Func.IsInBoard(new Vector2Int(x, y)))
                {
                    continue;
                }

                //ある程度プレイヤーから離れたら.
                if(x < _bPos.x-1 || x > _bPos.x+1 ||
                   y < _bPos.y-1 || y > _bPos.y+1
                ){
                    //足元を痕跡に置き換える.
                    if (scptBrdMng.Board[x, y].type == BoardType.PLAYER_FOOT)
                    {
                        scptBrdMng.Board[x, y].type = BoardType.PLAYER_TRAIL;
                    }
                }
            }
        }

        var plyPos = transform.position;

        //プレイヤー周りの4マス分.
        Vector2[] plyPos4 = new Vector2[4] { 
            new Vector2(plyPos.x+Gl_Const.SQUARE_SIZE/2, plyPos.y+Gl_Const.SQUARE_SIZE/2), 
            new Vector2(plyPos.x+Gl_Const.SQUARE_SIZE/2, plyPos.y-Gl_Const.SQUARE_SIZE/2), 
            new Vector2(plyPos.x-Gl_Const.SQUARE_SIZE/2, plyPos.y+Gl_Const.SQUARE_SIZE/2), 
            new Vector2(plyPos.x-Gl_Const.SQUARE_SIZE/2, plyPos.y-Gl_Const.SQUARE_SIZE/2)
        };

        //4マス分ループ.
        foreach (var i in plyPos4)
        {
            var bPos = Gl_Func.WPosToBPos(i);

            //盤面外ならスキップ.
            if (!Gl_Func.IsInBoard(new Vector2Int(bPos.x, bPos.y)))
            {
                continue;
            }
            //仮移動したマスが無なら.
            if (scptBrdMng.Board[bPos.x, bPos.y].type == BoardType.NONE)
            {
                scptBrdMng.Board[bPos.x, bPos.y].type = BoardType.PLAYER_FOOT; //足元にする.
            }
        }
    }

    /// <summary>
    /// プレイヤー死亡.
    /// </summary>
    public void PlayerDeath()
    {
        //全マスループ.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                //足元と痕跡を無に置き換える.
                if (scptBrdMng.Board[x, y].type == BoardType.PLAYER_FOOT ||
                    scptBrdMng.Board[x, y].type == BoardType.PLAYER_TRAIL)
                {
                    scptBrdMng.Board[x, y].type = BoardType.NONE;
                }
            }
        }
        
        scptBrdMng.DrawBoard(); //盤面更新.
        scptGameMng.PlayerDead(); //死亡処理.

        //透明化.
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0 ,0);
    }
}
