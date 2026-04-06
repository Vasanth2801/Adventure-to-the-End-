using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    bool isChasing;
    Animator animator;

    [Header("Enemy Settings")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private int facingDirection = 1;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private bool isAttacking = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(player == null)
        {
            return;
        }

        float distance = Vector2.Distance(player.position,transform.position);

        if(distance <=  attackRange)
        {
            isChasing = false;
            animator.SetBool("isChasing", false);
            rb.linearVelocity = Vector2.zero;
            
            if(!isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }

            return;
        }

        if(isChasing == true)
        {
            Chase();
        }
    }

    void Chase()
    {
        animator.SetBool("isChasing", true);
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while(true)
        {
            if(player == null)
            {
                break;
            }

            float distance = Vector2.Distance(player.position, transform.position);
            if(distance > attackRange)
            {
                break;
            }

            Attack();

            yield return new WaitForSeconds(attackCooldown);
        }
        isAttacking = false;
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach(Collider2D hit in hitPlayer)
        {
            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if(ph != null)
            {
                ph.TakeDamage(20);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            isChasing = false;
            animator.SetBool("isChasing", false);
            rb.linearVelocity = Vector2.zero;
        }
    }
}
