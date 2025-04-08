using UnityEngine;

public class OtamesiTile : MonoBehaviour
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
