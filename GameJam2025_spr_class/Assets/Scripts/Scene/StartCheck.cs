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
    /// 開始アクションをしたら.
    /// </summary>
    private void PushStart()
    {
        gameManager.PushStartPanel(); //開始の合図.
        gameObject.SetActive(false);  //パネル消滅.
    }
}