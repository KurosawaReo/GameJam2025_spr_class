using UnityEngine;
using System;
using UnityEngine.UI;
using Gloval;

public class GameManager : MonoBehaviour
{
    [Header("���ʃp�����[�^")]
        [Tooltip("�Q�[�����[�h�I��")]          public ModeName   gameMode = 0;

        [Tooltip("���U���g�p�l��")]            public GameObject resultPanel;
        [Tooltip("���U���g�p�e�L�X�g")]        public Text resultText;
        [Range(1, 100), Tooltip("�������x")]   public int  resultSpeed = 10;

        [Tooltip("�X�^�[�g�m�F"),   ReadOnly]  public bool startFlag = false;
        [Tooltip("�Q�[���I���m�F"), ReadOnly]  public bool gameOverFlag = false;
        [Tooltip("�v���C���[�m�F"), ReadOnly]  public bool playerDeadCheck = false;

        [Tooltip("�G�l�~�[���S��"), ReadOnly]  public int  deathEnemyCnt;
        [Tooltip("�G�l�~�[�c��"),   ReadOnly]  public int  presentEnemyCnt;

        [Tooltip("main camera"), SerializeField] Camera mainCamera;

    [Header("���Ԑ������[�h �p�����[�^")]
        [ConditionalDisableInInspector("gameMode", (int)ModeName.TimeUp, conditionalInvisible: false)]
            [Tooltip("���Ԑ����pSet")]             public GameObject TimerUpSet;
        [ConditionalDisableInInspector("gameMode", (int)ModeName.TimeUp, conditionalInvisible: false)]
            [Tooltip("�^�C�}�[�p�e�L�X�g")]        public Text timerText;
        [ConditionalDisableInInspector("gameMode", (int)ModeName.TimeUp, conditionalInvisible: false)]
            [Range(0, 100), Tooltip("�Q�[������")] public float gameTime;

    [Header("�r�Ń��[�h �p�����[�^")]
        [ConditionalDisableInInspector("gameMode", (int)ModeName.AllBreak, conditionalInvisible: false)]
            [Tooltip("Enemy�Ǘ��p�I�u�W�F�N�g�p")] public GameObject enemyParent;
        [ConditionalDisableInInspector("gameMode", (int)ModeName.AllBreak, conditionalInvisible: false)]
            [Tooltip("�r�Ń��[�h�pSet")]           public GameObject AllBreakSet;
        [ConditionalDisableInInspector("gameMode", (int)ModeName.AllBreak, conditionalInvisible: false)]
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
    /// ���݂̓G�̐����擾.
    /// </summary>
    /// <returns></returns>
    public int GetEnemyCount()
    {
        return enemyParent.transform.childCount;
    }

    /// <summary>
    /// �������p
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
                RemainingText.text = "�c�� : " + presentEnemyCnt;
                EnemyCount();
                break;
        }
        resultPanel.SetActive(false);
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
            case ModeName.TimeUp:
                resultText.text = "�I���I\n�G��" + deathEnemyCnt + "�̓|�����I";
                //if ((pastEnemyCount - presentEnemyCnt) == pastEnemyCount)
                //{
                //    resultText.text += "\n�r�ł��߂łƂ��I";
                //}
                break;

            case ModeName.AllBreak:
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

        //result�p�l����������.
        if (rsltPos.y > 0)
        {
            float y = rsltPos.y * resultSpeed * Time.deltaTime;     //y���̈ړ���.
            resultPanel.transform.position -= new Vector3(0, y, 0); //������.
        }
        else
        {
            resultPanel.transform.position = new Vector2(0, 0);     //�t�u��������0�ɌŒ�.
        }

        //�J�������Y�[���A�E�g������.
        //mainCamera.GetComponent<Animator>().SetTrigger("ZoomOut");
    }
}
