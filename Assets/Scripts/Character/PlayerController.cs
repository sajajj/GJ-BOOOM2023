using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("移动速度")]
    public float moveSpeed;
    [Tooltip("短闪距离")]
    public float teleportPower;
    public bool lockMovement;
    public LayerMask dashLayerMask;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Vector3 moveDir;
    private bool isDashButtonDown;
    RaycastHit raycastHit;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _animator = GetComponentInChildren<Animator>();
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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDir = new Vector3(horizontal, 0, vertical);

        if( Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) )
        {
            isDashButtonDown = true;
        }
    }

    void FixedUpdate()
    {
        Move();
        Teleport();
    }

    void Teleport()
    {
        if( isDashButtonDown )
        {
            Vector3 dashPosition = transform.position + moveDir * teleportPower;
            
            if (Physics.Raycast(transform.position, moveDir, out raycastHit, teleportPower, dashLayerMask))
            {
                dashPosition = raycastHit.point;
            }

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

        Vector3 playerVel = new Vector3(moveDir.x * moveSpeed, _rigidbody.velocity.y, moveDir.z * moveSpeed);
        _rigidbody.velocity = playerVel;

        _animator.SetBool("isRun", true);
    }
}
