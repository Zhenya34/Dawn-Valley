using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GardenBedsUpdater : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _dirtTileTopLeftCorner;
    [SerializeField] private TileBase _dirtTileTopRightCorner;
    [SerializeField] private TileBase _dirtTileBottomLeftCorner;
    [SerializeField] private TileBase _dirtTileBottomRightCorner;
    [SerializeField] private TileBase _dirtTileVerticalEdge;
    [SerializeField] private TileBase _dirtTileHorizontalEdge;
    [SerializeField] private TileBase _dirtTileCenter;
    [SerializeField] private TileBase _dirtTileTopEdge;
    [SerializeField] private TileBase _dirtTileBottomEdge;
    [SerializeField] private TileBase _dirtTileLeftEdge;
    [SerializeField] private TileBase _dirtTileRightEdge;
    [SerializeField] private TileBase _dirtTileSingle;
    [SerializeField] private TileBase _dirtTileThreeSidesTop;
    [SerializeField] private TileBase _dirtTileThreeSidesBottom;
    [SerializeField] private TileBase _dirtTileThreeSidesLeft;
    [SerializeField] private TileBase _dirtTileThreeSidesRight;

    private readonly HashSet<Vector3Int> _updatedTiles = new();

    public void UpdateTile(Vector3Int position)
    {
        TileBase currentTile = _tilemap.GetTile(position);
        if (!IsDirtTile(currentTile))
        {
            _updatedTiles.Add(position);
            return;
        }

        bool hasDirtTop = IsDirtTile(_tilemap.GetTile(position + new Vector3Int(0, 1, 0)));
        bool hasDirtRight = IsDirtTile(_tilemap.GetTile(position + new Vector3Int(1, 0, 0)));
        bool hasDirtBottom = IsDirtTile(_tilemap.GetTile(position + new Vector3Int(0, -1, 0)));
        bool hasDirtLeft = IsDirtTile(_tilemap.GetTile(position + new Vector3Int(-1, 0, 0)));

        int dirtCount = (hasDirtTop ? 1 : 0) + (hasDirtRight ? 1 : 0) + (hasDirtBottom ? 1 : 0) + (hasDirtLeft ? 1 : 0);

        if (dirtCount == 4)
        {
            _tilemap.SetTile(position, _dirtTileCenter);
        }
        else if (dirtCount == 3)
        {
            if (!hasDirtTop) _tilemap.SetTile(position, _dirtTileThreeSidesBottom);
            else if (!hasDirtRight) _tilemap.SetTile(position, _dirtTileThreeSidesLeft);
            else if (!hasDirtBottom) _tilemap.SetTile(position, _dirtTileThreeSidesTop);
            else if (!hasDirtLeft) _tilemap.SetTile(position, _dirtTileThreeSidesRight);
        }
        else if (dirtCount == 2)
        {
            if (hasDirtTop && hasDirtBottom)
                _tilemap.SetTile(position, _dirtTileVerticalEdge);
            else if (hasDirtLeft && hasDirtRight)
                _tilemap.SetTile(position, _dirtTileHorizontalEdge);
            else if (hasDirtTop && hasDirtRight)
                _tilemap.SetTile(position, _dirtTileBottomLeftCorner);
            else if (hasDirtTop && hasDirtLeft)
                _tilemap.SetTile(position, _dirtTileBottomRightCorner);
            else if (hasDirtBottom && hasDirtRight)
                _tilemap.SetTile(position, _dirtTileTopLeftCorner);
            else if (hasDirtBottom && hasDirtLeft)
                _tilemap.SetTile(position, _dirtTileTopRightCorner);
        }
        else if (dirtCount == 1)
        {
            if (hasDirtTop)
                _tilemap.SetTile(position, _dirtTileBottomEdge);
            else if (hasDirtRight)
                _tilemap.SetTile(position, _dirtTileLeftEdge);
            else if (hasDirtBottom)
                _tilemap.SetTile(position, _dirtTileTopEdge);
            else if (hasDirtLeft)
                _tilemap.SetTile(position, _dirtTileRightEdge);
        }
        else
        {
            _tilemap.SetTile(position, _dirtTileSingle);
        }

        _updatedTiles.Add(position);
        UpdateNeighbors(position);
    }

    private void UpdateNeighbors(Vector3Int position)
    {
        Vector3Int[] directions = new Vector3Int[]
        {
            new (0, 1, 0),
            new (1, 0, 0),
            new (0, -1, 0),
            new (-1, 0, 0)
        };

        foreach (var direction in directions)
        {
            Vector3Int neighborPosition = position + direction;
            TileBase neighborTile = _tilemap.GetTile(neighborPosition);
            if (IsDirtTile(neighborTile))
            {
                UpdateTile(neighborPosition);
            }
        }
    }

    private bool IsDirtTile(TileBase tile)
    {
        return tile == _dirtTileSingle || tile == _dirtTileCenter || tile == _dirtTileVerticalEdge ||
               tile == _dirtTileHorizontalEdge || tile == _dirtTileBottomLeftCorner ||
               tile == _dirtTileBottomRightCorner || tile == _dirtTileTopLeftCorner ||
               tile == _dirtTileTopRightCorner || tile == _dirtTileTopEdge || tile == _dirtTileBottomEdge ||
               tile == _dirtTileLeftEdge || tile == _dirtTileRightEdge || tile == _dirtTileThreeSidesTop ||
               tile == _dirtTileThreeSidesBottom || tile == _dirtTileThreeSidesLeft || tile == _dirtTileThreeSidesRight;
    }
}