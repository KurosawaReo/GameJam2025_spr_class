using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Serializable]
    public class GenerateItem
    {
        [Tooltip("生成するプレハブ"), SerializeField]
        public GameObject prefabItem;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
