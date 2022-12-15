 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

 public class Player : MonoBehaviour
 {
     #region//インスペクターで設定する
     [Header("移動速度")] public float speed;
     [Header("重力")] public float gravity;
     [Header("ジャンプ速度")] public float jumpSpeed;
     [Header("ジャンプする高さ")] public float jumpHeight;
     [Header("ジャンプする長さ")] public float jumpLimitTime;
     [Header("接地判定")] public GroundCheck ground;
     [Header("天井判定")] public GroundCheck head;
     [Header("ダッシュの速さ表現")] public AnimationCurve dashCurve;
     [Header("ジャンプの速さ表現")] public AnimationCurve jumpCurve;
     [Header("踏みつけ判定の高さの割合(%)")] public float stepOnRate;
     #endregion

     #region//プライベート変数 
     private Animator anim = null; 
     private Rigidbody2D rb = null; 
     private CapsuleCollider2D capcol = null;
     private SpriteRenderer sr = null;
     private bool isGround = false; 
     private bool isJump = false; 
     private bool isRun = false; 
     private bool isHead = false; 
     private bool isDown = false; 
     private bool isOtherJump = false;
     private bool canHeight = false;
     private bool canTime = false;
     private bool isContinue = false;
     private float jumpPos = 0.0f; 
     private float otherJumpHeight = 0.0f;
     private float dashTime = 0.0f;
     private float jumpTime = 0.0f;
     private float beforeKey = 0.0f;
     private float continueTime = 0.0f;
     private float blinkTime = 0.0f;
     private string enemyTag = "Enemy";
     private float xSpeed = 0.0f;
     private float ySpeed = 0.0f;
     private int right_push_flag = 0;
     private int left_push_flag = 0;
     #endregion 

     void Start() 
     {
          //コンポーネントのインスタンスを捕まえる
          anim = GetComponent<Animator>();
          rb = GetComponent<Rigidbody2D>();
          capcol = GetComponent<CapsuleCollider2D>();
          sr = GetComponent<SpriteRenderer>();
          ySpeed = -gravity;
     } 
    private void Update() 
     {
          if (isContinue)
          {
              //明滅　ついている時に戻る
              if (blinkTime > 0.2f)
              {
                  sr.enabled = true;
                  blinkTime = 0.0f;
              }
              //明滅　消えているとき
              else if (blinkTime > 0.1f)
              {
                  sr.enabled = false;
              }
              //明滅　ついているとき
              else
              {
                  sr.enabled = true;
              }
              //1秒たったら明滅終わり
              if (continueTime > 1.0f)
              {
                  isContinue = false;
                  blinkTime = 0.0f;
                  continueTime = 0.0f;
                  sr.enabled = true;
              }
              else
              {
                  blinkTime += Time.deltaTime;
                  continueTime += Time.deltaTime;
              }
          }
     } 

     void FixedUpdate()
     {
        
         if (!isDown)
         {
            // ySpeed = -gravity;
              //接地判定を得る
              isGround = ground.IsGround();
              isHead = head.IsGround(); 
         
              //各種座標軸の速度を求める
            //   float xSpeed = GetXSpeed();
            //   float ySpeed = GetYSpeed();

              //アニメーションを適用
              SetAnimation();

              //移動速度を設定
              rb.velocity = new Vector2(xSpeed, ySpeed);
         }
         else
         {
            //   rb.velocity = new Vector2(0, -gravity);
         }
     }

     /// <summary>
     /// Y成分で必要な計算をし、速度を返す。
     /// </summary> 
     /// <returns>Y軸の速さ</returns> 
     private float GetYSpeed() 
     {
        float ySpeed = -gravity;
        return ySpeed;
     }

     /// <summary> 
     /// X成分で必要な計算をし、速度を返す。 
     /// </summary> 
     /// <returns>X軸の速さ</returns> 
    public void right_move()
    {
        transform.localScale = new Vector3(1, 1, 1);
        isRun = true;
        dashTime += Time.deltaTime;
        xSpeed = speed;
        ySpeed = -gravity;
        right_push_flag = 1;
        SetAnimation();
        
        Debug.Log("xSpeed");
        Debug.Log(xSpeed);
        Debug.Log("ySpeed");
        Debug.Log(ySpeed);

        rb.velocity = new Vector2(xSpeed, ySpeed);
 
    
    }

    public void left_move()
    {
        transform.localScale = new Vector3(-1, 1, 1);
        isRun = true;
        dashTime += Time.deltaTime;
        xSpeed = -speed;
        ySpeed = -gravity;
        left_push_flag = 1;
        SetAnimation();
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }

    public void jump()
    {
        isJump = true;
        Debug.Log("jump");
        if (isOtherJump)
        {
            //現在の高さが飛べる高さより下か
            // bool canHeight = jumpPos + otherJumpHeight > transform.position.y;
            canHeight = jumpPos + otherJumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎてないか
            // bool canTime = jumpLimitTime > jumpTime;
            canTime = jumpLimitTime > jumpTime;
            if (canHeight && canTime && !isHead)
            {
                Debug.Log("canHeight && canTime && !isHead");
              ySpeed = jumpSpeed;
              jumpTime += Time.deltaTime;
            }
            else
            {
                Debug.Log("canHeight && canTime && !isHead else");
              isOtherJump = false;
              jumpTime = 0.0f;
            }
        }
        else if (isGround)
        {
            Debug.Log("isGrand_if");
            ySpeed = jumpSpeed;
            jumpPos = transform.position.y; //ジャンプした位置を記録する
            isJump = true;
            jumpTime = 0.0f;
        }
        /*
        else
        {
            Debug.Log("isGrand_else");
            isJump = false;
        }
        */
        // bool canHeight = jumpPos + jumpHeight > transform.position.y;
        canHeight = jumpPos + jumpHeight > transform.position.y;
        //ジャンプ時間が長くなりすぎてないか
        // bool canTime = jumpLimitTime > jumpTime;
        canTime = jumpLimitTime > jumpTime;
        if (canHeight && canTime && !isHead)
        {
            Debug.Log("(canHeight && canTime && !isHead)");
            ySpeed = jumpSpeed;
            jumpTime += Time.deltaTime;
        }
        else
        {
            isJump = false;
            jumpTime = 0.0f;
        }
        
        if (isJump)
        {
            Debug.Log("isJump");
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }
        if (isJump || isOtherJump)
        {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }
        Debug.Log("xSpeed");
        Debug.Log(xSpeed);
        Debug.Log("ySpeed");
        Debug.Log(ySpeed);
        SetAnimation();
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }
    public void stand()
    {
        isRun = false;
        xSpeed = 0.0f;
        ySpeed = -gravity;
        dashTime = 0.0f;
        right_push_flag = 0;
        left_push_flag = 0;
        isJump = false;
        jumpTime = 0.0f;
        rb.velocity = new Vector2(0, -gravity);
        SetAnimation();
    }
     private float GetXSpeed() 
     {
        //   float horizontalKey = Input.GetAxis("Horizontal");
        //   float xSpeed = 0.0f;
          /*
          if (horizontalKey > 0)
          {
            // right_move()
            
              transform.localScale = new Vector3(1, 1, 1);
              isRun = true;
              dashTime += Time.deltaTime;
              xSpeed = speed;
            
          }
          else if (horizontalKey < 0)
          {
            left_move()
              transform.localScale = new Vector3(-1, 1, 1);
              isRun = true;
              dashTime += Time.deltaTime;
              xSpeed = -speed;
          }
          else
          {
            stand()
              isRun = false;
              xSpeed = 0.0f;
              dashTime = 0.0f;
          }
          */
          

        //前回の入力からダッシュの反転を判断して速度を変える
        /*
        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }
        
        beforeKey = horizontalKey;
        xSpeed *= dashCurve.Evaluate(dashTime);
        beforeKey = horizontalKey;
        */
        return xSpeed; 
     } 

     /// <summary> 
     /// アニメーションを設定する 
     /// </summary> 
     private void SetAnimation() 
     {
          anim.SetBool("jump", isJump);
          anim.SetBool("ground", isGround);
          anim.SetBool("run", isRun); 
     }

     #region//接触判定      
     private void OnCollisionEnter2D(Collision2D collision) 
     {
        if (collision.collider.tag == enemyTag)
        {
            capcol = GetComponent<CapsuleCollider2D>();
            //踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));
              //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

              foreach (ContactPoint2D p in collision.contacts)
              {
                  if (p.point.y < judgePos)
                  {
                      ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                      if (o != null)
                      {
                          otherJumpHeight = o.boundHeight;    //踏んづけたものから跳ねる高さを取得する
                          o.playerStepOn = true;        //踏んづけたものに対して踏んづけた事を通知する
                          jumpPos = transform.position.y; //ジャンプした位置を記録する 
                          isOtherJump = true;
                          isJump = false;
                          jumpTime = 0.0f;
                      }
                      else
                      {
                          Debug.Log("ObjectCollisionが付いてないよ!");
                      }
                  }
                  else
                  {
                      anim.Play("player_down");
                      isDown = true;
                      break;
                  }
              }
        }
     }
     #endregion
     /// <summary>
/// コンティニュー待機状態か
/// </summary>
/// <returns></returns>
    public bool IsContinueWaiting()
    {
　　    return IsDownAnimEnd();
    }

    //ダウンアニメーションが完了しているかどうか
    private bool IsDownAnimEnd()
    {
　　    if(isDown && anim != null)
　　    {
　　　　    AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
　　　　    if (currentState.IsName("player_down"))
　　　　    {
　　　　　　    if(currentState.normalizedTime >= 1)
　　　　　　    {
　　　　　　　　    return true;
　　　　　　    }
　　　　    }
　　    }
　　    return false;
    }
 }