using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
