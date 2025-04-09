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

    [Header("ÉpÉâÉÅÅ[É^")]
    public Image fadePanel;
    public float fadeOutTime;

    void Awake()
    {
        Init();
    }


    void Init()
    {

    }

    public void SceneLoad(int sceneNumber)
    {
        SceneManager.LoadScene(sceneID[sceneNumber]);
    }

    public void FadeOutSceneLoad(int sceneNumber)
    {





        SceneManager.LoadScene(sceneID[sceneNumber]);

    }







}
