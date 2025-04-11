using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAnim : MonoBehaviour
{
    /// <summary>
    /// アニメーション終了.
    /// </summary>
    void AnimationEnd()
    {
        Destroy(gameObject); //自身を消滅.
    }
}
