using UnityEngine;

namespace Player.Placement
{
    public interface IBuildingState
    {
        void EndState();
        void OnAction(Vector3Int gridPosition);
        void UpdateState(Vector3Int gridPosition);
    }
}