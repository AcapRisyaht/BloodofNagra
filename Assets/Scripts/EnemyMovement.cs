using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 2f;
    public Transform Player;
    private Rigidbody2D rb;
    public float minDistance = 1f;
    public float stopRadius = 0.5f; // Add this line to define stopRadius
    public float chaseRadius = 5f; // Added chaseRadius field


    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, Player.position);

        if (distance < chaseRadius && distance > stopRadius)
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopRadius);
    }
}
