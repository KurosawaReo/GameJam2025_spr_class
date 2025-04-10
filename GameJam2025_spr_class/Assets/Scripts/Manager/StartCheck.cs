using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム開始前のクリックパネル.
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
