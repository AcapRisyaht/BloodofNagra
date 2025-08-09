using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject Player;
    public float speed = 2f;
    public float chaseRange = 5f;
    public float stopDistance = 1f;

    private Transform target;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Cari pemain berdasarkan tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player tidak ditemui. Pastikan ada GameObject bertag 'Player'");
        }
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= chaseRange & distance > stopDistance)
        {
            // Gerak ke arah pemain
            Vector2 direction = (target.position - transform.position).normalized;
            movement = direction;
        }
        else
        {
            // Berhenti jika terlalu dekat atau jauh j
            movement = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        // Hanya bergerak jika ada movement
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }
}
