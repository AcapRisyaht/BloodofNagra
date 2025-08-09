using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chaseRadius = 5f;
    public float stopDistance = 1.2f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseRadius && distance > stopDistance)
        {
            // Gerak ke arah pemain
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction;
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Tidak bergerak
            movement = Vector2.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Lukis radius di Scene View untuk rujukan
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
