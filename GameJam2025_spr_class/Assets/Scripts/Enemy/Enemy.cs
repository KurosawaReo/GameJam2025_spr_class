using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        IDLE,
        MOVE,
        
    }

    EnemyGenerator eg;

    [Tooltip("状態変数"), SerializeField]
    EnemyState state = EnemyState.IDLE;

    [Tooltip("目標地点") ,SerializeField]
    Vector3 targetPos;
    [Tooltip("移動速度")]
    float moveSpeed = 3f;
    [Tooltip("移動速度乱数")]
    float randMoveSpeedRatio;
    [Tooltip("移動速度乱数の最大値")]
    const float MAX_MOVE_SPEED_RATIO = 1.5f;
    [Tooltip("移動速度乱数の最小値")]
    const float MIN_MOVE_SPEED_RATIO = 0.9f;
    [Tooltip("移動停止の閾値")]
    const float MOVE_STOP_LIM = 0.02f;

    [Tooltip("待機時間計測")]
    float stateTimer;


    // Start is called before the first frame update
    void Start()
    {
        eg = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 状態遷移処理
        State();
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
        var move = targetPos - transform.position;
        transform.position += move.normalized * moveSpeed * randMoveSpeedRatio * Time.deltaTime;
    
        if((targetPos - transform .position).magnitude < MOVE_STOP_LIM)
        {
            print("移動終了");
            NextMove();
        }
    }

    /// <summary>
    /// 次の動き方をランダムで決める
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
    /// 移動開始時の処理と目標座標の決定
    /// </summary>
    void SetMove()
    {
        print("移動開始");
        state = EnemyState.MOVE;
        
        //ワールド座標の取得
        var (lb, rt) = Common.GetWorldWindowSize();

        var randX = Random.Range(lb.x + Common.MARGIN_LEFT + 2, rt.x - Common.MARGIN_RIGHT - 2);
        var randY = Random.Range(lb.y + Common.MARGIN_BOTTOM + 2, rt.y - Common.MARGIN_TOP - 2);

        targetPos = new Vector3(randX, randY, transform.position.z);

        randMoveSpeedRatio = Random.Range(MIN_MOVE_SPEED_RATIO, MAX_MOVE_SPEED_RATIO);


    }

    // todo.プレイヤーの当たり判定

   

}
