using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   public float moveSpeed = 5f;
   private Rigidbody2D rb;
   private Vector2 movement;
   private Animator animator;
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetBool("IsMoving", movement.x != 0 || movement.y != 0);  
    }
    
    void FixedUpdate()
    {
       rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
