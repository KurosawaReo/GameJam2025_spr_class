using UnityEngine;

namespace Gloval
{
    /// <summary>
    /// �Ֆʂ̃}�X���.
    /// </summary>
    public enum BoardType
    {
        NONE,         //��.
        PLAYER_TRAIL, //�v���C���[�̑���.
        PLAYER_AREA,  //�v���C���[�̐w�n.
        ENEMY,        //�G.
    }

    /// <summary>
    /// �O���[�o���萔.
    /// </summary>
    public static class Gl_Const
    {
        public const int   BOARD_HEI = 3;
        public const int   BOARD_WID = 5;

        //public const float BOARD_LEFTUP_X = 3;  //�Ֆʂ̍�����Wx.
        //public const float BOARD_LEFTUP_Y = -2; //�Ֆʂ̍�����Wy.
    }

    /// <summary>
    /// �O���[�o���֐�.
    /// </summary>
    public static class Gl_Func
    {
        /// <summary>
        /// �Ֆʃf�[�^�����ɐݒu.
        /// </summary>
        /// <param name="_obj">�ݒu����I�u�W�F�N�g</param>
        /// <param name="_x">�Ֆʂ̍��Wx</param>
        /// <param name="_y">�Ֆʂ̍��Wy</param>
        public static void PlaceOnBoard(GameObject _obj, int _x, int _y)
        {
            //�T�C�Y�擾.
            var size = _obj.GetComponent<SpriteRenderer>().bounds.size.x;
            //���W���v�Z(�����)
            float x = (_x - Gl_Const.BOARD_WID/2) * size;
            float y = (_y - Gl_Const.BOARD_HEI/2) * size;
            //�z�u.
            _obj.transform.position = new Vector2(x, y);
        }

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