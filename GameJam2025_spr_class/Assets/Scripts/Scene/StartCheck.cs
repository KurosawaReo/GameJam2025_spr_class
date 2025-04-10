using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���J�n�O�̃N���b�N�p�l��.
/// </summary>
public class StartCheck : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] GameManager gameManager;

    [Header("- camera -")]
    [SerializeField] Camera mainCamera;

    public void OnMouseDown()
    {
        //�J�n�̍��}.
        gameManager.startFlag = true;
        //�J�����A�j���[�V����.
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomIn");

        gameObject.SetActive(false); //�p�l������.
    }
}