using System.Collections;
using System.Collections.Generic;
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
    EnemyGenerator eg;
    PlayerManager pm;

    

    [Tooltip("��ԕϐ�"), SerializeField]
    EnemyState state = EnemyState.IDLE;

    [Tooltip("�ڕW�n�_") ,SerializeField]
    Vector3 targetPos;
    [Tooltip("�ړ����x")]
    float moveSpeed = 3f;
    [Tooltip("�ړ����x����")]
    float randMoveSpeedRatio;
    [Tooltip("�ړ����x�����̍ő�l")]
    const float MAX_MOVE_SPEED_RATIO = 0.5f;
    [Tooltip("�ړ����x�����̍ŏ��l")]
    const float MIN_MOVE_SPEED_RATIO = 0.1f;
    [Tooltip("�ړ���~��臒l")]
    const float MOVE_STOP_LIM = 0.02f;

    [Tooltip("�ҋ@���Ԍv��")]
    float stateTimer;


    // Start is called before the first frame update
    void Start()
    {
        
        bm = GameObject.Find("BoardManager").GetComponent<BoardManager>();

        eg = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();

        pm = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // ��ԑJ�ڏ���
        State();

        
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
                Idle();
                break;
            case EnemyState.MOVE:
                //�ړ�
                Move();
                break;

        }
    }

    /// <summary>
    /// �ҋ@��ԏ���
    /// </summary>
    void Idle()
    {
        if(stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
        }
        else
        {
            print("�ҋ@�I��");
            SetMove();
        }
    }

    // todo.�G�̓���
    /// <summary>
    /// �G�̈ړ�����  
    /// </summary>
    void Move()
    {
        

        var move = targetPos - transform.position;
        transform.position += move.normalized * moveSpeed * randMoveSpeedRatio * Time.deltaTime;

        // �ՖʊO�ɏo�Ȃ��悤�ɂ��鏈��
        //Vector2 pos = transform.position;
        //pos = Gl_Func.LimPosInBoard(pos);
        //transform.position = pos;

        Vector2Int position = Gl_Func.WPosToBPos(transform.position);
        if(bm.Board[position.x, position.y].type == BoardType.PLAYER_TRAIL)
        {
            pm.PlayerDeath();
        }



        if ((targetPos - transform .position).magnitude < MOVE_STOP_LIM)
        {
            print("�ړ��I��");
            NextMove();
        }
    }

    /// <summary>
    /// ���̓������������_���Ō��߂�
    /// </summary>
    void NextMove()
    {
        var rand = (EnemyState)Random.Range((int)EnemyState.IDLE, (int)EnemyState.MOVE + 1);

        switch (rand)
        {
            case EnemyState.IDLE:
                Idle();
                break;
            case EnemyState.MOVE:
                Move();
                break;
        }
    }

    /// <summary>
    /// �ړ��J�n���̏����ƖڕW���W�̌���
    /// </summary>
    void SetMove()
    {
        print("�ړ��J�n");
        state = EnemyState.MOVE;
        
        //���[���h���W�̎擾
        var (lb, rt) = Gl_Func.GetWorldWindowSize();

        Vector2 pos = transform.position;
        pos = Gl_Func.LimPosInBoard(pos);
        transform.position = pos;
                
        var randX = Random.Range(lb.x + Gl_Const.MARGIN_LEFT, Gl_Const.MARGIN_RIGHT);
        var randY = Random.Range(lb.y + Gl_Const.MARGIN_BOTTOM, Gl_Const.MARGIN_TOP);

        var randMoveX = Random.Range(randX, -randX);
        var randMoveY = Random.Range(randY, -randY);
        if (transform.position.x == randX)
        {
            targetPos = new Vector3(randX, randMoveY, transform.position.z);
        }
        else if (transform.position.x == -randX)
        {

        }

        

        targetPos = Gl_Func.LimPosInBoard(targetPos);
        

        randMoveSpeedRatio = Random.Range(MIN_MOVE_SPEED_RATIO, MAX_MOVE_SPEED_RATIO);

        
    }

    
}
