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
        Vector2 pos = transform.position;
        //����̎擾.
        var input = Gl_Func.InputKey4dir();
        
        //���삪�����.
        if(input.vec != Vector2.zero)
        {
            //���ړ�.
            pos += input.vec * moveSpeed * Time.deltaTime;
            
            var limX = Gl_Const.BOARD_WID * Gl_Const.SQUARE_SIZE / 2;
            var limY = Gl_Const.BOARD_HEI * Gl_Const.SQUARE_SIZE / 2;

            //���̈ړ����x(�����͂��̂܂�)
            if (Mathf.Abs(pos.x) > limX)
            {
                pos.x = Gl_Func.GetNumSign(pos.x) * limX;
            }
            //�c�̈ړ����x(�����͂��̂܂�)
            if (Mathf.Abs(pos.y) > limY)
            {
                pos.y = Gl_Func.GetNumSign(pos.y) * limY;
            }

            //����𔽉f.
            transform.position    = pos;
            transform.eulerAngles = new Vector3(0, 0, input.ang);
        }
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
