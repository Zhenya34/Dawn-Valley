using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<ItemHandler>(out var itemHandler))
            {
                itemHandler.CollectItem(GetComponent<SpriteRenderer>().sprite);
            }
            Destroy(gameObject);
        }
    }
}
