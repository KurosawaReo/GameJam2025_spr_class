using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class DotTextString : MonoBehaviour
{
    [SerializeField]
    STRING_TYPE stringType;

    [Tooltip("TIME�p")]
    [SerializeField]
    GameObject[] secondObj = new GameObject[2];
    [SerializeField]
    bool UseMinute;
    [SerializeField]
    GameObject[] minuteObj;

    [Tooltip("NUM_STRING�p")]
    GameObject[] numString;

    //���l�󂯎�� �b�󂯎��
    public int getNumSecond;



    enum STRING_TYPE
    {
        TIME,
        NUM_STRING,
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SwitchType();
    }

    void SwitchType()
    {
        switch (stringType)
        {
            case STRING_TYPE.TIME:
                DotNumTime();
                break;

            case STRING_TYPE.NUM_STRING:
                DotNumString();
                break;
        }
    }

    void DotNumTime()
    {

    }

    void DotNumString()
    {

    }
}
