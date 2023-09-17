using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpPower;     //因为这种方式是施加力的方式，所以要抵消重力，需要数值大一些
    private Rigidbody _rigidbody;
    private CharacterController _characterController;
    private Animator _animator;
    private bool canDoubleJump = false;
    private int maxJumpCount = 1;
    private int jumpCount;
    public bool lockMovement;
    private int rotationDegree = 0; //默认0度是沿着x轴正向移动

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _characterController = GetComponent<CharacterController>(); 
        _animator = GetComponent<Animator>(); 
        jumpCount = maxJumpCount;
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        Move();
        CheckDoubleJump();
        Jump();
        SwitchAnimation();
    }


    void Flip()
    {
        //不适用三角函数计算的情况
        bool  playerHasXAxisSpeed = (Mathf.Abs( _rigidbody.velocity.x ) > Mathf.Epsilon ); 
        if ( playerHasXAxisSpeed )
        {
            if( _rigidbody.velocity.x > 0.1f )
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if( _rigidbody.velocity.x < -0.1f )
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);  //人物翻转180度
            }
        }
    }

    void Move()
    {
        if (lockMovement)
            return;

        float moveDir = Input.GetAxis("Horizontal");
        if (moveDir == 0)
        {
            _animator.SetBool("isRun", false);
            return;
        }

        Vector3 playerVel = new Vector3(moveDir * moveSpeed, _rigidbody.velocity.y, 0);
        _rigidbody.velocity = playerVel;

        //bool  playerHasXAxisSpeed = Mathf.Abs( playerVel.x ) > Mathf.Epsilon; // Epsilon为大于0的无限小的数
        _animator.SetBool("isRun", true);
    }

    void Jump()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (!canDoubleJump)
            {
                if (IsOnGround())
                {
                    _rigidbody.AddForce(Vector3.up * jumpPower);
                    _animator.SetBool("isJump", true);
                }
            }
            else
            {
                if (jumpCount > 0)
                {
                    _rigidbody.AddForce(Vector3.up * jumpPower);
                    _animator.SetBool("isJump", true);

                    jumpCount--;
                }
            }
        }

    }

    void CheckDoubleJump()
    {
        if ( canDoubleJump )
        {
            maxJumpCount = 1;   //多一次跳跃
        }
        
        if ( IsOnGround() )
        {
            jumpCount = maxJumpCount;
        }
    }

    bool IsOnGround()
    {
        return Physics.Raycast( transform.position, -Vector3.up, 0.1f);
    }

    void SwitchAnimation()
    {
        //_animator.SetBool("idle", false);
        if(_animator.GetBool("isJump"))
        {
            if( _rigidbody.velocity.y < 0.0f && IsOnGround() )
            {
                _animator.SetBool("isJump", false);
            }
        }
    }

    /// <summary>
    /// 等待CameraRotation发来的事件
    /// </summary>
    /// <param name="degree">player需要旋转的角度</param>
    void Rotation(int degree)
    {
        lockMovement= true;
        _animator.SetBool("isRun", false);
        //_animator.SetBool("idle", true);
        rotationDegree = degree;
        Invoke("RotationDelay", 0.35f); //给足时间去结束动画
    }

    void RotationDelay()
    {
        this.transform.Rotate(0, rotationDegree, 0);
    }
}
