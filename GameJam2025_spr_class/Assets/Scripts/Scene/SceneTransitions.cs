/*
   - SceneTransitions.cs -
   シーン移動用.
   シーン名を同じにさせないと動かなくなる.
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    string[] sceneID = {"TitleScene",
                        "GameScene"};

    public void SceneLoad(int sceneNumber)
    {
        SceneManager.LoadScene(sceneID[sceneNumber]);
    }
}
