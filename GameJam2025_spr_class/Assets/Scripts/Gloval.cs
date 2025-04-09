using UnityEngine;

namespace Gloval
{
    /// <summary>
    /// 盤面のマス種類.
    /// </summary>
    public enum BoardType
    {
        NONE,         //無.
        PLAYER_TRAIL, //プレイヤーの足跡.
        PLAYER_AREA,  //プレイヤーの陣地.
        ENEMY,        //敵.
    }

    /// <summary>
    /// グローバル定数.
    /// </summary>
    public static class Gl_Const
    {
        public const int BOARD_HEI = 5;
        public const int BOARD_WID = 8;
        public const int SQUARE_SIZE = 1; //マスのサイズ倍率.

        //public const float BOARD_LEFTUP_X = 3;  //盤面の左上座標x.
        //public const float BOARD_LEFTUP_Y = -2; //盤面の左上座標y.
    }

    /// <summary>
    /// グローバル関数.
    /// </summary>
    public static class Gl_Func
    {
        /// <summary>
        /// 盤面データを元に設置.
        /// </summary>
        /// <param name="_obj">設置するオブジェクト</param>
        /// <param name="_x">盤面の座標x</param>
        /// <param name="_y">盤面の座標y</param>
        /// <param name="_scale">サイズ倍率</param>
        public static void PlaceOnBoard(GameObject _obj, int _x, int _y)
        {
            //座標を計算(中央基準)
            Vector2 pos = BPosToWPos(new Vector2Int(_x, _y));
            //配置.
            _obj.transform.position = pos;
        }

        /// <summary>
        /// world座標をboard座標に変換.
        /// </summary>
        public static Vector2Int WPosToBPos(Vector2 _pos)
        {
            //Unity上の座標の座標から、board配列の座標にするとどこになるか計算.
            //(世界の中央が0になるようにする)
            int x = Mathf.RoundToInt(_pos.x * Gl_Const.SQUARE_SIZE);
            int y = Mathf.RoundToInt(_pos.y * Gl_Const.SQUARE_SIZE);

            return new Vector2Int(x, y);
        }

        /// <summary>
        /// board座標をworld座標に変換.
        /// </summary>
        public static Vector2 BPosToWPos(Vector2Int _pos)
        {
            //board配列の座標から、Unity上の座標にするとどこになるか計算.
            float x = (_pos.x+0.5f - (float)Gl_Const.BOARD_WID/2) * Gl_Const.SQUARE_SIZE;
            float y = (_pos.y+0.5f - (float)Gl_Const.BOARD_HEI/2) * Gl_Const.SQUARE_SIZE;

            return new Vector2(x, y);
        }

        /// <summary>
        /// ゲームプレイ終了.
        /// </summary>
        public static void QuitGame()
        {
            //Unityエディタ実行中.
            if (Application.isEditor)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            //ビルド後.
            else
            {
                Application.Quit();
            }
        }
    }
}