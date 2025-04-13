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

    [Tooltip("敵の状態"), SerializeField]
    EnemyState state = EnemyState.IDLE;
    [Tooltip("移動の目標地点") ,SerializeField]
    Vector3 targetPos;
    [Tooltip("移動速度")]
    float moveSpeed = 3f;
    [Tooltip("移動速度乱数")]
    float randMoveSpeedRatio;

    [Tooltip("待機時間計測")] 
    float stateTimer;

    [Tooltip("消滅アニメーション"), SerializeField]
    GameObject prfbBreakAnim;

    void Start()
    {
        bm = GameObject.Find("BoardManager").GetComponent<BoardManager>();
        pm = GameObject.Find("Player").      GetComponent<PlayerManager>();
        gm = GameObject.Find("GameManager"). GetComponent<GameManager>();

        //eg = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
    }

    void Update()
    {
        //ゲーム中のみ.
        if (gm.isStart && !gm.isGameEnd)
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
                EnemyIdle();
                break;
            case EnemyState.MOVE:
                //移動
                EnemyMove();
                break;
        }
    }

    /// <summary>
    /// 待機状態処理
    /// </summary>
    void EnemyIdle()
    {
        if(stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
        }
        else
        {
            print("待機終了");
            SetGoalMove();
        }
    }

    // todo:敵の動き
    /// <summary>
    /// 敵の移動処理  
    /// </summary>
    void EnemyMove()
    {
        var tmpPos = transform.position; //現在座標の仮取得.
        var dis = targetPos - tmpPos;    //目標地点までの座標差(distance)

        //仮で移動したとする.
        tmpPos += dis.normalized * moveSpeed * randMoveSpeedRatio * Time.deltaTime;
        
        //何のマスに乗ってるかで処理を分ける.
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
                transform.position = tmpPos; //実際に移動する.
                break;
        }
        
        //目標地点に着いたら(一定範囲)
        if ((targetPos - transform.position).magnitude < Gl_Const.ENM_GOAL_STOP_RANGE)
        {
            SetGoalMove();
        }
    }
    
    void EnemyDeath()
    {
        gm.deathEnemyCnt++;  //死亡数+1.
        
        var obj = Instantiate(prfbBreakAnim);        //撃破アニメーションを召喚.
        obj.transform.position = transform.position; //敵の位置に移動.
        
        Destroy(gameObject); //自身を消滅.
    }

    /// <summary>
    /// 移動開始時の処理と、目標座標の決定
    /// </summary>
    void SetGoalMove()
    {
        state = EnemyState.MOVE;
        
        {
            //目標地点を抽選.
            Vector2Int rnd = Gl_Func.RndBPosOutside(30);

            //その座標が無マスなら.
            if (bm.Board[rnd.x, rnd.y].type == BoardType.NONE)
            {
                targetPos = Gl_Func.BPosToWPos(new Vector2Int(rnd.x, rnd.y));
            }
        }

        //移動速度の抽選.
        randMoveSpeedRatio = Random.Range(
            Gl_Const.ENM_MIN_MOVE_SPEED, Gl_Const.ENM_MAX_MOVE_SPEED
        );
    }
}
