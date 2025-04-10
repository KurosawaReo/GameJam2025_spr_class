using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    [Serializable]
    public class JsonScoreData
    {
        int Score1;
        int Score2;
        int Score3;
        int Score4;
        int Score5;
    }



    string filePath;
    //string jsonPath = "Resources/Jsons/";
    string jsonName = "Score.Json";





    void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void Init()
    {
        filePath = Application.dataPath + "/" + jsonName;

        if (!File.Exists(filePath))
        {
            //Save(data);
        }



    }


    JsonScoreData Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // �t�@�C���ǂݍ��ݎw��
        string json = rd.ReadToEnd();                           // �t�@�C�����e�S�ēǂݍ���
        rd.Close();                                             // �t�@�C������

        return JsonUtility.FromJson<JsonScoreData>(json);            // json�t�@�C�����^�ɖ߂��ĕԂ�
    }
}
