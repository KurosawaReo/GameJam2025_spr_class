using UnityEngine;
using System;
using UnityEngine.UI;
using Gloval;

public class GameManager : MonoBehaviour
{
    [Header("���ʃp�����[�^")]
        [Tooltip("�I�������Q�[�����[�h")]      public GameMode gameMode = 0;

        [Tooltip("���U���g�p�l��")]            public GameObject resultPanel;
        [Tooltip("���U���g�p�e�L�X�g")]        public Text resultText;
        [Tooltip("�X�^�[�g�m�F�p�l��")]        public GameObject startCheckPanel;

        [Range(1, 30), Tooltip("result�̓������x")]     public int  resultSpeed;  // = 4
        [Range(1, 30), Tooltip("zoom out�̓������x")]   public int  zoomOutSpeed; // = 5

        [Tooltip("�X�^�[�g���Ă��邩"),   ReadOnly]  public bool isStart = false;
        [Tooltip("�Q�[���I�����Ă��邩"), ReadOnly]  public bool isGameEnd = false;

        [Tooltip("�G�l�~�[�c��"),   ReadOnly]  public int   presentEnemyCnt;
        [Tooltip("�G�l�~�[���S��"), ReadOnly]  public int   deathEnemyCnt;

        [Tooltip("�Q�[������"),     ReadOnly]  public float gameTimer;

        [Tooltip("�G��1�̂ł��o��������"), ReadOnly]  public bool isEnemySpawn;

        [Tooltip("main camera"), SerializeField] Camera mainCamera;

    [Header("���Ԑ������[�h �p�����[�^")]
        [ConditionalDisableInInspector("gameMode", (int)GameMode.TimeUp, conditionalInvisible: false)]
            [Tooltip("���Ԑ����pSet")]             public GameObject TimerUpSet;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.TimeUp, conditionalInvisible: false)]
            [Tooltip("�^�C�}�[�p�e�L�X�g")]        public Text timerText;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.TimeUp, conditionalInvisible: false)]
            [Range(0, 100), Tooltip("�Q�[������")] public float resetGameTime;

    [Header("�r�Ń��[�h �p�����[�^")]
        [ConditionalDisableInInspector("gameMode", (int)GameMode.AllBreak, conditionalInvisible: false)]
            [Tooltip("Enemy�Ǘ��p�I�u�W�F�N�g�p")] public GameObject enemyParent;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.AllBreak, conditionalInvisible: false)]
            [Tooltip("�r�Ń��[�h�pSet")]           public GameObject AllBreakSet;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.AllBreak, conditionalInvisible: false)]
            [Tooltip("�c���p�e�L�X�g")]            public Text RemainingText;

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
    /// �Q�[��������.
    /// </summary>
    private void InitGame()
    {
        //�I���������[�h���擾.
        var scptDontDest = GameObject.Find("DontDestroyObj").GetComponent<DontDestroyObj>();
        gameMode = scptDontDest.mode;
        //�Ֆʐ���.
        scptBoardMng.BoardGenerate();

        ResetGame(); //���Z�b�g.
    }

    /// <summary>
    /// �Q�[�����Z�b�g.
    /// </summary>
    public void ResetGame()
    {
        //���[�h�ʃ��Z�b�g.
        switch (gameMode)
        {
            case GameMode.TimeUp:
                TimerUpSet.SetActive(true);
                timerText.text = "Time : " + Mathf.Ceil(resetGameTime);
                break;

            case GameMode.AllBreak:
                AllBreakSet.SetActive(true);
                RemainingText.text = "�c�� : " + 0;
                break;
        }

        //�t���O���Z�b�g.
        isStart      = false;
        isGameEnd    = false;
        isEnemySpawn = false;
        //���l���Z�b�g.
        presentEnemyCnt = 0;
        deathEnemyCnt = 0;
        gameTimer = resetGameTime;

        //�Ֆʃ��Z�b�g.
        scptBoardMng.InitBoard();
        scptBoardMng.DrawBoard();
        //�v���C���[���Z�b�g.
        scptPlayerMng.InitPlayer();
        //�p�l�����Z�b�g.
        startCheckPanel.SetActive(true);
        resultPanel.SetActive(false);
        //�S�Ă̓G������.
        for (int i = 0; i < enemyParent.transform.childCount; i++)
        {
            Destroy(enemyParent.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// �Q�[���S�̂̍X�V����.
    /// </summary>
    private void UpdateGame()
    {
        if (isStart && isGameEnd == false)
        {
            //�v���C���[�X�V.
            scptPlayerMng.UpdatePlayer();

            switch (gameMode)
            {
                case GameMode.TimeUp:
                    //�c�莞��.
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
                    //1�̂ł��G�������.
                    if (GetEnemyCount() > 0)
                    {
                        isEnemySpawn = true;
                    }
                    //1�x�G���o�������Ȃ�.
                    if (isEnemySpawn)
                    {
                        EnemyCount();

                        //���݂̓G��.
                        if (presentEnemyCnt <= 0)
                        {
                            GameEnd();
                            GameEndText();
                        }
                    }
                    break;
            }
        }
        //�Q�[���I�����Ă����.
        else if (isGameEnd == true)
        {
            GameEndLoop();
        }
    }

    /// <summary>
    /// �X�^�[�g�p�l������������.
    /// </summary>
    public void PushStartPanel()
    {
        isStart = true;

        //�J�������Y�[���C��������.
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomIn");

        //���[�h�ʂŏo�����s.
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
    /// ���Ԍ����p.
    /// </summary>
    void TimeLapse()
    {
        gameTimer -= (1 * Time.deltaTime);
        timerText.text = "Time : " + Mathf.Ceil(gameTimer);
        //print(gameTime);
    }

    /// <summary>
    /// ���݂̓G�̐����擾.
    /// </summary>
    /// <returns></returns>
    public int GetEnemyCount()
    {
        int cnt = enemyParent.transform.childCount;
        return cnt;
    }

    /// <summary>
    /// �G�̐��𐔂���.
    /// </summary>
    void EnemyCount()
    {
        presentEnemyCnt = GetEnemyCount();
        RemainingText.text = "�c�� : " + presentEnemyCnt;
    }

    /// <summary>
    /// �v���C���[�����񂾎��ɌĂяo�����.
    /// </summary>
    public void PlayerDead()
    {
        resultText.text = "�c�O�I����ł��܂����I\n�G��" + deathEnemyCnt + "�̓|�����I";
        isGameEnd = true;

        GameEnd();
    }

    /// <summary>
    /// �Q�[���I������(1�x�̂�)
    /// </summary>
    void GameEnd()
    {
        isGameEnd = true;

        //result�p�l����\��.
        resultPanel.SetActive(true);
        //�J�������Y�[���A�E�g������.
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomOut");
    }

    /// <summary>
    /// ���[�h�ʂ̃Q�[���I������.
    /// </summary>
    void GameEndText()
    {
        //���[�h��.
        switch (gameMode)
        {
            case GameMode.TimeUp:
                resultText.text = "�I���I\n�G��" + deathEnemyCnt + "�̓|�����I";
                break;

            case GameMode.AllBreak:
                resultText.text = "�I���I\n�r�ł��߂łƂ��I";
                break;
        }
    }

    /// <summary>
    /// �Q�[���I������(���[�v)
    /// </summary>
    void GameEndLoop()
    {
        var rsltPos = resultPanel.transform.position;
        var cmrPos = mainCamera.transform.position;

        //result�p�l����������.
        if (rsltPos.y > cmrPos.y)
        {
            //y���̈ړ���.
            float y = (rsltPos.y - cmrPos.y) * resultSpeed * Time.deltaTime;
            //������.
            resultPanel.transform.position -= new Vector3(0, y, 0);
        }
        else
        {
            //�t�u��������Œ�.
            resultPanel.transform.position = new Vector2(0, cmrPos.y);
        }

        //camera�𒆉��ɓ�����.
        {
            //y���̈ړ���.
            Vector3 pos = (cmrPos - Vector3.zero) * zoomOutSpeed * Time.deltaTime;
            //������.
            mainCamera.transform.position -= pos;
        }
    }
}
