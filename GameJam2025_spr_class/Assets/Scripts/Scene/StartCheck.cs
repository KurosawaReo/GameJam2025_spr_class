using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム開始前のクリックパネル.
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
            //開始の合図.
            gameManager.startFlag = true;
            //カメラをズームインさせる.
            mainCamera.GetComponent<Animator>().SetTrigger("ZoomIn");

            gameObject.SetActive(false); //パネル消滅.
        }
    }

    public void OnMouseDown()
    {
        //開始の合図.
        gameManager.startFlag = true;
        //カメラをズームインさせる.
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomIn");

        gameObject.SetActive(false); //パネル消滅.
    }
}