using UnityEngine;
using UnityEngine.Tilemaps;

public class HoeLogic : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _grassTile;
    [SerializeField] private TileBase _dirtTile;
    [SerializeField] private Player_Animation _playerAnim;
    [SerializeField] private GardenBedsUpdater _gardenBedsUpdater;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && _playerAnim.toolType == 3)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = _tilemap.WorldToCell(mouseWorldPos);

            TileBase clickedTile = _tilemap.GetTile(gridPosition);

            if (clickedTile == _grassTile)
            {
                _tilemap.SetTile(gridPosition, _dirtTile);
                _gardenBedsUpdater.UpdateTile(gridPosition);
            }
        }
    }
}