using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Placement
{
    public class GridData
    {
        private readonly Dictionary<Vector2Int, PlacementData> _placedObjects = new();

        public void AddObjectAt(Vector2Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
        {
            List<Vector2Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
            PlacementData data = new PlacementData(positionToOccupy, id, placedObjectIndex);
            foreach (var pos in positionToOccupy)
            {
                if (_placedObjects.ContainsKey(pos))
                    throw new Exception($"List already contain this tile: {pos}");
                _placedObjects[pos] = data;
            }
        }

        private List<Vector2Int> CalculatePositions(Vector2Int gridPosition, Vector2Int objectSize)
        {
            List<Vector2Int> returnVal = new();
            for (int x = 0; x < objectSize.x; x++)
            {
                for (int y = 0; y < objectSize.y; y++)
                {
                    returnVal.Add(new Vector2Int(gridPosition.x + x, gridPosition.y + y));
                }
            }
            return returnVal;
        }

        public bool CanPlaceObjectAt(Vector2Int gridPosition, Vector2Int objectSize)
        {
            List<Vector2Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
            foreach (var pos in positionToOccupy)
            {
                if (_placedObjects.ContainsKey(pos))
                    return false;
            }
            return true;
        }

        internal int GetRepresentationIndex(Vector2Int gridPosition)
        {
            if (_placedObjects.ContainsKey(gridPosition) == false)
                return -1;
            return _placedObjects[gridPosition].PlacedObjectIndex;
        }

        internal void RemoveObjectAt(Vector2Int gridPosition)
        {
            foreach (var pos in _placedObjects[gridPosition].OccupiedPositions)
            {
                _placedObjects.Remove(pos);
            }
        }
    }

    public class PlacementData
    {
        public readonly List<Vector2Int> OccupiedPositions;
        public int ID { get; private set; }
        public int PlacedObjectIndex { get; private set; }

        public PlacementData(List<Vector2Int> occupiedPositions, int iD, int placedObjectIndex)
        {
            OccupiedPositions = occupiedPositions;
            ID = iD;
            PlacedObjectIndex = placedObjectIndex;
        }
    }
}