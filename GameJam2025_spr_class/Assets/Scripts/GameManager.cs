using UnityEngine;
using System;
using UnityEngine.UI;
using Gloval;

public class GameManager : MonoBehaviour
{
    [Header("共通パラメータ")]
        [Tooltip("ゲームモード選択")]          public ModeName   gameMode = 0;

        [Tooltip("リザルトパネル")]            public GameObject resultPanel;
        [Tooltip("リザルト用テキスト")]        public Text resultText;
        [Range(1, 100), Tooltip("動く速度")]   public int  resultSpeed = 10;

        [Tooltip("スタート確認"),   ReadOnly]  public bool startFlag = false;
        [Tooltip("ゲーム終了確認"), ReadOnly]  public bool gameOverFlag = false;
        [Tooltip("プレイヤー確認"), ReadOnly]  public bool playerDeadCheck = false;

        [Tooltip("エネミー死亡数"), ReadOnly]  public int  deathEnemyCnt;
        [Tooltip("エネミー残数"),   ReadOnly]  public int  presentEnemyCnt;

        [Tooltip("main camera"), SerializeField] Camera mainCamera;

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
                        EnemyCount();
                    }
                    else
                    {
                        gameTime = 0;
                        GameEnd();
                        GameEndMode();
                    }
                    break;

                case ModeName.AllBreak:
                    if (presentEnemyCnt > 0)
                    {
                        EnemyCount();
                    }
                    else
                    {
                        GameEnd();
                        GameEndMode();
                    }
                    break;
            }
        }
        else if (playerDeadCheck == true)
        {
            GameEnd(); 
            GameEndDeadPly();
        }
    }

    /// <summary>
    /// 現在の敵の数を取得.
    /// </summary>
    /// <returns></returns>
    public int GetEnemyCount()
    {
        return enemyParent.transform.childCount;
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
                EnemyCount();
                break;

            case ModeName.AllBreak:
                AllBreakSet.SetActive(true);
                RemainingText.text = "残数 : " + presentEnemyCnt;
                EnemyCount();
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
    /// 敵の数を数える.
    /// </summary>
    void EnemyCount()
    {
        presentEnemyCnt = GetEnemyCount();
        RemainingText.text = "残数 : " + presentEnemyCnt;
    }

    /// <summary>
    /// プレイヤーが死んだ時に呼び出される.
    /// </summary>
    public void PlayerDead()
    {
        //playerDeadCord
        playerDeadCheck = true;
    }

    /// <summary>
    /// モード別のゲーム終了処理.
    /// </summary>
    void GameEndMode()
    {
        //モード別.
        switch (gameMode)
        {
            case ModeName.TimeUp:
                resultText.text = "終了！\n敵を" + deathEnemyCnt + "体倒した！";
                //if ((pastEnemyCount - presentEnemyCnt) == pastEnemyCount)
                //{
                //    resultText.text += "\n殲滅おめでとう！";
                //}
                break;

            case ModeName.AllBreak:
                resultText.text = "終了！\n殲滅おめでとう！";
                break;
        }
    }
    /// <summary>
    /// プレイヤー死亡のゲーム終了処理.
    /// </summary>
    void GameEndDeadPly()
    {
        resultText.text = "残念！死んでしまった！\n敵を" + deathEnemyCnt + "体倒した！";
    }
    /// <summary>
    /// ゲーム終了処理.
    /// </summary>
    void GameEnd()
    {
        //1度のみ.
        if (gameOverFlag == false)
        {
            resultPanel.SetActive(true); //resultパネルを表示.
            gameOverFlag = true;
        }

        var rsltPos = resultPanel.transform.position;

        //resultパネルを下げる.
        if (rsltPos.y > 0)
        {
            float y = rsltPos.y * resultSpeed * Time.deltaTime;     //y軸の移動量.
            resultPanel.transform.position -= new Vector3(0, y, 0); //動かす.
        }
        else
        {
            resultPanel.transform.position = new Vector2(0, 0);     //逆ブレしたら0に固定.
        }

        //カメラをズームアウトさせる.
        //mainCamera.GetComponent<Animator>().SetTrigger("ZoomOut");
    }
}
