using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitions : MonoBehaviour
{
    string[] sceneID = {"Title",
                        "StageSelect",
                        "Stage01"};

    public void SceneLoad(int sceneNumber)
    {
        Debug.Log("num:"+sceneNumber);
        SceneManager.LoadScene(sceneID[sceneNumber]);
    }
}
