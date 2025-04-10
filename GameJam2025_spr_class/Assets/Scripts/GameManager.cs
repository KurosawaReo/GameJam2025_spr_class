using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum ModeName
    {
        [InspectorName("時間制限モード"), Tooltip("時間制限モード")] TimeUp,
        [InspectorName("殲滅モード"),     Tooltip("殲滅モード")]     AllBreak
    }

            [Header("共通パラメータ")]
        [Tooltip("ゲームモード選択")]          public ModeName gameMode = 0;
        [Tooltip("リザルトパネル")]            public GameObject resultPanel;
        [Tooltip("リザルト用テキスト")]        public Text resultText;
        [Range(1, 100), Tooltip("動く速度")]   public int resultSpeed = 10;
        [Tooltip("スタート確認用"), ReadOnly]  public bool startFlag = false;
        [Tooltip("ゲーム終了確認"), ReadOnly]  public bool gameOverFlag = false;
        [Tooltip("プレイヤー確認"), ReadOnly]  public bool playerDeadCheck = false;
        [Tooltip("初期エネミー数"), ReadOnly]  public int pastEnemyCount;
        [Tooltip("エネミー残数"),   ReadOnly]  public int presentEnemyCount;

            [Header("時間制限モード パラメータ")]
    [ConditionalDisableInInspector("gameMode", (int)ModeName.TimeUp, conditionalInvisible: false)]
        [Tooltip("時間制限用Set")]             public GameObject TimerUpSet;
    [ConditionalDisableInInspector("gameMode", (int)ModeName.TimeUp, conditionalInvisible: false)]
        [Tooltip("タイマー用テキスト")]        public Text timerText;
    [ConditionalDisableInInspector("gameMode", (int)ModeName.TimeUp, conditionalInvisible: false)]
        [Range(0, 100), Tooltip("ゲーム時間")] public float gameTime;

            [Header("殲滅モード パラメータ")]
    [ConditionalDisableInInspector("gameMode", (int)ModeName.AllBreak, conditionalInvisible: false)]
        [Tooltip("Enemy管理用オブジェクト用")] public GameObject enemyParent;
    [ConditionalDisableInInspector("gameMode", (int)ModeName.AllBreak, conditionalInvisible: false)]
        [Tooltip("殲滅モード用Set")]           public GameObject AllBreakSet;
    [ConditionalDisableInInspector("gameMode", (int)ModeName.AllBreak, conditionalInvisible: false)]
        [Tooltip("残数用テキスト")]            public Text RemainingText;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (startFlag && playerDeadCheck == false)
        {
            switch (gameMode)
            {
                case ModeName.TimeUp:
                    if (gameTime > 0)
                    {
                        ModeTUTimeLapse();
                        GetEnemyCount();
                    }
                    else
                    {
                        gameTime = 0;
                        GameOver();
                        ModeGameClear();
                    }

                    break;
                case ModeName.AllBreak:
                    if (presentEnemyCount > 0)
                    {
                        GetEnemyCount();
                    }
                    else
                    {
                        GameOver();
                        ModeGameClear();
                    }

                    break;
            }
        }
        else if (playerDeadCheck == true) 
        {
            GameOver(); 
            PlayerDeadGameOver();
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
                pastEnemyCount = enemyParent.transform.childCount;
                GetEnemyCount();

                break;
            case ModeName.AllBreak:
                AllBreakSet.SetActive(true);
                RemainingText.text = "残数 : " + presentEnemyCount;
                pastEnemyCount = enemyParent.transform.childCount;
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
        if (gameOverFlag == false)
        {
            resultPanel.SetActive(true);
            gameOverFlag = true;
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

    /// <summary>
    /// モード別クリア処理
    /// </summary>
    void ModeGameClear()
    {

        switch (gameMode)
        {
            case ModeName.TimeUp:
                print("TUクリア");
                resultText.text = "終了！\n敵を" + (pastEnemyCount - presentEnemyCount) + "体倒した！";
                if ((pastEnemyCount - presentEnemyCount) == pastEnemyCount)
                {
                    resultText.text += "\n殲滅おめでとう！";
                }

                break;
            case ModeName.AllBreak:
                print("ABクリア");
                resultText.text = "終了！\n殲滅おめでとう！";

                break;
        }
    }

    /// <summary>
    /// プレイヤー死亡エンド
    /// </summary>
    void PlayerDeadGameOver()
    {
        print("プレイヤー死亡");
        resultText.text = "残念！死んでしまった！\n次はクリアを目指そう！";
    }

    /// <summary>
    /// プレイヤーが死んだときの処理
    /// </summary>
    public void PlayerDead()
    {
        //playerDeadCord
        playerDeadCheck = true;
    }
}
