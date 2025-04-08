using System.Collections.Generic;  // List��Queue���g�����߂̖��O���
using UnityEngine;                // Unity�̊�{�@�\�i�Q�[���I�u�W�F�N�g�Ǘ��Ȃǁj���g������

public class OtamesiGameManager : MonoBehaviour
{
    public GameObject tilePrefab;  // �^�C���̃v���n�u�iInspector�ŃZ�b�g�j
    public int width = 10;         // �t�B�[���h�̉����i10�}�X�j
    public int height = 10;        // �t�B�[���h�̏c���i10�}�X�j

    private OtamesiTile[,] field;         // �^�C�����Ǘ����邽�߂�2�����z��
    private Vector2Int playerPos;  // �v���C���[�̌��݈ʒu�i�O���b�h���W�j
    private List<Vector2Int> trail = new List<Vector2Int>();  // �v���C���[�̈ړ��o�H�i���̋O�Ձj

    // �Q�[���J�n���Ɉ�x�����Ă΂��
    void Start()
    {
        GenerateField();                         // �t�B�[���h�i�O���b�h�j�𐶐�
        playerPos = new Vector2Int(1, 1);        // �v���C���[�̏����ʒu��(1, 1)�ɐݒ�
        GetTile(playerPos).SetType(TileType.Player);    // �v���C���[�̈ʒu�ɁuPlayer�v��ݒ�
        GetTile(playerPos).SetType(TileType.Territory); // �����ʒu���uTerritory�v�i�w�n�j�Ƃ��Đݒ�
    }

    // ���t���[���Ă΂��i���[�U�[�̓��͏����j
    void Update()
    {
        Vector2Int move = Vector2Int.zero;  // �v���C���[�������������i�����͓����Ȃ��j

        // ����L�[�������ꂽ�ꍇ�A������ֈړ�
        if (Input.GetKeyDown(KeyCode.UpArrow)) move = Vector2Int.up;
        // �����L�[�������ꂽ�ꍇ�A�������ֈړ�
        if (Input.GetKeyDown(KeyCode.DownArrow)) move = Vector2Int.down;
        // �����L�[�������ꂽ�ꍇ�A�������ֈړ�
        if (Input.GetKeyDown(KeyCode.LeftArrow)) move = Vector2Int.left;
        // �E���L�[�������ꂽ�ꍇ�A�E�����ֈړ�
        if (Input.GetKeyDown(KeyCode.RightArrow)) move = Vector2Int.right;

        // ��L�ňړ����������܂����ꍇ�A�v���C���[���ړ�
        if (move != Vector2Int.zero)
        {
            MovePlayer(playerPos + move);  // �V�����ʒu�Ƀv���C���[���ړ�
        }
    }

    // �v���C���[��V�����ʒu�Ɉړ�����֐�
    void MovePlayer(Vector2Int newPos)
    {
        if (!InBounds(newPos)) return; // �V�����ʒu���͈͊O�̏ꍇ�͈ړ����Ȃ�

        var current = GetTile(playerPos); // ���݈ʒu�̃^�C�����擾

        // ���݈ʒu���uPlayer�v�Ȃ�΁A������uTrail�v�i���j�ɕς���
        if (current.type == TileType.Player)
            current.SetType(TileType.Trail);  // �v���C���[�̋O�Ղɕϊ�

        // �v���C���[�̈ʒu���X�V
        playerPos = newPos;
        var next = GetTile(playerPos);  // �V�����ʒu�̃^�C�����擾

        // ���̈ʒu���uTerritory�v�i�w�n�j�������ꍇ
        if (next.type == TileType.Territory)
        {
            FillEnclosedArea();  // �h��Ԃ��i�͂܂ꂽ�̈�̏����j
        }
        else
        {
            trail.Add(playerPos);  // �v���C���[�̋O�ՂɌ��݈ʒu��ǉ�
            next.SetType(TileType.Player); // �V�����ʒu���uPlayer�v�ɐݒ�
        }
    }

    // �͂܂ꂽ�̈��h��Ԃ��֐�
    void FillEnclosedArea()
    {
        bool[,] visited = new bool[width, height]; // �K�ꂽ�ꏊ���L�^����2D�z��

        // �O���瓞�B�\�ȗ̈��h��Ԃ��Ŗh��
        void FloodFillFromEdge()
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>(); // ���D��T�����s�����߂̃L���[

            // �O���i�[�j�̏�E���E���E�E����T�����J�n
            for (int x = 0; x < width; x++)
            {
                TryEnqueue(queue, x, 0, visited);              // ��[
                TryEnqueue(queue, x, height - 1, visited);     // ���[
            }
            for (int y = 0; y < height; y++)
            {
                TryEnqueue(queue, 0, y, visited);              // ���[
                TryEnqueue(queue, width - 1, y, visited);      // �E�[
            }

            // ���D��T���œh��Ԃ��镔���𒲂ׂ�
            while (queue.Count > 0)
            {
                Vector2Int p = queue.Dequeue();  // �L���[����ʒu�����o��
                foreach (var d in Directions())  // �㉺���E��4�������m�F
                {
                    Vector2Int np = p + d;  // �אڂ���ʒu���v�Z
                    if (InBounds(np) && !visited[np.x, np.y] &&
                        GetTile(np).type != TileType.Territory)  // �ז��ȃ^�C�����Ȃ����
                    {
                        visited[np.x, np.y] = true;  // �K�ꂽ���Ƃ��L�^
                        queue.Enqueue(np);  // �V�����ʒu���L���[�ɒǉ�
                    }
                }
            }
        }

        FloodFillFromEdge();  // �O�������Flood Fill�����s

        // �K��Ă��Ȃ��i�͂܂ꂽ�j�̈���uTerritory�v�i�w�n�j�ɕϊ�
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tile = GetTile(x, y);  // �e�^�C�����擾
                if (!visited[x, y] && tile.type != TileType.Territory)
                {
                    tile.SetType(TileType.Territory);  // �͂܂ꂽ�̈��w�n�ɂ���
                }
            }
        }

        // �v���C���[���ʂ��������w�n�ɕϊ�
        foreach (var pos in trail)
        {
            GetTile(pos).SetType(TileType.Territory);  // �e����w�n�ɕϊ�
        }

        trail.Clear();  // ���̋L�^�����Z�b�g
    }

    // �w�肳�ꂽ�ʒu���L���[�ɒǉ�����⏕�֐��i�K��Ă��Ȃ���΁j
    void TryEnqueue(Queue<Vector2Int> q, int x, int y, bool[,] visited)
    {
        if (GetTile(x, y).type != TileType.Territory)  // �������̈ʒu���w�n�łȂ����
        {
            q.Enqueue(new Vector2Int(x, y));  // �L���[�ɒǉ�
            visited[x, y] = true;  // �K�ꂽ���Ƃ��L�^
        }
    }

    // �w�肳�ꂽ�O���b�h�ʒu�̃^�C����Ԃ�
    OtamesiTile GetTile(Vector2Int pos) => field[pos.x, pos.y];

    // �ʒu(x, y)�̃^�C����Ԃ�
    OtamesiTile GetTile(int x, int y) => field[x, y];

    // �O���b�h�����ǂ������`�F�b�N����
    bool InBounds(Vector2Int pos) => pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;

    // �t�B�[���h�i�O���b�h�j�𐶐�����֐�
    void GenerateField()
    {
        field = new OtamesiTile[width, height];  // �t�B�[���h��2�����z��Ƃ��ď�����

        // �������ix���j�̃��[�v
        for (int x = 0; x < width; x++)
        {
            // �c�����iy���j�̃��[�v
            for (int y = 0; y < height; y++)
            {
                // �^�C�����C���X�^���X�����Ĕz�u
                var obj = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                var tile = obj.GetComponent<OtamesiTile>();  // Tile�X�N���v�g���擾
                tile.gridPos = new Vector2Int(x, y);  // �O���b�h���W��ݒ�
                field[x, y] = tile;  // ���̃^�C�����t�B�[���h�ɕۑ�
                tile.SetType(TileType.Empty);  // �^�C���̎�ނ�������ԁi��j�ɐݒ�
            }
        }
    }

    // 4�����i��A���A���A�E�j�����X�g�Ƃ��ĕԂ�
    List<Vector2Int> Directions()
    {
        return new List<Vector2Int> {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };
    }
}
