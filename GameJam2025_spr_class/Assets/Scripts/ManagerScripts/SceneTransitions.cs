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
        //GetComponent<Button>.interactable = false;


        SceneManager.LoadScene(sceneID[sceneNumber]);
    }


    IEnumerator FadeOut()
    {








        yield return null;
    }





}
