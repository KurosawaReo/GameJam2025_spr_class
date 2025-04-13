using UnityEngine;
using System;
using UnityEngine.UI;
using Gloval;

public class GameManager : MonoBehaviour
{
    [Header("共通パラメータ")]
        [Tooltip("選択したゲームモード")]      public GameMode gameMode = 0;

        [Tooltip("リザルトパネル")]            public GameObject resultPanel;
        [Tooltip("リザルト用テキスト")]        public Text resultText;
        [Tooltip("スタート確認パネル")]        public GameObject startCheckPanel;

        [Range(1, 30), Tooltip("resultの動く速度")]     public int  resultSpeed;  // = 4
        [Range(1, 30), Tooltip("zoom outの動く速度")]   public int  zoomOutSpeed; // = 5

        [Tooltip("スタートしているか"),   ReadOnly]  public bool isStart = false;
        [Tooltip("ゲーム終了しているか"), ReadOnly]  public bool isGameEnd = false;

        [Tooltip("エネミー残数"),   ReadOnly]  public int   presentEnemyCnt;
        [Tooltip("エネミー死亡数"), ReadOnly]  public int   deathEnemyCnt;

        [Tooltip("ゲーム時間"),     ReadOnly]  public float gameTimer;

        [Tooltip("敵が1体でも出現したか"), ReadOnly]  public bool isEnemySpawn;

        [Tooltip("main camera"), SerializeField] Camera mainCamera;

    [Header("時間制限モード パラメータ")]
        [ConditionalDisableInInspector("gameMode", (int)GameMode.TimeUp, conditionalInvisible: false)]
            [Tooltip("時間制限用Set")]             public GameObject TimerUpSet;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.TimeUp, conditionalInvisible: false)]
            [Tooltip("タイマー用テキスト")]        public Text timerText;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.TimeUp, conditionalInvisible: false)]
            [Range(0, 100), Tooltip("ゲーム時間")] public float resetGameTime;

    [Header("殲滅モード パラメータ")]
        [ConditionalDisableInInspector("gameMode", (int)GameMode.AllBreak, conditionalInvisible: false)]
            [Tooltip("Enemy管理用オブジェクト用")] public GameObject enemyParent;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.AllBreak, conditionalInvisible: false)]
            [Tooltip("殲滅モード用Set")]           public GameObject AllBreakSet;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.AllBreak, conditionalInvisible: false)]
            [Tooltip("残数用テキスト")]            public Text RemainingText;

    [Header("- script -")]
    [SerializeField] BoardManager   scptBoardMng;
    [SerializeField] PlayerManager  scptPlayerMng;
    [SerializeField] EnemyGenerator scptEnemyGnr;

    void Start()
    {
        InitGame();
    }

    void Update()
    {
        UpdateGame();
    }

    /// <summary>
    /// ゲーム初期化.
    /// </summary>
    private void InitGame()
    {
        //選択したモードを取得.
        var scptDontDest = GameObject.Find("DontDestroyObj").GetComponent<DontDestroyObj>();
        gameMode = scptDontDest.mode;
        //盤面生成.
        scptBoardMng.BoardGenerate();

        ResetGame(); //リセット.
    }

    /// <summary>
    /// ゲームリセット.
    /// </summary>
    public void ResetGame()
    {
        //モード別リセット.
        switch (gameMode)
        {
            case GameMode.TimeUp:
                TimerUpSet.SetActive(true);
                timerText.text = "Time : " + Mathf.Ceil(resetGameTime);
                break;

            case GameMode.AllBreak:
                AllBreakSet.SetActive(true);
                RemainingText.text = "残数 : " + 0;
                break;
        }

        //フラグリセット.
        isStart      = false;
        isGameEnd    = false;
        isEnemySpawn = false;
        //数値リセット.
        presentEnemyCnt = 0;
        deathEnemyCnt = 0;
        gameTimer = resetGameTime;

        //盤面リセット.
        scptBoardMng.InitBoard();
        scptBoardMng.DrawBoard();
        //プレイヤーリセット.
        scptPlayerMng.InitPlayer();
        //パネルリセット.
        startCheckPanel.SetActive(true);
        resultPanel.SetActive(false);
        //全ての敵を消去.
        for (int i = 0; i < enemyParent.transform.childCount; i++)
        {
            Destroy(enemyParent.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// ゲーム全体の更新処理.
    /// </summary>
    private void UpdateGame()
    {
        if (isStart && isGameEnd == false)
        {
            //プレイヤー更新.
            scptPlayerMng.UpdatePlayer();

            switch (gameMode)
            {
                case GameMode.TimeUp:
                    //残り時間.
                    if (gameTimer > 0)
                    {
                        TimeLapse();
                    }
                    else
                    {
                        gameTimer = 0;
                        GameEnd();
                        GameEndText();
                    }
                    break;

                case GameMode.AllBreak:
                    //1体でも敵がいれば.
                    if (GetEnemyCount() > 0)
                    {
                        isEnemySpawn = true;
                    }
                    //1度敵が出現したなら.
                    if (isEnemySpawn)
                    {
                        EnemyCount();

                        //現在の敵数.
                        if (presentEnemyCnt <= 0)
                        {
                            GameEnd();
                            GameEndText();
                        }
                    }
                    break;
            }
        }
        //ゲーム終了していれば.
        else if (isGameEnd == true)
        {
            GameEndLoop();
        }
    }

    /// <summary>
    /// スタートパネルを押した時.
    /// </summary>
    public void PushStartPanel()
    {
        isStart = true;

        //カメラをズームインさせる.
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomIn");

        //モード別で出現実行.
        switch (gameMode)
        {
            case GameMode.TimeUp:
                StartCoroutine(scptEnemyGnr.EnmSpawnTimeUp());
                break;

            case GameMode.AllBreak:
                StartCoroutine(scptEnemyGnr.EnmSpawnAllBreak());
                break;
        }
    }

    /// <summary>
    /// 時間減少用.
    /// </summary>
    void TimeLapse()
    {
        gameTimer -= (1 * Time.deltaTime);
        timerText.text = "Time : " + Mathf.Ceil(gameTimer);
        //print(gameTime);
    }

    /// <summary>
    /// 現在の敵の数を取得.
    /// </summary>
    /// <returns></returns>
    public int GetEnemyCount()
    {
        int cnt = enemyParent.transform.childCount;
        return cnt;
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
        resultText.text = "残念！死んでしまった！\n敵を" + deathEnemyCnt + "体倒した！";
        isGameEnd = true;

        GameEnd();
    }

    /// <summary>
    /// ゲーム終了処理(1度のみ)
    /// </summary>
    void GameEnd()
    {
        isGameEnd = true;

        //resultパネルを表示.
        resultPanel.SetActive(true);
        //カメラをズームアウトさせる.
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomOut");
    }

    /// <summary>
    /// モード別のゲーム終了処理.
    /// </summary>
    void GameEndText()
    {
        //モード別.
        switch (gameMode)
        {
            case GameMode.TimeUp:
                resultText.text = "終了！\n敵を" + deathEnemyCnt + "体倒した！";
                break;

            case GameMode.AllBreak:
                resultText.text = "終了！\n殲滅おめでとう！";
                break;
        }
    }

    /// <summary>
    /// ゲーム終了処理(ループ)
    /// </summary>
    void GameEndLoop()
    {
        var rsltPos = resultPanel.transform.position;
        var cmrPos = mainCamera.transform.position;

        //resultパネルを下げる.
        if (rsltPos.y > cmrPos.y)
        {
            //y軸の移動量.
            float y = (rsltPos.y - cmrPos.y) * resultSpeed * Time.deltaTime;
            //動かす.
            resultPanel.transform.position -= new Vector3(0, y, 0);
        }
        else
        {
            //逆ブレしたら固定.
            resultPanel.transform.position = new Vector2(0, cmrPos.y);
        }

        //cameraを中央に動かす.
        {
            //y軸の移動量.
            Vector3 pos = (cmrPos - Vector3.zero) * zoomOutSpeed * Time.deltaTime;
            //動かす.
            mainCamera.transform.position -= pos;
        }
    }
}
