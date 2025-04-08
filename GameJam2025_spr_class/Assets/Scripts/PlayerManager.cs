using UnityEngine;
using Gloval;

public class PlayerData 
{
    //private変数.
    private Vector2Int m_pos;

    //初期化(コンストラクタ)
    public PlayerData(Vector2Int _pos)
    {
        m_pos = _pos;
    }

    //set, get.
    public Vector2Int pos { get; set; }
}

/// <summary>
/// プレイヤーのメインプログラム.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
