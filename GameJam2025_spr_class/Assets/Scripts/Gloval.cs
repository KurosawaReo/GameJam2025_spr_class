using UnityEngine;

namespace Gloval
{
    /// <summary>
    /// �Ֆʂ̃}�X���.
    /// </summary>
    public enum Board
    {


    }

    /// <summary>
    /// �O���[�o���萔.
    /// </summary>
    public static class Gl_Const
    {
        public const int BOARD_HEI = 3;
        public const int BOARD_WID = 5;

        /*
        //��:
        public static string[] TEXT =
        {
            "AAA",
            "BBB",
            "CCC",
        };
        */
    }

    /// <summary>
    /// �O���[�o���֐�.
    /// </summary>
    public static class Gl_Func
    {
        

        /// <summary>
        /// �Q�[���v���C�I��.
        /// </summary>
        public static void QuitGame()
        {
            //Unity�G�f�B�^���s��.
            if (Application.isEditor)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            //�r���h��.
            else
            {
                Application.Quit();
            }
        }
    }
}