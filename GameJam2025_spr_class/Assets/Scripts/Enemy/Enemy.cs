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

    EnemyGenerator eg;

    [Tooltip("��ԕϐ�"), SerializeField]
    EnemyState state = EnemyState.IDLE;

    [Tooltip("�ڕW�n�_") ,SerializeField]
    Vector3 targetPos;
    [Tooltip("�ړ����x")]
    float moveSpeed = 3f;
    [Tooltip("�ړ����x����")]
    float randMoveSpeedRatio;
    [Tooltip("�ړ����x�����̍ő�l")]
    const float MAX_MOVE_SPEED_RATIO = 1.5f;
    [Tooltip("�ړ����x�����̍ŏ��l")]
    const float MIN_MOVE_SPEED_RATIO = 0.9f;
    [Tooltip("�ړ���~��臒l")]
    const float MOVE_STOP_LIM = 0.02f;

    [Tooltip("�ҋ@���Ԍv��")]
    float stateTimer;


    // Start is called before the first frame update
    void Start()
    {
        eg = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
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
    
        if((targetPos - transform .position).magnitude < MOVE_STOP_LIM)
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

        
        Vector2Int position = Gl_Func.WPosToBPos(transform.position);
        Debug.Log("position:"+position);

        var randX = Random.Range(lb.x + Gl_Const.MARGIN_LEFT + 2, rt.x - Gl_Const.MARGIN_RIGHT - 2);
        var randY = Random.Range(lb.y + Gl_Const.MARGIN_BOTTOM + 2, rt.y - Gl_Const.MARGIN_TOP - 2);

        targetPos = new Vector3(randX, randY, transform.position.z);

        randMoveSpeedRatio = Random.Range(MIN_MOVE_SPEED_RATIO, MAX_MOVE_SPEED_RATIO);

        
    }

    

   

}
