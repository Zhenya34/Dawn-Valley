using UnityEngine;
using UnityEngine.Tilemaps;

public class Planting : MonoBehaviour
{
    [SerializeField] private GameObject[] _sprouts = new GameObject[12];
    [SerializeField] private Player_Animation _playerAnim;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Tilemap _tilemap;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && _playerAnim.toolsAllowed == true)
        {
            Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            Vector3Int cellPosition = _tilemap.WorldToCell(mouseWorldPos);

            Vector3 tileCenter = _tilemap.GetCellCenterWorld(cellPosition);

            Instantiate(_sprouts[0], tileCenter, Quaternion.identity);
        }
    }
}
