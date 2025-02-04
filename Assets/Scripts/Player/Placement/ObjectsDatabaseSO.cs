using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Placement
{
    [CreateAssetMenu(fileName = "NewObjectsDatabase", menuName = "Database/ObjectsDatabase")]
    public class ObjectsDatabaseSo : ScriptableObject
    {
        public List<ObjectData> objectsData;

        public int GetStructureIDByName(string objectName)
        {
            foreach (var objectData in objectsData.Where(objectData => objectData.Name == objectName))
            {
                return objectData.ID;
            }

            return -1;
        }
    }

    [System.Serializable]
    public class ObjectData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}