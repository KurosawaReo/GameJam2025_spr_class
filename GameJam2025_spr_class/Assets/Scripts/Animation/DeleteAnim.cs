using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAnim : MonoBehaviour
{
    /// <summary>
    /// �A�j���[�V�����I��.
    /// </summary>
    void AnimationEnd()
    {
        Destroy(gameObject); //���g������.
    }
}
