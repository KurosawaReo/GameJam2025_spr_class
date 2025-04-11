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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            //�J�n�̍��}.
            gameManager.startFlag = true;
            //�J�������Y�[���C��������.
            mainCamera.GetComponent<Animator>().SetTrigger("ZoomIn");

            gameObject.SetActive(false); //�p�l������.
        }
    }

    public void OnMouseDown()
    {
        //�J�n�̍��}.
        gameManager.startFlag = true;
        //�J�������Y�[���C��������.
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomIn");

        gameObject.SetActive(false); //�p�l������.
    }
}