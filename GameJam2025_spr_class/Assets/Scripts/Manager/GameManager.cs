using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.WSA;
using UnityEngine.UI;
using System.ComponentModel;

public class GameManager : MonoBehaviour
{
    public enum ModeName
    {
        TimeUp,
        AllBreak
    }


    [Header("パラメータ")]
    public ModeName gameMode = 0;
    public float startWaitTime = 3f;
    public bool startFlag = false;

    [Header("TimeUpMode")]
    public GameObject modeTUObjects;
    public Text timerText;
    public float gameTime;

    [Header("AllBreakMode")]
    public GameObject modeABObjects;
    public GameObject resultPanel;
    public int enemyCount;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (startFlag)
        {
            switch (gameMode)
            {
                case ModeName.TimeUp:
                    if (gameTime > 0) { ModeTUTimeLapse(); }
                    else
                    {
                        gameTime = 0;



                    }

                    break;
                case ModeName.AllBreak:
                    if (enemyCount <= 0) { ModeABGameClear(); }

                    break;
            }
        }
    }

    /// <summary>
    /// 初期化用
    /// </summary>
    void Init()
    {
        switch (gameMode)
        {
            case ModeName.TimeUp:
                modeTUObjects.SetActive(true);
                timerText.text = "Time : " + Mathf.Ceil(gameTime);






                break;
            case ModeName.AllBreak:
                modeABObjects.SetActive(true);

                break;
        }
    }

    /// <summary>
    /// 時間減少用
    /// </summary>
    void ModeTUTimeLapse()
    {
        //print(gameTime);
        gameTime -= (1 * Time.deltaTime);
        timerText.text = "Time : " + Mathf.Ceil(gameTime);
    }




















    void ModeABGameClear()
    {
        print("ABkusa");
        if (resultPanel != null)
        {
            print("panel Arutte!");
            resultPanel.SetActive(true);
            StartCoroutine(MoovResultPanel());
        }
    }

    IEnumerator MoovResultPanel()
    {
        GetComponent<SceneTransitions>().SceneLoad(0);



        yield return null;


    }




}
