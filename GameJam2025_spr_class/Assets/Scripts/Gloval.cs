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
        public const int BOARD_HEI = 5;
        public const int BOARD_WID = 8;
        public const int SQUARE_SIZE = 1; //�}�X�̃T�C�Y�{��.

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
        /// <param name="_scale">�T�C�Y�{��</param>
        public static void PlaceOnBoard(GameObject _obj, int _x, int _y)
        {
            //���W���v�Z(�����)
            Vector2 pos = BPosToWPos(new Vector2Int(_x, _y));
            //�z�u.
            _obj.transform.position = pos;
        }

        /// <summary>
        /// world���W��board���W�ɕϊ�.
        /// </summary>
        public static Vector2Int WPosToBPos(Vector2 _pos)
        {
            //Unity��̍��W�̍��W����Aboard�z��̍��W�ɂ���Ƃǂ��ɂȂ邩�v�Z.
            //(���E�̒�����0�ɂȂ�悤�ɂ���)
            int x = Mathf.RoundToInt(_pos.x * Gl_Const.SQUARE_SIZE);
            int y = Mathf.RoundToInt(_pos.y * Gl_Const.SQUARE_SIZE);

            return new Vector2Int(x, y);
        }

        /// <summary>
        /// board���W��world���W�ɕϊ�.
        /// </summary>
        public static Vector2 BPosToWPos(Vector2Int _pos)
        {
            //board�z��̍��W����AUnity��̍��W�ɂ���Ƃǂ��ɂȂ邩�v�Z.
            float x = (_pos.x+0.5f - (float)Gl_Const.BOARD_WID/2) * Gl_Const.SQUARE_SIZE;
            float y = (_pos.y+0.5f - (float)Gl_Const.BOARD_HEI/2) * Gl_Const.SQUARE_SIZE;

            return new Vector2(x, y);
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