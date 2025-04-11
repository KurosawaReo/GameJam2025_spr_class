/*
   - Common.cs -
   全てのscriptで使える定数や関数などをまとめた所.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gloval
{
    /// <summary>
    /// 盤面のマス種類.
    /// </summary>
    public enum BoardType
    {
        NONE,         //無.
        PLAYER_FOOT,  //プレイヤーの足元(現在位置)
        PLAYER_TRAIL, //プレイヤーの痕跡.
        PLAYER_AREA,  //プレイヤーの陣地.
    }

    /// <summary>
    /// プレイしてるゲームモード.
    /// </summary>
    public enum GameMode
    {
        [InspectorName("未設定"), Tooltip("未設定")]
        None,
        [InspectorName("時間制限モード"), Tooltip("時間制限モード")] 
        TimeUp,
        [InspectorName("殲滅モード"),     Tooltip("殲滅モード")] 
        AllBreak
    }

    /// <summary>
    /// グローバル定数.
    /// </summary>
    public static class Gl_Const
    {
        //盤面(board)関係.
        public const int   BOARD_HEI      = 100;
        public const int   BOARD_WID      = 100;
        public const float SQUARE_SIZE    = 0.1f; //マスのサイズ倍率.
        public const int   INIT_AREA_SIZE = 4;    //初期陣地エリアのサイズ(中心から何ドット広げるか)

        //プレイヤー関係.
        public const int   PLAYER_TRAIL_SIZE = 1; //痕跡のサイズ(中心から何ドット広げるか)

        //敵関係.
        public const float MARGIN_TOP    = -2f;   //↓画面の余白.
        public const float MARGIN_RIGHT  = -2f;
        public const float MARGIN_LEFT   = 1.0f;
        public const float MARGIN_BOTTOM = 1.0f;

        public const float ENM_MAX_MOVE_SPEED  = 0.8f;     //移動速度乱数の最大値.
        public const float ENM_MIN_MOVE_SPEED  = 0.1f;     //移動速度乱数の最小値.
        public const float ENM_GOAL_STOP_RANGE = 0.02f;    //目標地点に着いたら移動停止する範囲.
        
        //TimeUpモード.
        public const int   ENM_TIMEUP_INIT_CNT = 3;        //初回の敵の出現数.
        public const int   ENM_TIMEUP_MAX_CNT  = 10;       //敵の同時最大出現数.
        public const float ENM_TIMEUP_MAX_INTERVAL = 3.0f; //敵の生成間隔乱数の最大値.
        public const float ENM_TIMEUP_MIN_INTERVAL = 0.5f; //敵の生成間隔乱数の最小値.
        //AllBreakモード.
        public const int   ENM_ALLBREAK_MAX_CNT = 15;      //初回の敵の出現数.
    }

    /// <summary>
    /// グローバル関数.
    /// </summary>
    public static class Gl_Func
    {
        /// <summary>
        /// 画面の左下と右上の座標を返す処理.
        /// </summary>
        /// <returns></returns>
        public static (Vector3, Vector3) GetWorldWindowSize()
        {
            Vector3 leftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

            return (leftBottom, rightTop);
        }

        /// <summary>
        /// 盤面データを元に設置.
        /// </summary>
        /// <param name="_obj">設置するオブジェクト</param>
        /// <param name="_x">盤面の座標x</param>
        /// <param name="_y">盤面の座標y</param>
        /// <param name="_scale">サイズ倍率</param>
        public static void PlaceOnBoard(GameObject _obj, int _x, int _y)
        {
            //座標を取得.
            Vector2 pos = BPosToWPos(new Vector2Int(_x, _y));
            //配置.
            _obj.transform.position = pos;
            _obj.transform.localScale = Vector2.one * Gl_Const.SQUARE_SIZE;
        }

        /// <summary>
        /// 盤面より外に出ていたら座標を修正する.
        /// </summary>
        /// <param name="pos">元の座標</param>
        /// <returns>変更した座標</returns>
        public static Vector2 LimPosInBoard(Vector2 pos)
        {
            //どこまで移動できるか.
            var limX = Gl_Const.BOARD_WID * Gl_Const.SQUARE_SIZE / 2 - Gl_Const.SQUARE_SIZE / 2;
            var limY = Gl_Const.BOARD_HEI * Gl_Const.SQUARE_SIZE / 2 - Gl_Const.SQUARE_SIZE / 2;

            //横の移動限度(符号はそのまま)
            if (Mathf.Abs(pos.x) > limX)
            {
                pos.x = GetNumSign(pos.x) * limX;
            }
            //縦の移動限度(符号はそのまま)
            if (Mathf.Abs(pos.y) > limY)
            {
                pos.y = GetNumSign(pos.y) * limY;
            }

            return pos; //修正した座標を返す.
        }

        /// <summary>
        /// board配列の中かどうか.
        /// </summary>
        /// <returns></returns>
        public static bool IsInBoard(Vector2Int _pos)
        {
            return (_pos.x >= 0) && (_pos.x < Gl_Const.BOARD_WID) &&
                   (_pos.y >= 0) && (_pos.y < Gl_Const.BOARD_HEI);
        }

        /// <summary>
        /// world座標をboard座標に変換.
        /// </summary>
        public static Vector2Int WPosToBPos(Vector2 _pos)
        {
            //Unity上の座標から、board配列の座標にするとどこになるか計算.
            //盤面の幅が偶数なら半マスずらす.
            float x = (_pos.x / Gl_Const.SQUARE_SIZE) - ((Gl_Const.BOARD_WID % 2 == 0) ? 0.5f : 0);
            float y = (_pos.y / Gl_Const.SQUARE_SIZE) - ((Gl_Const.BOARD_HEI % 2 == 0) ? 0.5f : 0);

            //この地点では世界の中央が座標(0, 0)
            Vector2Int bPos = new Vector2Int(
                Mathf.RoundToInt(x),
                Mathf.RoundToInt(y)
            );
            //盤面の左上が座標(0, 0)となるよう調整.
            bPos.x += Gl_Const.BOARD_WID / 2;
            bPos.y += Gl_Const.BOARD_HEI / 2;

            return bPos;
        }

        /// <summary>
        /// board座標をworld座標に変換.
        /// </summary>
        public static Vector2 BPosToWPos(Vector2Int _pos)
        {
            //board配列の座標から、Unity上の座標にするとどこになるか計算.
            float x = (_pos.x + 0.5f - (float)Gl_Const.BOARD_WID / 2) * Gl_Const.SQUARE_SIZE;
            float y = (_pos.y + 0.5f - (float)Gl_Const.BOARD_HEI / 2) * Gl_Const.SQUARE_SIZE;

            return new Vector2(x, y);
        }

        /// <summary>
        /// プラスかマイナスかを取得.
        /// </summary>
        /// <param name="_num">元の値</param>
        /// <returns>符号(0/+1/-1)</returns>
        public static int GetNumSign(float _num)
        {
            //0なら0、それ以外は符号を返す.
            return (_num == 0) ? 0 : (_num > 0) ? +1 : -1;
        }

        /// <summary>
        /// 4方向を指す座標を取得.
        /// </summary>
        /// <returns></returns>
        public static Vector2Int[] GetVector4dir()
        {
            return new Vector2Int[4] { 
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right 
            };
        }

        /// <summary>
        /// 上下左右の操作(斜め対応)
        /// </summary>
        /// <returns>座標(vec), 角度(ang)</returns>
        public static (Vector2 vec, float ang) InputKey4dir()
        {
            //操作量.
            Vector2 move = new Vector2(
                Input.GetAxisRaw("Horizontal"), //左右.
                Input.GetAxisRaw("Vertical")    //上下.
            );

            float sin = 0, cos = 0, theta = 0;

            //移動してるなら.
            if (move != Vector2.zero)
            {
                //角度(theta:シータ)をラジアンで求める.
                theta = Mathf.Atan2(move.y, move.x); //tan(タンジェント)

                //xとyの移動量を求める.
                sin = Mathf.Sin(theta); //sin = y成分.
                cos = Mathf.Cos(theta); //cos = x成分.

                //ほぼ0の値なら、0とみなす(計算上誤差があるため)
                if (Mathf.Abs(sin) < 0.0001f) { sin = 0; }
                if (Mathf.Abs(cos) < 0.0001f) { cos = 0; }
            }

            //ラジアンを角度へ変換.
            float angle = theta * 180/Mathf.PI - 90; //上が0度になるよう90度回転.

            return (new Vector2(cos, sin), angle);
        }
    }
}

#if false
//吉村作(2025/04/09地点)
public class Common : MonoBehaviour
{
    // 画面端の余白
    public const float MARGIN_TOP = 1.0f;
    public const float MARGIN_RIGHT = 1.0f;
    public const float MARGIN_LEFT = 1.0f;
    public const float MARGIN_BOTTOM = 1.0f;

    // アイテムの生成間隔
    public const float INTERVAL_ITEM_GEN = 1.0f;
    // エリア内のアイテムの最大数
    public const int MAX_ITEM_NUM = 5;

    /// <summary>
    /// 画面の左下と右上の座標を返す処理
    /// </summary>
    /// <returns></returns>
    public static (Vector3, Vector3) GetWorldWindowSize()
    {
        Vector3 leftBottom = Vector3.zero;
        Vector3 rightTop = Vector3.zero;

        leftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);
        rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        return (leftBottom, rightTop);
    }
}

/// <summary>
/// 敵の出現座標の抽選[仕様変更済]
/// </summary>
public static Vector2 RandEnemySpawnPos()
{
    //ワールド座標の取得.
    var (lb, rt) = GetWorldWindowSize();

    //距離の抽選.
    var randX = Random.Range(lb.x + Gl_Const.MARGIN_LEFT, Gl_Const.MARGIN_RIGHT);
    var randY = Random.Range(lb.y + Gl_Const.MARGIN_BOTTOM, Gl_Const.MARGIN_TOP);
    //プラスかマイナスかの抽選.
    var randMoveX = Random.Range(randX, -randX);
    var randMoveY = Random.Range(randY, -randY);

    //座標の抽選結果を返す.
    return new Vector2(randMoveX, randMoveY);
}
#endif