using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour

{

    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Animator animator;
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 24f;
    private bool isFacingRight = true;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if (rb.velocity.x != 0 )
        {
            animator.SetBool("IsWalking", true);
            if (rb.velocity.x > 0) isFacingRight = true;
            else isFacingRight = false;
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
        if (!isFacingRight)
        {
            spriteRenderer.flipX = true;
        }
        else if (isFacingRight)
        {
            spriteRenderer.flipX = false;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= 1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
}
