using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitions : MonoBehaviour
{
    string[] sceneID = {"Title",
                        "01",
                        "Result"};


    public void SceneLoad(int sceneNumber)
    {
        SceneManager.LoadScene(sceneID[sceneNumber]);
    }

    public void FadeOutSceneLoad(int sceneNumber)
    {





        SceneManager.LoadScene(sceneID[sceneNumber]);

    }







}
