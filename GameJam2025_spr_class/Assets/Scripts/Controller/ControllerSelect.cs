using Gloval;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerSelect : MonoBehaviour
{

    public enum SelectPosArea
    {
        [InspectorName("セレクト画面")] SelectScene,
        [InspectorName("リザルト画面")] GameScene
    }


    [Header("パラメータ")]
    [SerializeField] SceneTransitions sceneTransitions;
    [Tooltip("使用箇所")] public SelectPosArea selectPosArea = 0;
    [ReadOnly] public int selectID = 0;

    [ConditionalDisableInInspector("selectPosArea", (int)SelectPosArea.GameScene, conditionalInvisible: true)]
    public GameManager gameManager;

    GameObject childObject;

    // Select
    const float modeTUPosSelect = -7f;
    const float modeABPosSelect = 0.3f;

    // Game
    const float modeRestartSelectX = 2;

    const float modeY = -3.2f;



    private void Start()
    {
        childObject = gameObject.transform.GetChild(0).gameObject;
    }



    // Update is called once per frame
    void Update()
    {
        Select(selectPosArea);
    }

    void Select(SelectPosArea selectPosArea)
    {
        if (Input.GetJoystickNames() != null)
        {
            childObject.SetActive(true);

            SelectModeController();


            switch (selectPosArea)
            {
                case SelectPosArea.SelectScene:

                    SelectObject();

                    switch (selectID)
                    {
                        case 0:
                            // コントローラー
                            if (Input.GetKeyDown(KeyCode.JoystickButton0))
                            {
                                print("modeTU");


                                // select画面のやつです
                                // select画面のやつです
                                // select画面のやつです
                                // select画面のやつです
                                // select画面のやつです
                                // select画面のやつです


                                sceneTransitions.SceneLoad(2);
                            }
                            break;
                        case 1:
                            // コントローラー
                            if (Input.GetKeyDown(KeyCode.JoystickButton0))
                            {
                                print("modeAB");

                                // select画面のやつです
                                // select画面のやつです
                                // select画面のやつです
                                // select画面のやつです
                                // select画面のやつです
                                // select画面のやつです
                                // select画面のやつです

                                sceneTransitions.SceneLoad(2);
                            }
                            break;
                    }
                    break;
                case SelectPosArea.GameScene:






                    break;
            }
        }
        else 
        {
            childObject.SetActive(false);
        }
    }

    void SelectObject()
    {
        switch (selectID)
        {
            case 0:
                childObject.transform.position = new Vector3(modeTUPosSelect, 0, 0);
                break;
            case 1:
                childObject.transform.position = new Vector3(modeABPosSelect, 0, 0);
                break;
        }
    }

    void SelectModeController()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            selectID = 1;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            selectID = 0;
        }





    }




}
