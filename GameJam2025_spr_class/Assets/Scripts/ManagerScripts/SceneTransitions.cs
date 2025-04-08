using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    string[] sceneID = {"Title",
                        "01",
                        "Result"};

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    void Init()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SceneLoad(int sceneNumber)
    {
        SceneManager.LoadScene(sceneID[sceneNumber]);
    }


}
