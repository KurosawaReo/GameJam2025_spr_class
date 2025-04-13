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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            PushStart();
        }
    }

    public void OnMouseDown()
    {
        PushStart();
    }

    /// <summary>
    /// �J�n�A�N�V������������.
    /// </summary>
    private void PushStart()
    {
        gameManager.PushStartPanel(); //�J�n�̍��}.
        gameObject.SetActive(false);  //�p�l������.
    }
}