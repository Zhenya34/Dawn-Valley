using System.Collections.Generic;
using Enviroment.Time;
using UI.SampleScene.Inventory;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player.Placement
{
    public class ObjectPlacer : MonoBehaviour
    {
        private readonly List<GameObject> _placedGameObjects = new();
        [SerializeField] private DayNightCycle dayNightCycle;
        [SerializeField] private ItemUsageManager itemUsageManager;

        private InventorySlot _currentSlot;
        
        public int PlaceObject(GameObject prefab, Vector3 position)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.transform.position = position;
            _placedGameObjects.Add(newObject);
            
            itemUsageManager.UpdateCountOfItem(_currentSlot);
            
            if (newObject.TryGetComponent<Light2D>(out var lightComponent))
            {
                try
                {
                    dayNightCycle.AddLamp(lightComponent);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
            
            return _placedGameObjects.Count - 1;
        }
        
        internal void RemoveObjectAt(int gameObjectIndex)
        {
            if (_placedGameObjects[gameObjectIndex].TryGetComponent<Light2D>(out var lightComponent))
            {
                try
                {
                    dayNightCycle.RemoveLamp(lightComponent);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
            
            if(_placedGameObjects.Count <= gameObjectIndex || !_placedGameObjects[gameObjectIndex])
            {
                return;
            }

            Destroy(_placedGameObjects[gameObjectIndex]);
            _placedGameObjects[gameObjectIndex] = null;
        }

        public void SetStructure(InventorySlot slot) => _currentSlot = slot;
    }
}