using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    private Rigidbody _rigidbody;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        Move();
        
    }

    void Flip()
    {
        bool  playerHasXAxisSpeed = Mathf.Abs( _rigidbody.velocity.x ) > Mathf.Epsilon; 
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
        float moveDir = Input.GetAxis("Horizontal");
        Vector3 playerVel = new Vector3( moveDir * moveSpeed , _rigidbody.velocity.y ,0 );
        _rigidbody.velocity = playerVel;

        bool  playerHasXAxisSpeed = Mathf.Abs( playerVel.x ) > Mathf.Epsilon; // Epsilon为大于0的无限小的数
        _animator.SetBool("isRun", playerHasXAxisSpeed);
    }

}
