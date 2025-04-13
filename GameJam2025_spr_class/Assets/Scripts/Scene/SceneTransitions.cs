/*
   - SceneTransitions.cs -
   �V�[���ړ��p.
   �V�[�����𓯂��ɂ����Ȃ��Ɠ����Ȃ��Ȃ�.
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
