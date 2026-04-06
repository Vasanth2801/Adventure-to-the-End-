using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float facingDirection = 1;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;

    [Header("Normal Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask attackLayer;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Input Settings")]
    [SerializeField] private float moveInput;

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        Jump();

        if(moveInput > 0 && transform.localScale.x < 0 || moveInput < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        HandleAnimations();
    }

    private void FixedUpdate()
    {
        Move();

        Attack();

        HeavyAttack();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    void Attack()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, attackLayer);
            foreach(Collider2D hit in hitEnemies)
            {
                Debug.Log("Attacking");
            }
        }
    }

    void HeavyAttack()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("HeavyAttack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRadius, attackLayer);
            foreach(Collider2D hit in hitEnemies)
            {
                Debug.Log("Heavy Attacking");
            }
        }
    }


    void HandleAnimations()
    {
        animator.SetFloat("Speed",Mathf.Abs(moveInput));
        animator.SetBool("isJumping", rb.linearVelocity.y > 0.1);
    }
}