using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Placement
{
    public class GridData
    {
        private readonly Dictionary<Vector3Int, PlacementData> _placedObjects = new();

        public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
        {
            List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
            PlacementData data = new(positionToOccupy, ID, placedObjectIndex);
            foreach (var pos in positionToOccupy)
            {
                if (!_placedObjects.TryAdd(pos, data))
                {
                    throw new Exception($"Dictionary already contains this cell positiojn {pos}");
                }
            }
        }

        private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
        {
            List<Vector3Int> returnVal = new();
            for(int x = 0; x < objectSize.x; x++)
            {
                for(int y = 0; y < objectSize.y; y++)
                {
                    returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
                }
            }
            return returnVal;
        }

        public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
        {
            List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
            foreach (var pos in positionToOccupy)
            {
                if (_placedObjects.ContainsKey(pos))
                    return false;
            }
            return true;
        }

        internal int GetRepresentationIndex(Vector3Int gridPosition)
        {
            if(_placedObjects.ContainsKey(gridPosition) == false)
            {
                return -1;
            }
            return _placedObjects[gridPosition].PlacedObjectIndex;
        }

        internal void RemoveObjectAt(Vector3Int gridPosition)
        {
            foreach(var pos in _placedObjects[gridPosition].OccupiedPositions)
            {
                _placedObjects.Remove(pos);
            }
        }
    }

    public class PlacementData
    {
        public readonly List<Vector3Int> OccupiedPositions;
        public int ID { get; private set; }
        public int PlacedObjectIndex { get; private set; }

        public PlacementData(List<Vector3Int> occupiedPosition, int iD, int placedObjectIndex)
        {
            OccupiedPositions = occupiedPosition;
            ID = iD;
            PlacedObjectIndex = placedObjectIndex;
        }
    }
}