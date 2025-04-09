using UnityEngine;
using Gloval;

public class PlayerData 
{
    //private�ϐ�.
    private Vector2Int m_boardPos;

    //������(�R���X�g���N�^)
    public PlayerData(Vector2Int _boardPos)
    {
        m_boardPos = _boardPos;
    }

    //set, get.
    public Vector2Int boardPos
    { 
        get => m_boardPos;
        set => m_boardPos = value;
    }
}

/// <summary>
/// �v���C���[�̃��C���v���O����.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [Header("- value -")]
    [SerializeField] float moveSpeed;

    //�v���C���[�f�[�^.
    PlayerData player = new PlayerData(
        new Vector2Int(0, 0) //boardPos.
    );

    void Start()
    {
        
    }

    void Update()
    {
        InputMove();
        PlySurround();
    }

    /// <summary>
    /// �ړ�����.
    /// </summary>
    private void InputMove()
    {
        //���݈ʒu���擾.
        var pos = transform.position;
        //�ړ���.
        pos.x += Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        pos.y += Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

        //���̈ړ����x(�����͂��̂܂�)
        if (Mathf.Abs(pos.x) > Gl_Const.BOARD_WID * Gl_Const.SQUARE_SIZE/2)
        {
            pos.x = Gl_Func.GetNumSign(pos.x) * Gl_Const.BOARD_WID * Gl_Const.SQUARE_SIZE/2;
        }
        //�c�̈ړ����x(�����͂��̂܂�)
        if (Mathf.Abs(pos.y) > Gl_Const.BOARD_HEI * Gl_Const.SQUARE_SIZE/2)
        {
            pos.y = Gl_Func.GetNumSign(pos.y) * Gl_Const.BOARD_HEI * Gl_Const.SQUARE_SIZE/2;
        }

        //�ړ����s.
        transform.position = pos;
    }

    /// <summary>
    /// �v���C���[���͂�����.
    /// </summary>
    private void PlySurround()
    {
        Vector2Int position = Gl_Func.WPosToBPos(transform.position);
        Debug.Log("position:" + position);
    }
}
