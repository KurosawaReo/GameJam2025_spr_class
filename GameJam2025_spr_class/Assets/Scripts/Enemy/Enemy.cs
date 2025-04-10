using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gloval;
using static UnityEditor.PlayerSettings;

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

        pm = GameObject.Find("Player").GetComponent<PlayerManager>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

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
        var pos = transform.position;

        
        var move = targetPos - pos;
        pos += move.normalized * moveSpeed * randMoveSpeedRatio * Time.deltaTime;
        Vector2Int position = Gl_Func.WPosToBPos(pos);

        switch (bm.Board[position.x, position.y].type)
        {
            case BoardType.PLAYER_AREA:
                SetMove();
                break;
            case BoardType.PLAYER_TRAIL:
                pm.PlayerDeath();
                break;
            case BoardType.NONE:
                transform.position = pos;
                break;
        }
        
        if ((targetPos - transform .position).magnitude < MOVE_STOP_LIM)
        {
            SetMove();
        }
    }
    
    void EnemyDeath()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// �ړ��J�n���̏����ƖڕW���W�̌���
    /// </summary>
    void SetMove()
    {
        print("�ړ��J�n");
        state = EnemyState.MOVE;
        
        Vector2 pos = transform.position;
        pos = Gl_Func.LimPosInBoard(pos);
        transform.position = pos;
        
        targetPos = Gl_Func.RandEnemySpawnPos();
        targetPos = Gl_Func.LimPosInBoard(targetPos);
        
        randMoveSpeedRatio = Random.Range(MIN_MOVE_SPEED_RATIO, MAX_MOVE_SPEED_RATIO);
    }
}
