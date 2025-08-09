using UnityEngine;

public class TreeSorting : MonoBehaviour
{
    public SpriteRenderer treeRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpriteRenderer playerRenderer = collision.GetComponent<SpriteRenderer>();
            if (playerRenderer != null && treeRenderer != null)
            {
                // Letakkan pokok di depan pemain
                treeRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (treeRenderer != null)
            {
                // Kembalikan pokok ke lapisan asal
                treeRenderer.sortingOrder = 0;
            }
        }
    }
}
