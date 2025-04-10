using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.WSA;
using UnityEngine.UI;
using System.ComponentModel;
using Unity.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public enum ModeName
    {
        [InspectorName("時間制限モード")]  TimeUp,
        [InspectorName("殲滅モード")]      AllBreak
    }

        [Header("共通パラメータ")]
    [Tooltip("ゲームモード選択")]          public ModeName gameMode = 0;
    [Tooltip("リザルトパネル")]            public GameObject resultPanel;
    [Tooltip("リザルト用テキスト")]        public Text resultText;
    [Range(1, 100), Tooltip("動く速度")]   public int resultSpeed = 10;
    [Tooltip("スタート確認用"), ReadOnly]  public bool startFlag = false;
    [Tooltip("ゲーム終了確認"), ReadOnly]  public bool gameOverCheck = false;
    [Tooltip("初期エネミー数"), ReadOnly]  public int pastEnemyCount;
    [Tooltip("エネミー残数"),   ReadOnly]  public int presentEnemyCount;


    [Header("時間制限モード パラメータ")]
    [Tooltip("時間制限用Set")]             public GameObject TimerUpSet;
    [Tooltip("タイマー用テキスト")]        public Text timerText;
    [Range(0, 100), Tooltip("ゲーム時間")] public float gameTime;

        [Header("殲滅モード パラメータ")]
    [Tooltip("Enemy管理用オブジェクト用")] public GameObject enemyParent;
    [Tooltip("殲滅モード用Set")]           public GameObject AllBreakSet;
    [Tooltip("残数用テキスト")]            public Text RemainingText;


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
                    if (gameTime > 0) 
                    {
                        GameOver();
                        ModeTUTimeLapse(); 
                    }
                    else
                    {
                        gameTime = 0;



                    }

                    break;
                case ModeName.AllBreak:
                    if (presentEnemyCount <= 0) 
                    {
                        GameOver();
                        ModeABGameClear(); 
                    }
                    else
                    {
                        pastEnemyCount = enemyParent.transform.childCount;
                        GetEnemyCount();
                    }

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
                TimerUpSet.SetActive(true);
                timerText.text = "Time : " + Mathf.Ceil(gameTime);
                break;
            case ModeName.AllBreak:
                AllBreakSet.SetActive(true);
                RemainingText.text = "残数 : " + presentEnemyCount;
                GetEnemyCount();
                break;
        }
        resultPanel.SetActive(false);
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

    /// <summary>
    /// 敵の数把握用
    /// </summary>
    void GetEnemyCount()
    {
        presentEnemyCount = enemyParent.transform.childCount;
        RemainingText.text = "残数 : " + presentEnemyCount;
    }



    /// <summary>
    /// 共通ゲーム終了処理
    /// </summary>
    void GameOver()
    {
        if (gameOverCheck == false)
        {
            resultPanel.SetActive(true);
        }

        if (resultPanel.transform.position.y > 0)
        {
            resultPanel.transform.position -= new Vector3(0, resultSpeed * Time.deltaTime, 0);
        }
        else
        {
            resultPanel.transform.position = new Vector2(0, 0);
        }
    }












    void ModeABGameClear()
    {
        print("ABクリア");
    }


}
