using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum State
    {
        Normal,
        Rolling,
    }

    [Tooltip("移动速度")]
    public float moveSpeed;
    [Tooltip("翻滚速度,即会一瞬间冲出的初始速度")]
    public float initRollSpeed;
    [Tooltip("翻滚速度会按照一定倍数随时间衰减，直至正常速度")]
    public float rollSpeedDropMultiplier;
    [Tooltip("短闪距离")]
    public float teleportPower;
    [Tooltip("传入短闪特效的Prefab")]
    public GameObject teleportEffect;
    public bool lockMovement;
    public LayerMask dashLayerMask;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private Vector3 faceDir;
    private Vector3 moveDir;
    private Vector3 rollDir;
    private float rollSpeed;
    private bool isDashButtonDown;
    private RaycastHit raycastHit;
    private State state;
    

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _animator = GetComponentInChildren<Animator>();
        state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        KeyController();
        Flip();
    }

    void Flip()
    {
        bool playerHasXAxisSpeed = Mathf.Abs(_rigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if (_rigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (_rigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);  //人物翻转180度
            }
        }
    }

    void KeyController()
    {
        switch( state )
        {
            case State.Normal:
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                moveDir = new Vector3(horizontal, 0, vertical);

                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    isDashButtonDown = true;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if ( moveDir != Vector3.zero )
                    {
                        rollDir = moveDir;
                    }
                    else
                    {
                        rollDir = faceDir;
                    }
                    
                    rollSpeed = initRollSpeed;
                    state = State.Rolling;
                    //等翻滚动画做好了，在这里播放翻滚动画
                }
                break;
            case State.Rolling:
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                float rollSpeedMinimum = 2f;
                if ( rollSpeed < rollSpeedMinimum )
                {
                    state = State.Normal;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        switch( state)
        {
            case State.Normal:
                Move();
                Teleport();
                break;
            case State.Rolling:
                Roll();
                break;
        }
    }

    void Roll()
    {
        //_rigidbody.velocity = new Vector3(moveDir.x * moveSpeed, _rigidbody.velocity.y, moveDir.z * moveSpeed);
        _rigidbody.velocity = rollDir * rollSpeed;
    }

    void Teleport()
    {
        if( isDashButtonDown )
        {
            Vector3 dashPosition = transform.position + moveDir * teleportPower;
            if ( moveDir != Vector3.zero )
            {
                faceDir = moveDir;
            }
            
            if (Physics.Raycast(transform.position, moveDir, out raycastHit, teleportPower, dashLayerMask))
            {
                dashPosition = raycastHit.point;
            }

            // Spawn visual effect
            DashEffect.CreateDashEffect(transform.position, moveDir, Vector3.Distance(transform.position, dashPosition), teleportEffect);

            _rigidbody.MovePosition(dashPosition);
            isDashButtonDown= false;
        }
    }

    void Move()
    {
        if (lockMovement)
            return;

        if (moveDir == Vector3.zero)
        {
            _animator.SetBool("isRun", false);
            return;
        }

        _rigidbody.velocity = new Vector3(moveDir.x * moveSpeed, _rigidbody.velocity.y, moveDir.z * moveSpeed);
        if (moveDir != Vector3.zero)
        {
            faceDir = moveDir;
        }

        _animator.SetBool("isRun", true);
    }
}
