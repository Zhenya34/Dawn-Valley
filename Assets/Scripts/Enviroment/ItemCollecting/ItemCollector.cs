using UnityEngine;

namespace Enviroment.ItemCollecting
{
    public class ItemCollector : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(nameof(Player))) return;
            if (collision.TryGetComponent<ItemHandler>(out var itemHandler))
            {
                itemHandler.CollectItem(GetComponent<SpriteRenderer>().sprite);
            }
            Destroy(gameObject);
        }
    }
}
