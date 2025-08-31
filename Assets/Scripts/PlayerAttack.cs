using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;
    private PlayerControls playControls;
    public GameObject attackHitBox; // Untuk aktivkan hitbox
    public bool isMoving = false; // Tambah ini untuk cek apakah player sedang bergerak
    public Vector2 LastMoveDir = Vector2.right; // Arah hadap terakhir, default ke kanan
    void Awake()
    {
        playControls = new PlayerControls();
    }

    void OnEnable()
    {
        playControls.Enable();
        playControls.Combat.Attack.performed += OnAttack;

        playControls.Movement.Move.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            isMoving = input != Vector2.zero;
            if (isMoving)

                LastMoveDir = input;

        };

        playControls.Movement.Move.canceled += ctx => isMoving = false;
    }

    void OnDisable()
    {
        playControls.Combat.Attack.performed -= OnAttack;

        playControls.Movement.Move.performed -= ctx => isMoving = ctx.ReadValue<Vector2>() != Vector2.zero;
        playControls.Movement.Move.canceled -= ctx => isMoving = false;

        playControls.Disable();
    }
    
    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (isMoving)
        {
            Debug.Log("Can't attack while moving");
            return;
        }
        Debug.Log("Player Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Ambil arah hadap pemain
        Vector2 facingDir = transform.right; // Assuming right is the facing direction
        if (facingDir == Vector2.zero)
            facingDir = Vector2.right; // Default facing direction

        foreach (Collider2D enemy in hitEnemies)
        {
            Vector2 enemyDir = ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;
            float dot = Vector2.Dot(LastMoveDir.normalized, enemyDir);


            if (dot < 0.7) // Enemy is behind the player
            {
                SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
                if (enemySprite != null)
                {
                    enemySprite.color = Color.red; // Change color to red on hit
                    StartCoroutine(ResetColor(enemySprite));

                    Debug.Log("Musuh kena serangan!");
                }

                else
                {
                    Debug.Log("Musuh tidak berada di depan!");
                }

            }
        }
    }

    void DisableHitBox()
    {
        attackHitBox.SetActive(false);
    }

    private IEnumerator ResetColor(SpriteRenderer enemySprite)
    {
        yield return new WaitForSeconds(0.2f);
        enemySprite.color = Color.white;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
