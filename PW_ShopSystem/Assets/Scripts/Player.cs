using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] float _jumpVelocity = 5.0f;
    [SerializeField] int _remainingJumps = 2;
    [SerializeField] bool _isGrounded;
    [SerializeField] Transform _feet;
    [SerializeField] float _downPull;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    float horizontalInput;
    bool jump;
    bool isWalking;
    bool flipSprite;
    float _fallTimer = 0;
    Vector2 _startingPosition;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _isGrounded = true;
        _startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
        HandleFalling();      
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput, rb.velocity.y);
        if (jump && _remainingJumps > 0)
        {
            _remainingJumps--;
            rb.velocity = new Vector2(rb.velocity.x, _jumpVelocity);
            _fallTimer = 0;
            jump = false;
        }
    }

    private void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal") * _speed;
        isWalking = horizontalInput != 0;

        animator.SetBool("isWalking", isWalking);
        flipSprite = horizontalInput < 0;
        spriteRenderer.flipX = flipSprite;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _remainingJumps > 0)
        {
            jump = true;
        }
    }

    void HandleFalling()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));

        if (hit != null)
        {
            _remainingJumps = 2;
            _isGrounded = true;
            _fallTimer = 0;
        }
        else
        {
            _isGrounded = false;
        }

        if (!_isGrounded)
        {
            _fallTimer += Time.deltaTime;
            var downForce = _downPull * _fallTimer * _fallTimer;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - downForce);
        }
    }
}
