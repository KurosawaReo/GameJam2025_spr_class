using UnityEngine;
using Gloval;

public class PlayerData 
{
    //private変数.
    private Vector2Int m_boardPos;

    //初期化(コンストラクタ)
    public PlayerData(Vector2Int _boardPos)
    {
        m_boardPos = _boardPos;
    }

    //set, get.
    public Vector2Int boardPos
    { 
        get => m_boardPos;
        set => m_boardPos = value;
    }
}

/// <summary>
/// プレイヤーのメインプログラム.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [Header("- value -")]
    [SerializeField] float moveSpeed;

    //プレイヤーデータ.
    PlayerData player = new PlayerData(
        new Vector2Int(0, 0) //boardPos.
    );

    void Start()
    {
        
    }

    void Update()
    {
        InputMove();
        PlySurround();
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
        if(input.vec != Vector2.zero)
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
            transform.position    = pos;
            transform.eulerAngles = new Vector3(0, 0, input.ang);
        }
    }

    /// <summary>
    /// プレイヤーが囲う処理.
    /// </summary>
    private void PlySurround()
    {
        Vector2Int position = Gl_Func.WPosToBPos(transform.position);
        Debug.Log("position:" + position);
    }
}
