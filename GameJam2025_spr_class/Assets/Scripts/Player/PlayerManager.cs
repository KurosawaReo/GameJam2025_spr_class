/*
   - PlayerManager.cs -
*/

using UnityEngine;
using Gloval;
using System.Xml.Serialization;

public class PlayerData
{
    public Vector2Int beforeBPos { get; set; }

    //初期化(コンストラクタ)
    public PlayerData(Vector2Int _beforeBPos)
    {
        beforeBPos = _beforeBPos;
    }
}

/// <summary>
/// プレイヤーのメインプログラム.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] GameManager  scptGameMng;
    [SerializeField] BoardManager scptBrdMng;

    [Header("- value -")]
    [SerializeField] float moveSpeed;

    ////プレイヤーデータ.
    PlayerData player = new PlayerData(
        new Vector2Int(0, 0)  //beforeBPos.
    );

    void Update()
    {
        //ゲーム中のみ.
        if (scptGameMng.startFlag && !scptGameMng.gameOverFlag)
        {
            InputMove();
            PlayerMove();
        }
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
            //仮移動.
            pos += input.vec * moveSpeed * Time.deltaTime;
            //盤面より外に出ていたら座標を修正する.
            pos = Gl_Func.LimPosInBoard(pos);

            //操作を反映.
            transform.position = pos;
            transform.eulerAngles = new Vector3(0, 0, input.ang);
        }
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
            player.beforeBPos = bPos; //座標更新.

            //今いるマスのタイプ別処理.
            switch (scptBrdMng.Board[bPos.x, bPos.y].type)
            {
                case BoardType.PLAYER_AREA:
                {
                    scptBrdMng.SurroundTrail(); //囲う処理.


                    //全マスループ.
                    for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
                        for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                            //足元と痕跡をエリアで埋める.
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
                    PlayerDeath();     //死亡処理.
                    break;
                }
                case BoardType.PLAYER_FOOT:
                case BoardType.NONE:
                {
                    PlayerTrail(bPos); //痕跡処理.
                    break;
                }
            }
        }
    }

    /// <summary>
    /// プレイヤーの痕跡処理.
    /// </summary>
    /// <param name="bPos">ボード座標</param>
    private void PlayerTrail(Vector2Int bPos)
    {
        //プレイヤー周辺のマスループ.
        for (int y = bPos.y-7; y < bPos.y+7; y++) {
            for (int x = bPos.x-7; x < bPos.x+7; x++) {

                //盤面外ならスキップ.
                if (!Gl_Func.IsInBoard(new Vector2Int(x, y)))
                {
                    continue;
                }

                //足元じゃなくなるマスを変える.
                if(x < bPos.x - Gl_Const.PLAYER_TRAIL_SIZE ||
                   x > bPos.x + Gl_Const.PLAYER_TRAIL_SIZE ||
                   y < bPos.y - Gl_Const.PLAYER_TRAIL_SIZE ||
                   y > bPos.y + Gl_Const.PLAYER_TRAIL_SIZE
                ){
                    //足元を痕跡に置き換える.
                    if (scptBrdMng.Board[x, y].type == BoardType.PLAYER_FOOT)
                    {
                        scptBrdMng.Board[x, y].type = BoardType.PLAYER_TRAIL;
                    }
                }  
            }
        }

        //一定の範囲に足元マスを塗る.
        for (int y = bPos.y-Gl_Const.PLAYER_TRAIL_SIZE; y < bPos.y+Gl_Const.PLAYER_TRAIL_SIZE; y++) {
            for (int x = bPos.x-Gl_Const.PLAYER_TRAIL_SIZE; x < bPos.x+Gl_Const.PLAYER_TRAIL_SIZE; x++) {

                //盤面外ならスキップ.
                if (!Gl_Func.IsInBoard(new Vector2Int(x, y)))
                {
                    continue;
                }
                //仮移動したマスが無なら.
                if (scptBrdMng.Board[x, y].type == BoardType.NONE)
                {
                    scptBrdMng.Board[x, y].type = BoardType.PLAYER_FOOT; //足元にする.
                }
            }
        }

        //scptBrdMng.SurroundTrail(); //囲う処理.
    }

    /// <summary>
    /// プレイヤー死亡.
    /// </summary>
    public void PlayerDeath()
    {
        //痕跡を無に置き換える.
        scptBrdMng.ReplaceAllBoard(BoardType.PLAYER_TRAIL, BoardType.NONE);
        //死亡処理.
        scptGameMng.PlayerDead();
        
        Destroy(gameObject); //自身を消滅.
    }
}
