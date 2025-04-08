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
        public const int   BOARD_HEI = 3;
        public const int   BOARD_WID = 5;

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
        public static void PlaceOnBoard(GameObject _obj, int _x, int _y)
        {
            //サイズ取得.
            var size = _obj.GetComponent<SpriteRenderer>().bounds.size.x;
            //座標を計算(中央基準)
            float x = (_x - Gl_Const.BOARD_WID/2) * size;
            float y = (_y - Gl_Const.BOARD_HEI/2) * size;
            //配置.
            _obj.transform.position = new Vector2(x, y);
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