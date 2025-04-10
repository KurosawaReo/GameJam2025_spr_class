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

    public void OnMouseDown()
    {
        //開始の合図.
        gameManager.startFlag = true;
        //カメラアニメーション.
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomIn");

        gameObject.SetActive(false); //パネル消滅.
    }
}