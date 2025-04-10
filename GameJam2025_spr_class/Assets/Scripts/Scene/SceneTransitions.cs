using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitions : MonoBehaviour
{
    string[] sceneID = {"Title",
                        "StageSelect",
                        "GameScene"};

    public void SceneLoad(int sceneNumber)
    {
        SceneManager.LoadScene(sceneID[sceneNumber]);
    }
}
