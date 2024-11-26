using System.Collections.Generic;
using UnityEngine;

namespace Player.Placement
{
    public class ObjectPlacer : MonoBehaviour
    {
        private readonly List<GameObject> _placedGameObjects = new();

        public int PlaceObject(GameObject prefab, Vector3 position)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.transform.position = position;
            _placedGameObjects.Add(newObject);
            return _placedGameObjects.Count - 1;
        }
        
        internal void RemoveObjectAt(int gameObjectIndex)
        {
            if(_placedGameObjects.Count <= gameObjectIndex || _placedGameObjects[gameObjectIndex] == null)
            {
                return;
            }

            Destroy(_placedGameObjects[gameObjectIndex]);
            _placedGameObjects[gameObjectIndex] = null;
        }
    }
}