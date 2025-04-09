/*
   - Common.cs -
   �S�Ă�script�Ŏg����萔��֐��Ȃǂ��܂Ƃ߂���.
*/
using System.Collections;
using System.Collections.Generic;
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
        // �Ֆ�(board)�Ɋւ���萔.
        public const int   BOARD_HEI = 10;
        public const int   BOARD_WID = 20;
        public const float SQUARE_SIZE = 0.5f; //�}�X�̃T�C�Y�{��.

        // ��ʒ[�̗]��
        public const float MARGIN_TOP = 1.0f;
        public const float MARGIN_RIGHT = 1.0f;
        public const float MARGIN_LEFT = 1.0f;
        public const float MARGIN_BOTTOM = 1.0f;

        // �A�C�e���̐����Ԋu
        public const float INTERVAL_ITEM_GEN = 1.0f;
        // �G���A���̃A�C�e���̍ő吔
        public const int MAX_ITEM_NUM = 5;
    }

    /// <summary>
    /// �O���[�o���֐�.
    /// </summary>
    public static class Gl_Func
    {
        /// <summary>
        /// �v���X���}�C�i�X�����擾.
        /// </summary>
        /// <param name="_num">���̒l</param>
        /// <returns>����(0/+1/-1)</returns>
        public static int GetNumSign(float _num)
        {
            //0�Ȃ�0�A����ȊO�͕�����Ԃ�.
            return (_num == 0) ? 0 : (_num > 0) ? +1 : -1;
        }

        /// <summary>
        /// �㉺���E�̑���(�΂ߑΉ�)
        /// </summary>
        /// <returns>�΂ߑΉ��ς݂̑����</returns>
        public static Vector2 InputKey4dir()
        {
            //�����.
            Vector2 input = new Vector2(
                Input.GetAxisRaw("Horizontal"), //���E.
                Input.GetAxisRaw("Vertical")    //�㉺.
            );

            float sin = 0, cos = 0;

            //���삵�Ă�Ȃ�.
            if (input != Vector2.zero)
            {
                //�p�x(theta:�V�[�^)�����W�A���ŋ��߂�.
                var theta = Mathf.Atan2(input.y, input.x); //tan(�^���W�F���g)

                //x��y�̈ړ��ʂ����߂�.
                sin = Mathf.Sin(theta); //sin = y����.
                cos = Mathf.Cos(theta); //cos = x����.

                //�ق�0�̒l�Ȃ�A0�Ƃ݂Ȃ�(�v�Z��덷�����邽��)
                if (Mathf.Abs(sin) < 0.0001f) { sin = 0; }
                if (Mathf.Abs(cos) < 0.0001f) { cos = 0; }
            }

            //�v�Z��̈ړ��ʂ�Ԃ�.
            return new Vector2(cos, sin);
        }

        /// <summary>
        /// ��ʂ̍����ƉE��̍��W��Ԃ�����.
        /// </summary>
        /// <returns></returns>
        public static (Vector3, Vector3) GetWorldWindowSize()
        {
            Vector3 leftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

            return (leftBottom, rightTop);
        }

        /// <summary>
        /// �Ֆʃf�[�^�����ɐݒu.
        /// </summary>
        /// <param name="_obj">�ݒu����I�u�W�F�N�g</param>
        /// <param name="_x">�Ֆʂ̍��Wx</param>
        /// <param name="_y">�Ֆʂ̍��Wy</param>
        /// <param name="_scale">�T�C�Y�{��</param>
        public static void PlaceOnBoard(GameObject _obj, int _x, int _y)
        {
            //���W���擾.
            Vector2 pos = BPosToWPos(new Vector2Int(_x, _y));
            //�z�u.
            _obj.transform.position   = pos;
            _obj.transform.localScale = Vector2.one * Gl_Const.SQUARE_SIZE;
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
            float x = (_pos.x + 0.5f - (float)Gl_Const.BOARD_WID / 2) * Gl_Const.SQUARE_SIZE;
            float y = (_pos.y + 0.5f - (float)Gl_Const.BOARD_HEI / 2) * Gl_Const.SQUARE_SIZE;

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

#if false
//�g����(2025/04/09�n�_)
public class Common : MonoBehaviour
{
    // ��ʒ[�̗]��
    public const float MARGIN_TOP = 1.0f;
    public const float MARGIN_RIGHT = 1.0f;
    public const float MARGIN_LEFT = 1.0f;
    public const float MARGIN_BOTTOM = 1.0f;

    // �A�C�e���̐����Ԋu
    public const float INTERVAL_ITEM_GEN = 1.0f;
    // �G���A���̃A�C�e���̍ő吔
    public const int MAX_ITEM_NUM = 5;

    /// <summary>
    /// ��ʂ̍����ƉE��̍��W��Ԃ�����
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
#endif