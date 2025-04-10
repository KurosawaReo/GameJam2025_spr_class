/*
   - PlayerManager.cs -
*/

using UnityEngine;
using Gloval;

public class PlayerData 
{
    public Vector2Int bPos { get; set; }

    //初期化(コンストラクタ)
    public PlayerData(Vector2Int _bPos)
    {
        bPos = _bPos;
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
    //PlayerData player = new PlayerData(
    //    new Vector2Int(0, 0)  //boardPos.
    //);

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
        var bpos = Gl_Func.WPosToBPos(transform.position);

        //現在マスが無なら.
        if(scptBrdMng.Board[bpos.x, bpos.y].type == BoardType.NONE)
        {
            scptBrdMng.Board[bpos.x, bpos.y].type = BoardType.PLAYER_TRAIL; //痕跡にする.
            scptBrdMng.SurroundTrail(); //囲う処理.
        }
    }

    /// <summary>
    /// プレイヤー死亡.
    /// </summary>
    public void PlayerDeath()
    {

    }
}
