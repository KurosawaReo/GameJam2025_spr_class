/*
   - TestTile.cs -
   プレイヤーが囲ったマスを埋めるプログラムに使う.
   (chatGPT生成、保管用)
*/
#if true
using UnityEngine;

public enum TileType
{
    Empty,      // .            空白
    Trail,      // *            尻尾
    Territory,  // #            陣地
    Player      // P（可視化用）プレイヤー
}

public class TestTile
{
    public Vector2Int gridPos;
    public TileType type;
    public SpriteRenderer sr;

    public void SetType(TileType newType)
    {
        type = newType;
        sr.color = GetColor(type);
    }

    private Color GetColor(TileType t)
    {
        switch (t)
        {
            case TileType.Empty: return Color.white;
            case TileType.Trail: return Color.cyan;
            case TileType.Territory: return Color.green;
            case TileType.Player: return Color.red;
            default: return Color.black;
        }
    }
}
#endif