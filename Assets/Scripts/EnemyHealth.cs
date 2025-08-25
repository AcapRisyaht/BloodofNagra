using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red;
    private Color originalColor;
    public int maxHealth = 100;
    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
     public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashColor());

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    IEnumerator FlashColor()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
    void Die()
    {
        Destroy(gameObject);
    }
   
}