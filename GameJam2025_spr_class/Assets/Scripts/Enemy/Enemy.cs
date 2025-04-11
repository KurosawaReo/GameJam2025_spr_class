using UnityEngine;
using Gloval;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        IDLE,
        MOVE,
    }
    
    BoardManager bm;
    PlayerManager pm;
    GameManager gm;
    //EnemyGenerator eg;

    [Tooltip("��ԕϐ�"), SerializeField]
    EnemyState state = EnemyState.IDLE;

    [Tooltip("�ړ��̖ڕW�n�_") ,SerializeField]
    Vector3 targetPos;
    [Tooltip("�ړ����x")]
    float moveSpeed = 3f;
    [Tooltip("�ړ����x����")]
    float randMoveSpeedRatio;

    [Tooltip("�ҋ@���Ԍv��")] 
    float stateTimer;

    [Tooltip("���ŃA�j���[�V����"), SerializeField]
    GameObject prfbBreakAnim;

    // Start is called before the first frame update
    void Start()
    {
        bm = GameObject.Find("BoardManager").GetComponent<BoardManager>();
        pm = GameObject.Find("Player").      GetComponent<PlayerManager>();
        gm = GameObject.Find("GameManager"). GetComponent<GameManager>();

        //eg = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[�����̂�.
        if (gm.startFlag && !gm.gameOverFlag)
        {
            // ��ԑJ�ڏ���
            State();

            Vector2Int position = Gl_Func.WPosToBPos(transform.position);
            if (bm.Board[position.x, position.y].type == BoardType.PLAYER_AREA)
            {
                EnemyDeath();
            }
        }
    }

    /// <summary>
    /// ��Ԃ��Ƃ̏���
    /// </summary>
    void State()
    {
        switch (state)
        {
            case EnemyState.IDLE:
                //�ҋ@
                EnemyIdle();
                break;
            case EnemyState.MOVE:
                //�ړ�
                EnemyMove();
                break;
        }
    }

    /// <summary>
    /// �ҋ@��ԏ���
    /// </summary>
    void EnemyIdle()
    {
        if(stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
        }
        else
        {
            print("�ҋ@�I��");
            SetGoalMove();
        }
    }

    // todo:�G�̓���
    /// <summary>
    /// �G�̈ړ�����  
    /// </summary>
    void EnemyMove()
    {
        var tmpPos = transform.position; //���ݍ��W�̉��擾.
        var dis = targetPos - tmpPos;    //�ڕW�n�_�܂ł̍��W��(distance)

        //���ňړ������Ƃ���.
        tmpPos += dis.normalized * moveSpeed * randMoveSpeedRatio * Time.deltaTime;
        
        //���̃}�X�ɏ���Ă邩�ŏ����𕪂���.
        Vector2Int bpos = Gl_Func.WPosToBPos(tmpPos);
        switch (bm.Board[bpos.x, bpos.y].type)
        {
            case BoardType.PLAYER_AREA:
                SetGoalMove();
                break;
            case BoardType.PLAYER_TRAIL:
                pm.PlayerDeath();
                break;
            case BoardType.NONE:
                transform.position = tmpPos; //���ۂɈړ�����.
                break;
        }
        
        //�ڕW�n�_�ɒ�������(���͈�)
        if ((targetPos - transform.position).magnitude < Gl_Const.ENM_GOAL_STOP_RANGE)
        {
            SetGoalMove();
        }
    }
    
    void EnemyDeath()
    {
        gm.deathEnemyCnt++;  //���S��+1.
        
        var obj = Instantiate(prfbBreakAnim);
        obj.transform.position = transform.position;
        
        Destroy(gameObject); //���g������.
    }

    /// <summary>
    /// �ړ��J�n���̏����ƁA�ڕW���W�̌���
    /// </summary>
    void SetGoalMove()
    {
        print("�ړ��J�n");
        state = EnemyState.MOVE;
        
        Vector2 pos = transform.position;
        pos = Gl_Func.LimPosInBoard(pos);
        transform.position = pos;

        //�ڕW�n�_�̒��I.
        {
            int rndX = Random.Range(0, Gl_Const.BOARD_WID - 1);
            int rndY = Random.Range(0, Gl_Const.BOARD_HEI - 1);

            //���̍��W�����}�X�Ȃ�.
            if (bm.Board[rndX, rndY].type == BoardType.NONE)
            {
                var wPos = Gl_Func.BPosToWPos(new Vector2Int(rndX, rndY));
                targetPos = Gl_Func.LimPosInBoard(wPos);
            }
        }

        randMoveSpeedRatio = Random.Range(
            Gl_Const.ENM_MIN_MOVE_SPEED, Gl_Const.ENM_MAX_MOVE_SPEED
        );
    }
}
