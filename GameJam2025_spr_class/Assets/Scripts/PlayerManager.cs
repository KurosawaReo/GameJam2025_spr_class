using UnityEngine;
using Gloval;

public class PlayerData 
{
    //private�ϐ�.
    private Vector2Int m_pos;

    //������(�R���X�g���N�^)
    public PlayerData(Vector2Int _pos)
    {
        m_pos = _pos;
    }

    //set, get.
    public Vector2Int pos { get; set; }
}

/// <summary>
/// �v���C���[�̃��C���v���O����.
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
