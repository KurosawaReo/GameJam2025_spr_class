using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{

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
}

