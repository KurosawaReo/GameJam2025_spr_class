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

    DontDestroyObj scptDontDest;


    // Select
    const float modeTUPosSelect = -7f;
    const float modeABPosSelect = 0.3f;


    // Game
    const float modeTitleSelectX = -2.5f;
    const float modeRestartSelectX = 2.875f;
    const float modeY = -3.2f;



    private void Start()
    {
        childObject = gameObject.transform.GetChild(0).gameObject;

        //取得.
        scptDontDest = GameObject.Find("DontDestroyObj").GetComponent<DontDestroyObj>();

    }



    // Update is called once per frame
    void Update()
    {
        Select(selectPosArea);
    }

    void Select(SelectPosArea selectPosArea)
    {
        print(Input.GetJoystickNames()[1]);
        print(Input.GetJoystickNames()[0]);

        if (Input.GetJoystickNames()[0] != string.Empty)
        {
            childObject.SetActive(true);

            SelectModeController();

            SelectObject();

            switch (selectPosArea)
            {
                case SelectPosArea.SelectScene:
                    switch (selectID)
                    {
                        case 0:
                            // コントローラー
                            if (Input.GetKeyDown(KeyCode.JoystickButton0))
                            {
                                print("modeTU");

                                scptDontDest.mode = GameMode.TimeUp;
                                sceneTransitions.SceneLoad(2);
                            }
                            break;
                        case 1:
                            // コントローラー
                            if (Input.GetKeyDown(KeyCode.JoystickButton0))
                            {
                                print("modeAB");

                                scptDontDest.mode = GameMode.AllBreak;
                                sceneTransitions.SceneLoad(2);
                            }
                            break;
                    }
                    break;
                case SelectPosArea.GameScene:
                    switch (selectID)
                    {
                        case 0:
                            // コントローラー
                            if (Input.GetKeyDown(KeyCode.JoystickButton0))
                            {
                                sceneTransitions.SceneLoad(0);
                            }
                            break;
                        case 1:
                            // コントローラー
                            if (Input.GetKeyDown(KeyCode.JoystickButton0))
                            {
                                sceneTransitions.SceneLoad(2);
                            }
                            break;
                    }
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
        switch (selectPosArea)
        {
            case SelectPosArea.SelectScene:
                switch (selectID)
                {
                    case 0:
                        childObject.transform.position = new Vector3(modeTUPosSelect, 0, 0);
                        break;
                    case 1:
                        childObject.transform.position = new Vector3(modeABPosSelect, 0, 0);
                        break;
                }
                break;
            case SelectPosArea.GameScene:
                print("kakunin");
                switch (selectID)
                {
                    case 0:
                        childObject.transform.localPosition = new Vector3(modeTitleSelectX, modeY, 0);
                        break;
                    case 1:
                        childObject.transform.localPosition = new Vector3(modeRestartSelectX, modeY, 0);
                        break;
                }
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
