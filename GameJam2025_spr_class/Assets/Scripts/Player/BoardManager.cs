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
    [Header("- script -")]
    [SerializeField] GameManager scptGameMng;

    [Header("- prefab -")]
    [SerializeField] GameObject prfbSqrBack; //square back:�}�X�̔w�i�p.
    [SerializeField] GameObject prfbSqrType; //square type:�}�X�̎�ޗp.
    [Space]
    [SerializeField] GameObject prfbInObj;   //prefab�����鏊.

    [Header("- image -")]
    [SerializeField] Sprite imgPlyTrail;
    [SerializeField] Sprite imgPlyArea;

    //�Ֆʃf�[�^�̃T�C�Y�����߂�.
    BoardData[,] board = new BoardData[Gl_Const.BOARD_WID, Gl_Const.BOARD_HEI];
    public BoardData[,] Board 
    { 
        get => board; 
        set => board = value;
    }

    Color areaColor; //�G���A�F��RGB.

    /// <summary>
    /// �Ֆʂ𐶐�����.
    /// </summary>
    public void BoardGenerate()
    {
        //�S�}�X���[�v.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                var objBack = Instantiate(prfbSqrBack, prfbInObj.transform);
                var objType = Instantiate(prfbSqrType, prfbInObj.transform);

                //���g�̃f�[�^���쐬.
                board[x, y] = new BoardData();
                //�e�}�X��sprite�����L�^.
                board[x, y].typeSR = objType.GetComponent<SpriteRenderer>();

                //�Ֆʏ�ɐݒu.
                Gl_Func.PlaceOnBoard(objBack, x, y);
                Gl_Func.PlaceOnBoard(objType, x, y);
            }
        }
    }

    /// <summary>
    /// �Ֆʃf�[�^�̏�����.
    /// </summary>
    public void InitBoard()
    {
        RndAreaColor();

        //�S�}�X���[�v.
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                //�����͏����w�n�ɂ���.
                if (x >= Gl_Const.BOARD_WID/2   - Gl_Const.INIT_AREA_SIZE/2 &&
                    x <= Gl_Const.BOARD_WID/2-1 + Gl_Const.INIT_AREA_SIZE/2 &&
                    y >= Gl_Const.BOARD_HEI/2   - Gl_Const.INIT_AREA_SIZE/2 &&
                    y <= Gl_Const.BOARD_HEI/2-1 + Gl_Const.INIT_AREA_SIZE/2
                ){
                    board[x, y].type = BoardType.PLAYER_AREA; //�G���A�ɐݒ�.
                }
                else
                {
                    board[x, y].type = BoardType.NONE; //���ɐݒ�.
                }
            }
        }
    }

    /// <summary>
    /// �Ֆʂ̉摜�X�V.
    /// </summary>
    public void DrawBoard()
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
                        board[x, y].typeSR.color  = Color.white;
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
    /// �F�̒��I.
    /// </summary>
    public void RndAreaColor()
    {
        //����߂̐F�̒��Œ��I.
        float r = Random.Range(0.6f, 1f);
        float g = Random.Range(0.6f, 1f);
        float b = Random.Range(0.6f, 1f);

        areaColor = new Color(r, g, b);
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
        //�͂��̂ɐ����������ǂ���.
        bool isSurround = false;

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
        for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                //�K��ĂȂ����̃}�X������Ȃ�.
                if (!isVisit[x, y] && board[x, y].type == BoardType.NONE)
                {
                    board[x, y].type = BoardType.PLAYER_AREA; //�G���A�Ŗ��߂�.
                    isSurround = true;
                }
            }
        }

        //�͂��̂ɐ��������Ȃ�.
        if (isSurround)
        {
            //�S�}�X���[�v.
            for (int y = 0; y < Gl_Const.BOARD_HEI; y++) {
                for (int x = 0; x < Gl_Const.BOARD_WID; x++) {

                    //�����ƍ��Ղ��G���A�Ŗ��߂�.
                    if (board[x, y].type == BoardType.PLAYER_FOOT ||
                        board[x, y].type == BoardType.PLAYER_TRAIL)
                    {
                        board[x, y].type = BoardType.PLAYER_AREA;
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
