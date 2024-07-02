using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField] private float offset = 0f;
    private readonly int sortingOrderBase;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        _renderer.sortingOrder = (int)(sortingOrderBase + transform.position.y + offset);
    }
}