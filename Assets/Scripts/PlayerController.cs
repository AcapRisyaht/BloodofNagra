using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    private Vector2 movement;

    [Header("Combat")]
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public int attackDamage = 1;
    public float parryDuration = 0.3f;

    private bool isParrying = false;

    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Movement.Attack.performed += ctx => Attack();
        playerControls.Movement.Parry.performed += ctx => StartCoroutine(Parry());
    }

    private void OnDisable()
    {
        playerControls.Movement.Attack.performed -= ctx => Attack();
        playerControls.Movement.Parry.performed -= ctx => StartCoroutine(Parry());
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
        AdjustPlayerFacingDirection();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        bool isMoving = movement.sqrMagnitude > 0.01f;

        myAnimator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            myAnimator.SetFloat("MoveX", movement.x);
            myAnimator.SetFloat("MoveY", movement.y);
            myAnimator.SetFloat("LastMoveX", movement.x > 0 ? 1 : (movement.x < 0 ? -1 : 0));
            myAnimator.SetFloat("LastMoveY", movement.y > 0 ? 1 : (movement.y < 0 ? -1 : 0));
        }
        else
        {
            myAnimator.SetFloat("MoveX", 0);
            myAnimator.SetFloat("MoveY", 0);
        }
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        // (optional, boleh ditambah logic hadap sprite ke kiri/kanan)
    }

    private void Attack()
    {
        Debug.Log("Pemain menyerang!");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Kena musuh: " + enemy.name);
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    private IEnumerator Parry()
    {
        isParrying = true;
        Debug.Log("Parry aktif!");
        mySpriteRenderer.color = Color.cyan; // kesan visual

        yield return new WaitForSeconds(parryDuration);

        mySpriteRenderer.color = Color.white;
        isParrying = false;
        Debug.Log("Parry tamat.");
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}