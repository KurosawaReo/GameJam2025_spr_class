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
        [Range(1, 100), Tooltip("�������x")]   public int  resultSpeed = 10;

        [Tooltip("�X�^�[�g�m�F"),   ReadOnly]  public bool startFlag = false;
        [Tooltip("�Q�[���I���m�F"), ReadOnly]  public bool gameOverFlag = false;
        [Tooltip("�v���C���[�m�F"), ReadOnly]  public bool playerDeadCheck = false;

        [Tooltip("�G�l�~�[���S��"), ReadOnly]  public int  deathEnemyCnt;
        [Tooltip("�G�l�~�[�c��"),   ReadOnly]  public int  presentEnemyCnt;

        [Tooltip("�G��1�̂ł��o��������"), ReadOnly]  public bool isEnemySpawn;

        [Tooltip("main camera"), SerializeField] Camera mainCamera;

    [Header("���Ԑ������[�h �p�����[�^")]
        [ConditionalDisableInInspector("gameMode", (int)GameMode.TimeUp, conditionalInvisible: false)]
            [Tooltip("���Ԑ����pSet")]             public GameObject TimerUpSet;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.TimeUp, conditionalInvisible: false)]
            [Tooltip("�^�C�}�[�p�e�L�X�g")]        public Text timerText;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.TimeUp, conditionalInvisible: false)]
            [Range(0, 100), Tooltip("�Q�[������")] public float gameTime;

    [Header("�r�Ń��[�h �p�����[�^")]
        [ConditionalDisableInInspector("gameMode", (int)GameMode.AllBreak, conditionalInvisible: false)]
            [Tooltip("Enemy�Ǘ��p�I�u�W�F�N�g�p")] public GameObject enemyParent;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.AllBreak, conditionalInvisible: false)]
            [Tooltip("�r�Ń��[�h�pSet")]           public GameObject AllBreakSet;
        [ConditionalDisableInInspector("gameMode", (int)GameMode.AllBreak, conditionalInvisible: false)]
            [Tooltip("�c���p�e�L�X�g")]            public Text RemainingText;

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
                case GameMode.TimeUp:
                    //�c�莞��.
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
                            GameEndMode();
                        }
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
    /// �������p
    /// </summary>
    void Init()
    {
        //�I���������[�h���擾.
        var scptDontDstry = GameObject.Find("DontDestroyObj").GetComponent<DontDestroyObj>();
        gameMode = scptDontDstry.mode;

        Debug.Log("gameMode:"+gameMode);

        //���[�h��.
        switch (gameMode)
        {
            case GameMode.TimeUp:
                TimerUpSet.SetActive(true);
                timerText.text = "Time : " + Mathf.Ceil(gameTime);
                EnemyCount();
                break;

            case GameMode.AllBreak:
                AllBreakSet.SetActive(true);
                RemainingText.text = "�c�� : " + presentEnemyCnt;
                EnemyCount();
                break;
        }

        //�ŏ���result�p�l���𖳌���.
        resultPanel.SetActive(false);
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
    /// ���Ԍ����p
    /// </summary>
    void ModeTUTimeLapse()
    {
        //print(gameTime);
        gameTime -= (1 * Time.deltaTime);
        timerText.text = "Time : " + Mathf.Ceil(gameTime);
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
        //playerDeadCord
        playerDeadCheck = true;
    }

    /// <summary>
    /// ���[�h�ʂ̃Q�[���I������.
    /// </summary>
    void GameEndMode()
    {
        //���[�h��.
        switch (gameMode)
        {
            case GameMode.TimeUp:
                resultText.text = "�I���I\n�G��" + deathEnemyCnt + "�̓|�����I";
                //if ((pastEnemyCount - presentEnemyCnt) == pastEnemyCount)
                //{
                //    resultText.text += "\n�r�ł��߂łƂ��I";
                //}
                break;

            case GameMode.AllBreak:
                resultText.text = "�I���I\n�r�ł��߂łƂ��I";
                break;
        }
    }
    /// <summary>
    /// �v���C���[���S�̃Q�[���I������.
    /// </summary>
    void GameEndDeadPly()
    {
        resultText.text = "�c�O�I����ł��܂����I\n�G��" + deathEnemyCnt + "�̓|�����I";
    }
    /// <summary>
    /// �Q�[���I������.
    /// </summary>
    void GameEnd()
    {
        //1�x�̂�.
        if (gameOverFlag == false)
        {
            resultPanel.SetActive(true); //result�p�l����\��.
            gameOverFlag = true;
        }

        var rsltPos = resultPanel.transform.position;
        var cmrPos  = mainCamera.transform.position;

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

        //�J�������Y�[���A�E�g������.
        //mainCamera.GetComponent<Animator>().SetTrigger("ZoomOut");
    }
}
