/*
   - PlayerManager.cs -
*/

using UnityEngine;
using Gloval;

public class PlayerData
{
    public Vector2Int beforeBPos { get; set; }
    public Vector2 inputVec { get; set; } //最後に操作した方向.
    public float inputAng { get; set; } //最後に操作した角度.

    //初期化(コンストラクタ)
    public PlayerData(Vector2Int _beforeBPos, Vector2 _inputVec, float _inputAng)
    {
        beforeBPos = _beforeBPos;
        inputVec   = _inputVec;
        inputAng   = _inputAng;
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

    ////プレイヤーデータ.
    PlayerData player = new PlayerData(
        new Vector2Int(0, 0), //beforeBPos.
        new Vector2   (0, 1), //inputVec.
        0                     //inputAng.
    );

    void Update()
    {
        //ゲーム中のみ.
        if (scptGameMng.startFlag && !scptGameMng.gameOverFlag)
        {
            InputMove();
            PlayerMove();
            CameraMove();
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
        
        scptBrdMng.UpdateBoard(); //盤面更新.
        scptGameMng.PlayerDead(); //死亡処理.

        //透明化.
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0 ,0);
    }
}
