using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum ModeName
    {
        [InspectorName("���Ԑ������[�h"), Tooltip("���Ԑ������[�h")] TimeUp,
        [InspectorName("�r�Ń��[�h"),     Tooltip("�r�Ń��[�h")]     AllBreak
    }

            [Header("���ʃp�����[�^")]
        [Tooltip("�Q�[�����[�h�I��")]          public ModeName gameMode = 0;
        [Tooltip("���U���g�p�l��")]            public GameObject resultPanel;
        [Tooltip("���U���g�p�e�L�X�g")]        public Text resultText;
        [Range(1, 100), Tooltip("�������x")]   public int resultSpeed = 10;
        [Tooltip("�X�^�[�g�m�F�p"), ReadOnly]  public bool startFlag = false;
        [Tooltip("�Q�[���I���m�F"), ReadOnly]  public bool gameOverFlag = false;
        [Tooltip("�v���C���[�m�F"), ReadOnly]  public bool playerDeadCheck = false;
        [Tooltip("�����G�l�~�[��"), ReadOnly]  public int pastEnemyCount;
        [Tooltip("�G�l�~�[�c��"),   ReadOnly]  public int presentEnemyCount;

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
    /// �������p
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
                RemainingText.text = "�c�� : " + presentEnemyCount;
                pastEnemyCount = enemyParent.transform.childCount;
                GetEnemyCount();

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
    /// �G�̐��c���p
    /// </summary>
    void GetEnemyCount()
    {
        presentEnemyCount = enemyParent.transform.childCount;
        RemainingText.text = "�c�� : " + presentEnemyCount;
    }

    /// <summary>
    /// ���ʃQ�[���I������
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
    /// ���[�h�ʃN���A����
    /// </summary>
    void ModeGameClear()
    {

        switch (gameMode)
        {
            case ModeName.TimeUp:
                print("TU�N���A");
                resultText.text = "�I���I\n�G��" + (pastEnemyCount - presentEnemyCount) + "�̓|�����I";
                if ((pastEnemyCount - presentEnemyCount) == pastEnemyCount)
                {
                    resultText.text += "\n�r�ł��߂łƂ��I";
                }

                break;
            case ModeName.AllBreak:
                print("AB�N���A");
                resultText.text = "�I���I\n�r�ł��߂łƂ��I";

                break;
        }
    }

    /// <summary>
    /// �v���C���[���S�G���h
    /// </summary>
    void PlayerDeadGameOver()
    {
        print("�v���C���[���S");
        resultText.text = "�c�O�I����ł��܂����I\n���̓N���A��ڎw�����I";
    }

    /// <summary>
    /// �v���C���[�����񂾂Ƃ��̏���
    /// </summary>
    public void PlayerDead()
    {
        //playerDeadCord
        playerDeadCheck = true;
    }
}
