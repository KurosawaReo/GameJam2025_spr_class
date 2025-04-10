/*
   - BoardManager.cs -
   �ŏ��ɔՖʂ���鎞�̂݁A�S�}�X��prefab�𐶐���
   ����ȍ~�́A�X�V������x��prefab�̉摜�������ւ���.
*/
using System.Collections.Generic; //List��Queue���g�����߂ɕK�v.
using Gloval;
using UnityEngine;

/// <summary>
/// �Ֆʂ̊e�}�X�ɐݒ肷��f�[�^.
/// </summary>
public class BoardData
{
    public BoardType      type   { get; set; } //���̃}�X��.
    public SpriteRenderer typeSR { get; set; } //�}�X�ɔz�u����sprite(�摜���)
}

/// <summary>
/// �Ֆʂ̊Ǘ��v���O����.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbSqrBack; //square back:�}�X�̔w�i�p.
    [SerializeField] GameObject prfbSqrType; //square type:�}�X�̎�ޗp.
    [Space]
    [SerializeField] GameObject prfbInObj;   //prefab�����鏊.

    [Header("- image -")]
    [SerializeField] Sprite imgPlyTrail;
    [SerializeField] Sprite imgPlyArea;

    //�Ֆʂ̊e�}�X�f�[�^.
    BoardData[,] board = new BoardData[Gl_Const.BOARD_WID, Gl_Const.BOARD_HEI];
    public BoardData[,] Board 
    { 
        get => board; 
        set => board = value; 
    }

    Color areaColor; //�G���A�F��RGB.


    void Start()
    {
        RandAreaColor();
        InitBoard();
        BoardGenerate();
    }

    void Update()
    {
        UpdateBoard();
    }

    /// <summary>
    /// �F�̒��I.
    /// </summary>
    private void RandAreaColor()
    {
        //����߂̐F�̒��Œ��I.
        float r = Random.Range(0.5f, 1f);
        float g = Random.Range(0.5f, 1f);
        float b = Random.Range(0.5f, 1f);

        areaColor = new Color(r, g, b);
    }

    /// <summary>
    /// �Ֆʃf�[�^�̏�����.
    /// </summary>
    private void InitBoard()
    {
        //�S�}�X���[�v.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                board[x, y] = new BoardData();
                board[x, y].type = BoardType.NONE; //���ɐݒ�.
            }
        }

        int len = 2;

        //����������͏����w�n�ɂ���.
        for (int y = Gl_Const.BOARD_HEI/2-len; y < Gl_Const.BOARD_HEI/2+len; y++) {
            for (int x = Gl_Const.BOARD_WID/2-2; x < Gl_Const.BOARD_WID/2+2; x++) {

                board[x, y].type = BoardType.PLAYER_AREA;
            }
        }
    }

    /// <summary>
    /// �Ֆʂ𐶐�����.
    /// </summary>
    private void BoardGenerate()
    {
        //1�}�X����.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                var objBack = Instantiate(prfbSqrBack, prfbInObj.transform);
                var objType = Instantiate(prfbSqrType, prfbInObj.transform);

                //�e�}�X��sprite�����L�^.
                board[x, y].typeSR = objType.GetComponent<SpriteRenderer>();
                
                //�Ֆʏ�ɐݒu.
                Gl_Func.PlaceOnBoard(objBack, x, y);
                Gl_Func.PlaceOnBoard(objType, x, y);
            }
        }
    }

    /// <summary>
    /// �Ֆʂ̍X�V.
    /// </summary>
    private void UpdateBoard()
    {
        //1�}�X����.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                //��ޕʂŉ摜���Z�b�g.
                switch (board[x, y].type) 
                { 
                    case BoardType.NONE:
                        board[x, y].typeSR.sprite = null; 
                        break;
                    case BoardType.PLAYER_TRAIL:
                        board[x, y].typeSR.sprite = imgPlyTrail;
                        board[x, y].typeSR.color  = areaColor;
                        break;
                    case BoardType.PLAYER_AREA:
                        board[x, y].typeSR.sprite = imgPlyArea; 
                        board[x, y].typeSR.color  = areaColor;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// �v���C���[���Ղň͂�����.
    /// </summary>
    public void SurroundTrail()
    {
        //����queue�ɓ��������W�̃}�X�𒲂אs����.
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        //�K�ꂽ�}�X�̋L�^�p.
        bool[,] isVisit = new bool[Gl_Const.BOARD_WID, Gl_Const.BOARD_HEI];
        //������.
        for (int x = 0; x < Gl_Const.BOARD_WID; x++) {
            for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
                isVisit[x, y] = false;
            }
        }
        //1�ł��K��ĂȂ��}�X�����邩�ǂ���.
        bool isNoVisit = false;

        //board�̏�[�Ɖ��[.
        for (int x = 0; x < Gl_Const.BOARD_WID; x++)
        {
            TryEnqueue(queue, x, 0,                    isVisit);
            TryEnqueue(queue, x, Gl_Const.BOARD_HEI-1, isVisit);
        }
        //board�̍��[�ƉE�[.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++)
        {
            TryEnqueue(queue, 0,                    y, isVisit);
            TryEnqueue(queue, Gl_Const.BOARD_WID-1, y, isVisit);
        }

        //���T���}�X���Ȃ��Ȃ�܂Ń��[�v.
        while (queue.Count > 0)
        {
            var pos = queue.Dequeue(); //���T���}�X�̍��W��1���o��.

            //4����(�㉺���E)���[�v.
            foreach (var dir in Gl_Func.GetVector4dir()) 
            {
                var newPos = pos + dir; //�ׂ̃}�X�Ɉړ�.

                //�Ֆʂ̒��ɂ��� and �K�ꂽ���Ƃ��Ȃ��Ȃ�.
                if (Gl_Func.IsInBoard(newPos) && !isVisit[newPos.x, newPos.y])
                {
                    TryEnqueue(queue, newPos.x, newPos.y, isVisit); //���̒T�����}�X.
                }
            }
        }

        //�S�}�X���[�v.
        for (int x = 0; x < Gl_Const.BOARD_WID; x++) {
            for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {

                //�K��ĂȂ����̃}�X������Ȃ�.
                if (!isVisit[x, y] && board[x, y].type == BoardType.NONE)
                {
                    isNoVisit = true;
                    break;
                }
            }
            //����������I��.
            if (isNoVisit) { break; }
        }

        //�K��ĂȂ��}�X������Ȃ�.
        if (isNoVisit)
        {
            //�S�}�X���[�v.
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {
                for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {

                    //�v���C���[�̍��Ճ}�X.
                    if (board[x, y].type == BoardType.PLAYER_TRAIL)
                    {
                        board[x, y].type = BoardType.PLAYER_AREA; //�G���A�Ŗ��߂�.
                    }
                    //�K��ĂȂ����̃}�X(=�͂�ꂽ�͈�)
                    else if (!isVisit[x, y] && board[x, y].type == BoardType.NONE)
                    {
                        board[x, y].type = BoardType.PLAYER_AREA; //�G���A�Ŗ��߂�.
                    }
                }
            }
        }
    }

    /// <summary>
    /// �T�����}�X.
    /// �����Ȃ��}�X�Ȃ�queue�ɒǉ�.
    /// </summary>
    private void TryEnqueue(Queue<Vector2Int> _queue, int _x, int _y, bool[,] _isVisit)
    {
        //�܂������Ȃ��Ȃ�.
        if (board[_x, _y].type == BoardType.NONE)
        {
            _queue.Enqueue(new Vector2Int(_x, _y)); //�T������}�X�ɒǉ�.
            _isVisit[_x, _y] = true;                //�����͖K���.
        }
    }
}
