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

    void Start()
    {

    }

    void Update()
    {
        InputMove();
        PlyTrail();
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

            var limX = Gl_Const.BOARD_WID * Gl_Const.SQUARE_SIZE / 2;
            var limY = Gl_Const.BOARD_HEI * Gl_Const.SQUARE_SIZE / 2;

            //横の移動限度(符号はそのまま)
            if (Mathf.Abs(pos.x) > limX)
            {
                pos.x = Gl_Func.GetNumSign(pos.x) * limX;
            }
            //縦の移動限度(符号はそのまま)
            if (Mathf.Abs(pos.y) > limY)
            {
                pos.y = Gl_Func.GetNumSign(pos.y) * limY;
            }

            //操作を反映.
            transform.position = pos;
            transform.eulerAngles = new Vector3(0, 0, input.ang);
        }
    }

    /// <summary>
    /// プレイヤーの痕跡を残す処理.
    /// </summary>
    private void PlyTrail()
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
}
