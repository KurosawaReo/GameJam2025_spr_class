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

    [Tooltip("状態変数"), SerializeField]
    EnemyState state = EnemyState.IDLE;

    [Tooltip("目標地点") ,SerializeField]
    Vector3 targetPos;
    [Tooltip("移動速度")]
    float moveSpeed = 3f;
    [Tooltip("移動速度乱数")]
    float randMoveSpeedRatio;
    [Tooltip("移動速度乱数の最大値")]
    const float MAX_MOVE_SPEED_RATIO = 0.5f;
    [Tooltip("移動速度乱数の最小値")]
    const float MIN_MOVE_SPEED_RATIO = 0.1f;
    [Tooltip("移動停止の閾値")]
    const float MOVE_STOP_LIM = 0.02f;

    [Tooltip("待機時間計測")]
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
        //ゲーム中のみ.
        if (gm.startFlag && !gm.gameOverFlag)
        {
            // 状態遷移処理
            State();

            Vector2Int position = Gl_Func.WPosToBPos(transform.position);
            if (bm.Board[position.x, position.y].type == BoardType.PLAYER_AREA)
            {
                EnemyDeath();
            }
        }
    }

    /// <summary>
    /// 状態ごとの処理
    /// </summary>
    void State()
    {
        switch (state)
        {
            case EnemyState.IDLE:
                //待機
                Idle();
                break;
            case EnemyState.MOVE:
                //移動
                Move();
                break;

        }
    }

    /// <summary>
    /// 待機状態処理
    /// </summary>
    void Idle()
    {
        if(stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
        }
        else
        {
            print("待機終了");
            SetMove();
        }
    }

    // todo.敵の動き
    /// <summary>
    /// 敵の移動処理  
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
    /// 移動開始時の処理と目標座標の決定
    /// </summary>
    void SetMove()
    {
        print("移動開始");
        state = EnemyState.MOVE;
        
        Vector2 pos = transform.position;
        pos = Gl_Func.LimPosInBoard(pos);
        transform.position = pos;
        
        targetPos = Gl_Func.RandEnemySpawnPos();
        targetPos = Gl_Func.LimPosInBoard(targetPos);
        
        randMoveSpeedRatio = Random.Range(MIN_MOVE_SPEED_RATIO, MAX_MOVE_SPEED_RATIO);
    }
}
