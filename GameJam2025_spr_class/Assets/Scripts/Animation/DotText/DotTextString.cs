using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class DotTextString : MonoBehaviour
{
    DotText DotText;

    static int ONE_MINUTE = 60;

    [SerializeField]
    STRING_TYPE stringType;

    [Tooltip("TIMEóp")]
    [SerializeField]
    GameObject[] secondObj = new GameObject[2];
    [SerializeField]
    bool UseMinute;
    [SerializeField]
    GameObject[] minuteObj;

    int[] objSetInt;

    [Tooltip("NUM_STRINGóp")]
    GameObject[] numString;

    //êîíléÛÇØéÊÇË ïbéÛÇØéÊÇË
    public int getNumSecond;
    int getNumSecondMemo;

    int loopNum = 0;

    

    enum SWITCH_TYPE
    {
        SECONDS,
        MINUTE,
    }


    enum STRING_TYPE
    {
        TIME,
        NUM_STRING,
    }

    // Start is called before the first frame update
    void Start()
    {
        objSetInt = new int[secondObj.Length + minuteObj.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (getNumSecondMemo != getNumSecond)
        {
            getNumSecondMemo = getNumSecond;
            SwitchType();
        }
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
        ReSetNum();
        switch (UseMinute)
        {
            case true:

                objSetInt[secondObj.Length] = getNumSecond / ONE_MINUTE;
                Calculation(SWITCH_TYPE.MINUTE);

                objSetInt[0] = getNumSecond % ONE_MINUTE;
                Calculation(SWITCH_TYPE.SECONDS);


                SetNumObj();
                break;

            case false:

                break;
        }
    }

    void ReSetNum()
    {
        loopNum = 0;
        for (int i = 0; i < objSetInt.Length; i++)
        {
            objSetInt[i] = 0;
        }
    }

    void Calculation(SWITCH_TYPE switchType)
    {
        switch (switchType)
        {
            case SWITCH_TYPE.MINUTE:
                if (objSetInt[secondObj.Length + loopNum] >= 10)
                {
                    Debug.Log("objSetInt[secondObj.Length + loopNum]:" + objSetInt[secondObj.Length + loopNum]);
                    objSetInt[secondObj.Length + loopNum + 1] = objSetInt[secondObj.Length + loopNum] / 10;
                    Debug.Log("objSetInt[secondObj.Length + loopNum + 1]:" + objSetInt[secondObj.Length + loopNum + 1]);
                    objSetInt[secondObj.Length + loopNum] = objSetInt[secondObj.Length + loopNum] % 10;

                    loopNum++;
                     Calculation(SWITCH_TYPE.MINUTE);
                }

                break;

            case SWITCH_TYPE.SECONDS:
                if (objSetInt[0] >= 10)
                {
                    //Debug.Log("objSetInt[0]:" + objSetInt[0]);
                    objSetInt[0 + 1] = objSetInt[0] / 10;
                    objSetInt[0] = objSetInt[0] % 10;
                }
                break;
        }
    }

    void DotNumString()
    {

    }

    void SetNumObj()
    {
        for (int i = 0; i < objSetInt.Length; i++)
        {
            
            if (stringType == STRING_TYPE.TIME)
            {
                switch (i)
                {
                    case 0:
                    case 1:
                        DotText = secondObj[i].GetComponent<DotText>();
                        DotText.dotText = SwitchNumText(objSetInt[i]);
                        //Debug.Log(i + ":" + objSetInt[i]);
                        Debug.Log(i + " retrn:" + DotText.dotText);
                        //Debug.Log(i + ":" + DotText.dotText);
                        break;

                    default:
                        DotText = minuteObj[i - secondObj.Length].GetComponent<DotText>();
                        DotText.dotText = SwitchNumText(objSetInt[i]);
                        //Debug.Log(i + ":" + objSetInt[i]);
                        Debug.Log(i + " retrn:" + DotText.dotText);
                        //Debug.Log(i + ":" + DotText.dotText);
                        break;
                }
            }
        }
    }

    DotText.DOT_TEXT SwitchNumText(int _i)
    {
        switch (_i)
        {
            case 0:
                return DotText.DOT_TEXT.ZERO;

            case 1:
                return DotText.DOT_TEXT.ONE;

            case 2:
                return DotText.DOT_TEXT.TWO;

            case 3:
                return DotText.DOT_TEXT.THREE;

            case 4:
                return DotText.DOT_TEXT.FOUR;

            case 5:
                return DotText.DOT_TEXT.FIVE;

            case 6:
                return DotText.DOT_TEXT.SIX;

            case 7:
                return DotText.DOT_TEXT.SEVEN;

            case 8:
                return DotText.DOT_TEXT.EIGHT;

            case 9:
                return DotText.DOT_TEXT.NINE;
        }
        return DotText.DOT_TEXT.ZERO;
    }
}
