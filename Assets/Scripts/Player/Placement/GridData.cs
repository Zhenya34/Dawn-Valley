using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Placement
{
    public class GridData
    {
        private readonly Dictionary<Vector2Int, PlacementData> _placedObjects = new();

        public void AddObjectAt(Vector2Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
        {
            var positionToOccupy = CalculatePositions(gridPosition, objectSize);
            var data = new PlacementData(positionToOccupy, id, placedObjectIndex);
            foreach (var pos in positionToOccupy.Where(pos => !_placedObjects.TryAdd(pos, data)))
            {
                throw new Exception($"List already contain this tile: {pos}");
            }
        }

        private static List<Vector2Int> CalculatePositions(Vector2Int gridPosition, Vector2Int objectSize)
        {
            List<Vector2Int> returnVal = new();
            for (var x = 0; x < objectSize.x; x++)
            {
                for (var y = 0; y < objectSize.y; y++)
                {
                    returnVal.Add(new Vector2Int(gridPosition.x + x, gridPosition.y + y));
                }
            }
            return returnVal;
        }

        public bool CanPlaceObjectAt(Vector2Int gridPosition, Vector2Int objectSize)
        {
            var positionToOccupy = CalculatePositions(gridPosition, objectSize);
            return positionToOccupy.All(pos => !_placedObjects.ContainsKey(pos));
        }

        internal int GetRepresentationIndex(Vector2Int gridPosition)
        {
            if (_placedObjects.TryGetValue(gridPosition, out var o) == false)
                return -1;
            return o.PlacedObjectIndex;
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
        public int PlacedObjectIndex { get; }

        public PlacementData(List<Vector2Int> occupiedPositions, int iD, int placedObjectIndex)
        {
            OccupiedPositions = occupiedPositions;
            ID = iD;
            PlacedObjectIndex = placedObjectIndex;
        }
    }
}