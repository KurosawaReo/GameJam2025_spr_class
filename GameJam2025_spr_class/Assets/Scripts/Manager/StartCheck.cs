using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���J�n�O�̃N���b�N�p�l��.
/// </summary>
public class StartCheck : MonoBehaviour
{
    public GameManager gameManager;

    public void OnMouseDown()
    {
        print("start");
        gameManager.startFlag = true;
        gameObject.SetActive(false);
    }
}
