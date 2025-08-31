using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private PlayerControls playControls;
    private Vector2 movement;
    private Animator myAnimator;
    private Vector2 LastMoveDir;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Awake()
    {
        playControls = new PlayerControls();
    }

    void Update()
    {
        PlayerInput();
    }

    void OnEnable()
    {
        playControls.Enable();
        playControls.Movement.Enable();
    }

    void OnDisable()
    {
        playControls.Disable();
        playControls.Movement.Disable();
    }

    private void PlayerInput()
    {
        movement = playControls.Movement.Move.ReadValue<Vector2>();

        bool isMoving = movement.sqrMagnitude > 0.01f;

        myAnimator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            // Update arah semasa (untuk Blend Tree)
            myAnimator.SetFloat("MoveX", movement.x);
            myAnimator.SetFloat("MoveY", movement.y);

            // Simpan arah terakhir
            myAnimator.SetFloat("LastMoveX", movement.x > 0 ? 1 : (movement.x < 0 ? -1 : 0));
            myAnimator.SetFloat("LastMoveY", movement.y > 0 ? 1 : (movement.y < 0 ? -1 : 0));
        }
        else
        {
            // Bila idle, tetapkan 0 pada MoveX & MoveY (supaya Blend Tree idle aktif)
            myAnimator.SetFloat("MoveX", 0);
            myAnimator.SetFloat("MoveY", 0);
        }
    }


    void FixedUpdate()
    {
        if (movement.sqrMagnitude > 0.01f)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

    }

    private Vector2 SnapToFourDirections(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            return new Vector2(Mathf.Sign(input.x), 0);
        }
        else
        {
            return new Vector2(0, Mathf.Sign(input.y));
        }
    }

}
