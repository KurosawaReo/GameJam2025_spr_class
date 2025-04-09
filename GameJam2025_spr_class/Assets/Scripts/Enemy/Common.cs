using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{

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
}

