/*
   - PlayerManager.cs -
*/

using UnityEngine;
using Gloval;

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
    [SerializeField] BoardManager scptBrdMng;

    [Header("- value -")]
    [SerializeField] float moveSpeed;

    ////プレイヤーデータ.
    PlayerData player = new PlayerData(
        new Vector2Int(0, 0)  //beforeBPos.
    );

    void Update()
    {
        InputMove();
        PlayerTrail();
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
    /// プレイヤーの痕跡を残す処理.
    /// </summary>
    private void PlayerTrail()
    {
        //プレイヤーのいるboard座標取得.
        var bPos = Gl_Func.WPosToBPos(transform.position);

        //座標が変化した(=移動したなら)
        if (player.beforeBPos != bPos)
        {
            player.beforeBPos = bPos; //座標更新.

            //陣地の中にいる間.
            if (scptBrdMng.Board[bPos.x, bPos.y].type == BoardType.PLAYER_AREA)
            {
                scptBrdMng.FillTrail(); //痕跡をエリアにする.
            }
            //陣地にいない間.
            else
            {
                //プレイヤーの位置を中心にループ.
                for (int i = -Gl_Const.PLAYER_TRAIL_SIZE / 2; i < Gl_Const.PLAYER_TRAIL_SIZE / 2; i++)
                {
                    for (int j = -Gl_Const.PLAYER_TRAIL_SIZE / 2; j < Gl_Const.PLAYER_TRAIL_SIZE / 2; j++)
                    {
                        var tmpBPos = bPos + new Vector2Int(i, j); //座標仮移動.

                        //盤面外ならスキップ.
                        if (!Gl_Func.IsInBoard(tmpBPos))
                        {
                            continue;
                        }
                        //仮移動したマスが無なら.
                        if (scptBrdMng.Board[tmpBPos.x, tmpBPos.y].type == BoardType.NONE)
                        {
                            scptBrdMng.Board[tmpBPos.x, tmpBPos.y].type = BoardType.PLAYER_TRAIL; //痕跡にする.
                        }
                    }
                }

                scptBrdMng.SurroundTrail(); //囲う処理.
            }
        }
    }

    /// <summary>
    /// プレイヤー死亡.
    /// </summary>
    public void PlayerDeath()
    {

    }
}
