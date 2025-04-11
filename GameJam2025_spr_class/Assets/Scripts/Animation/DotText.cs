using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DotText : MonoBehaviour
{
    [Header("表示文字"), SerializeField]
    public DOT_TEXT dotText;
    DOT_TEXT dotTextMemo;


    [Header("ブロック情報"), SerializeField]
    int MAX_BLOCK;

    [SerializeField]
    GameObject[] Dot;

    [Header("テキスト固定"), SerializeField]
    bool Fixation = false;

    [Header("色設定")]

    [SerializeField]
    bool useColor;

    [SerializeField]
    int R;
    [SerializeField]
    int G;
    [SerializeField]
    int B;

    [Header("画像設定"),SerializeField]
    Sprite dotImage;


    public enum DOT_TEXT
    {
        ZERO,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,

        EXCLAMATION_MARK,
        COLON,
        SEMICOLON,

        TEST,
    }


    // Start is called before the first frame update
    void Start()
    {
        SetRGB();
        SetImg();
        DotNum();
    }

    void SetImg()
    {
        if (dotImage == null) return;

        for (int i = 0; i < MAX_BLOCK; i++)
        {
            SpriteRenderer spriteRenderer = Dot[i].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = dotImage;
            }
        }
    }

    void SetRGB()
    {
        // R, G, Bを0-1の範囲に変換
        if(useColor == false)
        {
            R = 255;
            G = 255;
            B = 255;
        }
        Color color = new Color(R / 255f, G / 255f, B / 255f);

        for (int i = 0; i < MAX_BLOCK; i++)
        {
            Renderer renderer = Dot[i].GetComponent<Renderer>();
            renderer.material.color = color;
        }
    }

    void DotNum()
    {
        switch (dotText)
        {
            case DOT_TEXT.ZERO:
                ActiveDot(2, 3, 4, 5, 7, 10, 12, 13, 15, 18, 20, 21, 22, 23);
                break;
            case DOT_TEXT.ONE:
                ActiveDot(3, 8, 13, 14, 15, 16, 17, 18);
                break;
            case DOT_TEXT.TWO:
                ActiveDot(2, 5, 6, 7, 10, 12, 13, 16, 18, 20, 21, 24);
                break;
            case DOT_TEXT.THREE:
                ActiveDot(2, 5, 7, 12, 13, 15, 18, 20, 22, 23);
                break;
            case DOT_TEXT.FOUR:
                ActiveDot(3, 4, 8, 10, 13, 16, 19, 20, 21, 22, 23, 24);
                break;
            case DOT_TEXT.FIVE:
                ActiveDot(1, 2, 3, 5, 7, 9, 12, 13, 15, 18, 19, 22, 23);
                break;
            case DOT_TEXT.SIX:
                ActiveDot(2, 3, 4, 5, 7, 10, 12, 13, 16, 18, 20, 23);
                break;
            case DOT_TEXT.SEVEN:
                ActiveDot(1, 5, 6, 7, 10, 13, 15, 19, 20);
                break;
            case DOT_TEXT.EIGHT:
                ActiveDot(2, 4, 5, 7, 9, 12, 13, 15, 18, 20, 22, 23);
                break;
            case DOT_TEXT.NINE:
                ActiveDot(2, 5, 7, 9, 12, 13, 15, 18, 20, 21, 22, 23);
                break;
            case DOT_TEXT.A:
                ActiveDot(2, 3, 4, 5, 6, 7, 10, 13, 16, 20, 21, 22, 23, 24);
                break;
            case DOT_TEXT.B:
                ActiveDot(1, 2, 3, 4, 5, 6, 7, 9, 12, 13, 15, 18, 20, 22, 23);
                break;
            case DOT_TEXT.C:
                ActiveDot(2, 3, 4, 5, 7, 12, 13, 18, 20, 23);
                break;
            case DOT_TEXT.D:
                ActiveDot(1, 2, 3, 4, 5, 6, 7, 12, 13, 18, 20, 21, 22, 23);
                break;
            case DOT_TEXT.E:
                ActiveDot(1, 2, 3, 4, 5, 6, 7, 9, 12, 13, 15, 18, 19, 24);
                break;
            case DOT_TEXT.F:
                ActiveDot(1, 2, 3, 4, 5, 6, 7, 9, 13, 15, 19);
                break;
            case DOT_TEXT.G:
                ActiveDot(2, 3, 4, 5, 7, 12, 13, 16, 18, 20, 22, 23);
                break;
            case DOT_TEXT.H:
                ActiveDot(1, 2, 3, 4, 5, 6, 9, 15, 19, 20, 21, 22, 23, 24);
                break;
            case DOT_TEXT.J:
                ActiveDot(4, 5, 12, 18, 19, 20, 21, 22, 23);
                break;
            case DOT_TEXT.K:
                ActiveDot(1, 2, 3, 4, 5, 6, 9, 14, 16, 19, 23, 24);
                break;
            case DOT_TEXT.L:
                ActiveDot(1, 2, 3, 4, 5, 6, 12, 18, 24);
                break;
            case DOT_TEXT.N:
                ActiveDot(1, 2, 3, 4, 5, 6, 9, 16, 19, 20, 21, 22, 23, 24);
                break;
            case DOT_TEXT.O:
                ActiveDot(2, 3, 4, 5, 7, 12, 13, 18, 20, 21, 22, 23);
                break;
            case DOT_TEXT.P:
                ActiveDot(1, 2, 3, 4, 5, 6, 7, 10, 13, 16, 20, 21);
                break;
            case DOT_TEXT.Q:
                ActiveDot(2, 3, 4, 5, 7, 12, 13, 17, 18, 20, 21, 22, 24);
                break;
            case DOT_TEXT.R:
                ActiveDot(1, 2, 3, 4, 5, 6, 7, 10, 13, 16, 20, 21, 23, 24);
                break;
            case DOT_TEXT.S:
                ActiveDot(2, 5, 7, 9, 12, 13, 16, 18, 20, 23);
                break;
            case DOT_TEXT.U:
                ActiveDot(1, 2, 3, 4, 5, 12, 18, 19, 20, 21, 22, 23);
                break;
            case DOT_TEXT.X:
                ActiveDot(1, 2, 4, 5, 6, 9, 15, 19, 20, 22, 23, 24);
                break;
            case DOT_TEXT.Z:
                ActiveDot(1, 5, 6, 7, 10, 12, 13, 15, 18, 19, 20, 24);
                break;

            //Five rows
            case DOT_TEXT.I:
                ActiveDot(1, 6, 7, 8, 9, 10, 11, 12, 13, 18);
                break;
            case DOT_TEXT.M:
                ActiveDot(1, 2, 3, 4, 5, 6, 8, 15, 20, 25, 26, 27, 28, 29, 30);
                break;
            case DOT_TEXT.T:
                ActiveDot(1, 7, 13, 14, 15, 16, 17, 18, 19, 25);
                break;
            case DOT_TEXT.V:
                ActiveDot(1, 2, 9, 10, 17, 18, 21, 22, 25, 26);
                break;
            case DOT_TEXT.W:
                ActiveDot(1, 2, 3, 4, 11, 12, 15, 16, 23, 24, 25, 26, 27, 28);
                break;
            case DOT_TEXT.Y:
                ActiveDot(1, 2, 9, 16, 17, 18, 21, 25, 26);
                break;

            case DOT_TEXT.EXCLAMATION_MARK:
                ActiveDot(1, 2, 3, 4, 6);
                break;

            case DOT_TEXT.COLON:
                ActiveDot(2, 5);
                break;

            case DOT_TEXT.SEMICOLON:
                ActiveDot(2, 5, 6);
                break;




            case DOT_TEXT.TEST:
                ActiveDot(30, 15, 7, 3, 1);
                break;

        }

    }

    void ActiveDot(params int[] _activeDot)
    {
        _activeDot = Sort(_activeDot);
        switch (Fixation)
        {
            case true:
                //_activeDot = -1;
                List<int> activeDotList = _activeDot.ToList();
                activeDotList.Add(-1);
                _activeDot = activeDotList.ToArray();

                for (int i = 0; i < _activeDot.Length - 1;)
                {
                    for (int j = 0; j < MAX_BLOCK; j++)
                    {
                        if ((_activeDot[i] - 1) == j)
                        {
                            Dot[j].SetActive(true);
                            i++;
                        }
                        else
                        {
                            Destroy(Dot[j]);
                        }
                    }
                }

                break;

            case false:
                dotTextMemo = dotText;
                if (_activeDot.Length >= MAX_BLOCK) return;
                for (int i = 0; i < MAX_BLOCK; i++)
                {
                    Dot[i].SetActive(false);
                }
                for (int i = 0; i < _activeDot.Length; i++)
                {
                    if (_activeDot[i] > MAX_BLOCK)
                    {
                        Debug.Log("対応できない数値が入っていました");
                        return;
                    }
                    Dot[_activeDot[i] - 1].SetActive(true);
                }
                break;
        }
    }

    int[] Sort(int[] _activeDot)
    {
        for (int i = 0; i < _activeDot.Length - 1; i++)
        {
            for (int j = 0; j < _activeDot.Length - 1 - i; j++)
            {
                if (_activeDot[j] > _activeDot[j + 1])
                {
                    int memo = _activeDot[j];
                    _activeDot[j] = _activeDot[j + 1];
                    _activeDot[j + 1] = memo;
                }
            }
        }
        return _activeDot;
    }

    // Update is called once per frame
    void Update()
    {
        if (Fixation == false)
        {
            if (dotTextMemo != dotText)
            {
                DotNum();
            }
        }
    }
}
