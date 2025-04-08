using UnityEngine;

namespace Gloval
{
    /// <summary>
    /// 盤面のマス情報.
    /// </summary>
    public enum Board
    {


    }

    /// <summary>
    /// グローバル定数.
    /// </summary>
    public static class Gl_Const
    {
        public const int BOARD_HEI = 3;
        public const int BOARD_WID = 5;

        /*
        //例:
        public static string[] TEXT =
        {
            "AAA",
            "BBB",
            "CCC",
        };
        */
    }

    /// <summary>
    /// グローバル関数.
    /// </summary>
    public static class Gl_Func
    {
        

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