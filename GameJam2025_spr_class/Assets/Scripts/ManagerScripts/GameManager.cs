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
        [InspectorName("���Ԑ������[�h")]  TimeUp,
        [InspectorName("�r�Ń��[�h")]      AllBreak
    }

        [Header("���ʃp�����[�^")]
    [Tooltip("�Q�[�����[�h�I��")]          public ModeName gameMode = 0;
    [Tooltip("���U���g�p�l��")]            public GameObject resultPanel;
    [Tooltip("���U���g�p�e�L�X�g")]        public Text resultText;
    [Range(1, 100), Tooltip("�������x")]   public int resultSpeed = 10;
    [Tooltip("�X�^�[�g�m�F�p"), ReadOnly]  public bool startFlag = false;
    [Tooltip("�Q�[���I���m�F"), ReadOnly]  public bool gameOverCheck = false;
    [Tooltip("�����G�l�~�[��"), ReadOnly]  public int pastEnemyCount;
    [Tooltip("�G�l�~�[�c��"),   ReadOnly]  public int presentEnemyCount;


    [Header("���Ԑ������[�h �p�����[�^")]
    [Tooltip("���Ԑ����pSet")]             public GameObject TimerUpSet;
    [Tooltip("�^�C�}�[�p�e�L�X�g")]        public Text timerText;
    [Range(0, 100), Tooltip("�Q�[������")] public float gameTime;

        [Header("�r�Ń��[�h �p�����[�^")]
    [Tooltip("Enemy�Ǘ��p�I�u�W�F�N�g�p")] public GameObject enemyParent;
    [Tooltip("�r�Ń��[�h�pSet")]           public GameObject AllBreakSet;
    [Tooltip("�c���p�e�L�X�g")]            public Text RemainingText;


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
    /// �������p
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
                RemainingText.text = "�c�� : " + presentEnemyCount;
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
        print("AB�N���A");
    }


}
