using System.Collections.Generic;  // ListやQueueを使うための名前空間
using UnityEngine;                // Unityの基本機能（ゲームオブジェクト管理など）を使うため

public class OtamesiGameManager : MonoBehaviour
{
    public GameObject tilePrefab;  // タイルのプレハブ（Inspectorでセット）
    public int width = 10;         // フィールドの横幅（10マス）
    public int height = 10;        // フィールドの縦幅（10マス）

    private OtamesiTile[,] field;         // タイルを管理するための2次元配列
    private Vector2Int playerPos;  // プレイヤーの現在位置（グリッド座標）
    private List<Vector2Int> trail = new List<Vector2Int>();  // プレイヤーの移動経路（線の軌跡）

    // ゲーム開始時に一度だけ呼ばれる
    void Start()
    {
        GenerateField();                         // フィールド（グリッド）を生成
        playerPos = new Vector2Int(1, 1);        // プレイヤーの初期位置を(1, 1)に設定
        GetTile(playerPos).SetType(TileType.Player);    // プレイヤーの位置に「Player」を設定
        GetTile(playerPos).SetType(TileType.Territory); // 初期位置を「Territory」（陣地）として設定
    }

    // 毎フレーム呼ばれる（ユーザーの入力処理）
    void Update()
    {
        Vector2Int move = Vector2Int.zero;  // プレイヤーが動いた方向（初期は動かない）

        // 上矢印キーが押された場合、上方向へ移動
        if (Input.GetKeyDown(KeyCode.UpArrow)) move = Vector2Int.up;
        // 下矢印キーが押された場合、下方向へ移動
        if (Input.GetKeyDown(KeyCode.DownArrow)) move = Vector2Int.down;
        // 左矢印キーが押された場合、左方向へ移動
        if (Input.GetKeyDown(KeyCode.LeftArrow)) move = Vector2Int.left;
        // 右矢印キーが押された場合、右方向へ移動
        if (Input.GetKeyDown(KeyCode.RightArrow)) move = Vector2Int.right;

        // 上記で移動方向が決まった場合、プレイヤーを移動
        if (move != Vector2Int.zero)
        {
            MovePlayer(playerPos + move);  // 新しい位置にプレイヤーを移動
        }
    }

    // プレイヤーを新しい位置に移動する関数
    void MovePlayer(Vector2Int newPos)
    {
        if (!InBounds(newPos)) return; // 新しい位置が範囲外の場合は移動しない

        var current = GetTile(playerPos); // 現在位置のタイルを取得

        // 現在位置が「Player」ならば、それを「Trail」（線）に変える
        if (current.type == TileType.Player)
            current.SetType(TileType.Trail);  // プレイヤーの軌跡に変換

        // プレイヤーの位置を更新
        playerPos = newPos;
        var next = GetTile(playerPos);  // 新しい位置のタイルを取得

        // 次の位置が「Territory」（陣地）だった場合
        if (next.type == TileType.Territory)
        {
            FillEnclosedArea();  // 塗りつぶし（囲まれた領域の処理）
        }
        else
        {
            trail.Add(playerPos);  // プレイヤーの軌跡に現在位置を追加
            next.SetType(TileType.Player); // 新しい位置を「Player」に設定
        }
    }

    // 囲まれた領域を塗りつぶす関数
    void FillEnclosedArea()
    {
        bool[,] visited = new bool[width, height]; // 訪れた場所を記録する2D配列

        // 外から到達可能な領域を塗りつぶしで防ぐ
        void FloodFillFromEdge()
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>(); // 幅優先探索を行うためのキュー

            // 外周（端）の上・下・左・右から探索を開始
            for (int x = 0; x < width; x++)
            {
                TryEnqueue(queue, x, 0, visited);              // 上端
                TryEnqueue(queue, x, height - 1, visited);     // 下端
            }
            for (int y = 0; y < height; y++)
            {
                TryEnqueue(queue, 0, y, visited);              // 左端
                TryEnqueue(queue, width - 1, y, visited);      // 右端
            }

            // 幅優先探索で塗りつぶせる部分を調べる
            while (queue.Count > 0)
            {
                Vector2Int p = queue.Dequeue();  // キューから位置を取り出す
                foreach (var d in Directions())  // 上下左右の4方向を確認
                {
                    Vector2Int np = p + d;  // 隣接する位置を計算
                    if (InBounds(np) && !visited[np.x, np.y] &&
                        GetTile(np).type != TileType.Territory)  // 邪魔なタイルがなければ
                    {
                        visited[np.x, np.y] = true;  // 訪れたことを記録
                        queue.Enqueue(np);  // 新しい位置をキューに追加
                    }
                }
            }
        }

        FloodFillFromEdge();  // 外周からのFlood Fillを実行

        // 訪れていない（囲まれた）領域を「Territory」（陣地）に変換
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tile = GetTile(x, y);  // 各タイルを取得
                if (!visited[x, y] && tile.type != TileType.Territory)
                {
                    tile.SetType(TileType.Territory);  // 囲まれた領域を陣地にする
                }
            }
        }

        // プレイヤーが通った線も陣地に変換
        foreach (var pos in trail)
        {
            GetTile(pos).SetType(TileType.Territory);  // 各線を陣地に変換
        }

        trail.Clear();  // 線の記録をリセット
    }

    // 指定された位置をキューに追加する補助関数（訪れていなければ）
    void TryEnqueue(Queue<Vector2Int> q, int x, int y, bool[,] visited)
    {
        if (GetTile(x, y).type != TileType.Territory)  // もしその位置が陣地でなければ
        {
            q.Enqueue(new Vector2Int(x, y));  // キューに追加
            visited[x, y] = true;  // 訪れたことを記録
        }
    }

    // 指定されたグリッド位置のタイルを返す
    OtamesiTile GetTile(Vector2Int pos) => field[pos.x, pos.y];

    // 位置(x, y)のタイルを返す
    OtamesiTile GetTile(int x, int y) => field[x, y];

    // グリッド内かどうかをチェックする
    bool InBounds(Vector2Int pos) => pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;

    // フィールド（グリッド）を生成する関数
    void GenerateField()
    {
        field = new OtamesiTile[width, height];  // フィールドを2次元配列として初期化

        // 横方向（x軸）のループ
        for (int x = 0; x < width; x++)
        {
            // 縦方向（y軸）のループ
            for (int y = 0; y < height; y++)
            {
                // タイルをインスタンス化して配置
                var obj = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                var tile = obj.GetComponent<OtamesiTile>();  // Tileスクリプトを取得
                tile.gridPos = new Vector2Int(x, y);  // グリッド座標を設定
                field[x, y] = tile;  // そのタイルをフィールドに保存
                tile.SetType(TileType.Empty);  // タイルの種類を初期状態（空）に設定
            }
        }
    }

    // 4方向（上、下、左、右）をリストとして返す
    List<Vector2Int> Directions()
    {
        return new List<Vector2Int> {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };
    }
}
