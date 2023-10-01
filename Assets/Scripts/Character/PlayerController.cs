using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody _rigidbody;
    private Animator _animator;
    public bool lockMovement;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        Move();
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

    void Move()
    {
        if (lockMovement)
            return;

        //float moveDir = Input.GetAxis("Horizontal");
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(horizontal, 0, vertical);
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
