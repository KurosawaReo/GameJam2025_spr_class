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
        PLAYER_FOOT,  //�v���C���[�̑���(���݈ʒu)
        PLAYER_TRAIL, //�v���C���[�̍���.
        PLAYER_AREA,  //�v���C���[�̐w�n.
    }

    /// <summary>
    /// �v���C���Ă�Q�[�����[�h.
    /// </summary>
    public enum GameMode
    {
        [InspectorName("���ݒ�"), Tooltip("���ݒ�")]
        None,
        [InspectorName("���Ԑ������[�h"), Tooltip("���Ԑ������[�h")] 
        TimeUp,
        [InspectorName("�r�Ń��[�h"),     Tooltip("�r�Ń��[�h")] 
        AllBreak
    }

    /// <summary>
    /// �O���[�o���萔.
    /// </summary>
    public static class Gl_Const
    {
        //�Ֆ�(board)�֌W.
        public const int   BOARD_HEI      = 100;
        public const int   BOARD_WID      = 100;
        public const float SQUARE_SIZE    = 0.1f; //�}�X�̃T�C�Y�{��.
        public const int   INIT_AREA_SIZE = 4;    //�����w�n�G���A�̃T�C�Y(���S���牽�h�b�g�L���邩)

        //�v���C���[�֌W.
        public const int   PLAYER_TRAIL_SIZE = 1; //���Ղ̃T�C�Y(���S���牽�h�b�g�L���邩)

        //�G�֌W.
        public const float MARGIN_TOP    = -2f;   //����ʂ̗]��.
        public const float MARGIN_RIGHT  = -2f;
        public const float MARGIN_LEFT   = 1.0f;
        public const float MARGIN_BOTTOM = 1.0f;

        public const float ENM_MAX_MOVE_SPEED  = 0.8f;     //�ړ����x�����̍ő�l.
        public const float ENM_MIN_MOVE_SPEED  = 0.1f;     //�ړ����x�����̍ŏ��l.
        public const float ENM_GOAL_STOP_RANGE = 0.02f;    //�ڕW�n�_�ɒ�������ړ���~����͈�.
        
        //TimeUp���[�h.
        public const int   ENM_TIMEUP_INIT_CNT = 3;        //����̓G�̏o����.
        public const int   ENM_TIMEUP_MAX_CNT  = 10;       //�G�̓����ő�o����.
        public const float ENM_TIMEUP_MAX_INTERVAL = 3.0f; //�G�̐����Ԋu�����̍ő�l.
        public const float ENM_TIMEUP_MIN_INTERVAL = 0.5f; //�G�̐����Ԋu�����̍ŏ��l.
        //AllBreak���[�h.
        public const int   ENM_ALLBREAK_MAX_CNT = 15;      //����̓G�̏o����.
    }

    /// <summary>
    /// �O���[�o���֐�.
    /// </summary>
    public static class Gl_Func
    {
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
            _obj.transform.position = pos;
            _obj.transform.localScale = Vector2.one * Gl_Const.SQUARE_SIZE;
        }

        /// <summary>
        /// �Ֆʂ��O�ɏo�Ă�������W���C������.
        /// </summary>
        /// <param name="pos">���̍��W</param>
        /// <returns>�ύX�������W</returns>
        public static Vector2 LimPosInBoard(Vector2 pos)
        {
            //�ǂ��܂ňړ��ł��邩.
            var limX = Gl_Const.BOARD_WID * Gl_Const.SQUARE_SIZE / 2 - Gl_Const.SQUARE_SIZE / 2;
            var limY = Gl_Const.BOARD_HEI * Gl_Const.SQUARE_SIZE / 2 - Gl_Const.SQUARE_SIZE / 2;

            //���̈ړ����x(�����͂��̂܂�)
            if (Mathf.Abs(pos.x) > limX)
            {
                pos.x = GetNumSign(pos.x) * limX;
            }
            //�c�̈ړ����x(�����͂��̂܂�)
            if (Mathf.Abs(pos.y) > limY)
            {
                pos.y = GetNumSign(pos.y) * limY;
            }

            return pos; //�C���������W��Ԃ�.
        }

        /// <summary>
        /// board�z��̒����ǂ���.
        /// </summary>
        /// <returns></returns>
        public static bool IsInBoard(Vector2Int _pos)
        {
            return (_pos.x >= 0) && (_pos.x < Gl_Const.BOARD_WID) &&
                   (_pos.y >= 0) && (_pos.y < Gl_Const.BOARD_HEI);
        }

        /// <summary>
        /// world���W��board���W�ɕϊ�.
        /// </summary>
        public static Vector2Int WPosToBPos(Vector2 _pos)
        {
            //Unity��̍��W����Aboard�z��̍��W�ɂ���Ƃǂ��ɂȂ邩�v�Z.
            //�Ֆʂ̕��������Ȃ甼�}�X���炷.
            float x = (_pos.x / Gl_Const.SQUARE_SIZE) - ((Gl_Const.BOARD_WID % 2 == 0) ? 0.5f : 0);
            float y = (_pos.y / Gl_Const.SQUARE_SIZE) - ((Gl_Const.BOARD_HEI % 2 == 0) ? 0.5f : 0);

            //���̒n�_�ł͐��E�̒��������W(0, 0)
            Vector2Int bPos = new Vector2Int(
                Mathf.RoundToInt(x),
                Mathf.RoundToInt(y)
            );
            //�Ֆʂ̍��オ���W(0, 0)�ƂȂ�悤����.
            bPos.x += Gl_Const.BOARD_WID / 2;
            bPos.y += Gl_Const.BOARD_HEI / 2;

            return bPos;
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
        /// 4�������w�����W���擾.
        /// </summary>
        /// <returns></returns>
        public static Vector2Int[] GetVector4dir()
        {
            return new Vector2Int[4] { 
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right 
            };
        }

        /// <summary>
        /// �㉺���E�̑���(�΂ߑΉ�)
        /// </summary>
        /// <returns>���W(vec), �p�x(ang)</returns>
        public static (Vector2 vec, float ang) InputKey4dir()
        {
            //�����.
            Vector2 move = new Vector2(
                Input.GetAxisRaw("Horizontal"), //���E.
                Input.GetAxisRaw("Vertical")    //�㉺.
            );

            float sin = 0, cos = 0, theta = 0;

            //�ړ����Ă�Ȃ�.
            if (move != Vector2.zero)
            {
                //�p�x(theta:�V�[�^)�����W�A���ŋ��߂�.
                theta = Mathf.Atan2(move.y, move.x); //tan(�^���W�F���g)

                //x��y�̈ړ��ʂ����߂�.
                sin = Mathf.Sin(theta); //sin = y����.
                cos = Mathf.Cos(theta); //cos = x����.

                //�ق�0�̒l�Ȃ�A0�Ƃ݂Ȃ�(�v�Z��덷�����邽��)
                if (Mathf.Abs(sin) < 0.0001f) { sin = 0; }
                if (Mathf.Abs(cos) < 0.0001f) { cos = 0; }
            }

            //���W�A�����p�x�֕ϊ�.
            float angle = theta * 180/Mathf.PI - 90; //�オ0�x�ɂȂ�悤90�x��].

            return (new Vector2(cos, sin), angle);
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

/// <summary>
/// �G�̏o�����W�̒��I[�d�l�ύX��]
/// </summary>
public static Vector2 RandEnemySpawnPos()
{
    //���[���h���W�̎擾.
    var (lb, rt) = GetWorldWindowSize();

    //�����̒��I.
    var randX = Random.Range(lb.x + Gl_Const.MARGIN_LEFT, Gl_Const.MARGIN_RIGHT);
    var randY = Random.Range(lb.y + Gl_Const.MARGIN_BOTTOM, Gl_Const.MARGIN_TOP);
    //�v���X���}�C�i�X���̒��I.
    var randMoveX = Random.Range(randX, -randX);
    var randMoveY = Random.Range(randY, -randY);

    //���W�̒��I���ʂ�Ԃ�.
    return new Vector2(randMoveX, randMoveY);
}
#endif