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
    [Header("- value -")]
    [SerializeField] float moveSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        var nowPos = transform.position;
        nowPos.x += Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        nowPos.y += Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        transform.position = nowPos;

        Vector2Int position = Gl_Func.WPosToBPos(transform.position);
        Debug.Log("position:"+position);
    }   
}
