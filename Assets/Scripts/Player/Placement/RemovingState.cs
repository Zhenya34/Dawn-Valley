using UnityEngine;

namespace Player.Placement
{
    public class RemovingState : IBuildingState
    {
        private int _gameObjectIndex = -1;
        readonly Grid _grid;
        readonly PreviewSystem _previewSystem;
        readonly GridData _floorData;
        readonly GridData _furnitureData;
        readonly ObjectPlacer _objectPlacer;
        readonly SoundFeedback _soundFeedback;

        public RemovingState(Grid grid,
            PreviewSystem previewSystem,
            GridData floorData,
            GridData furnitureData,
            ObjectPlacer objectPlacer,
            SoundFeedback soundFeedback)
        {
            _grid = grid;
            _previewSystem = previewSystem;
            _floorData = floorData;
            _furnitureData = furnitureData;
            _objectPlacer = objectPlacer;
            _soundFeedback = soundFeedback;
            previewSystem.StartShowingRemovePreview();
        }

        public void EndState() => _previewSystem.StopShowingPreview();

        public void OnAction(Vector2Int gridPosition)
        {
            GridData selectedData = null;
            var gridPosition3D = new Vector3Int(gridPosition.x, gridPosition.y, 0);

            if (_furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
                selectedData = _furnitureData;
            else if (_floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false) 
                selectedData = _floorData;

            if (selectedData == null)
            {
                _soundFeedback.PlaySound(SoundType.WrongPlacement);
            }
            else
            {
                _soundFeedback.PlaySound(SoundType.Remove);
                _gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
                if (_gameObjectIndex == -1)
                    return;
                selectedData.RemoveObjectAt(gridPosition);
                _objectPlacer.RemoveObjectAt(_gameObjectIndex);
            }

            var cellPosition = _grid.CellToWorld(gridPosition3D);
            _previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
        }

        private bool CheckIfSelectionIsValid(Vector2Int gridPosition)
        {
            return !(_furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
                     _floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }

        public void UpdateState(Vector2Int gridPosition)
        {
            var validity = CheckIfSelectionIsValid(gridPosition);
            var gridPosition3D = new Vector3Int(gridPosition.x, gridPosition.y, 0);
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition3D), validity);
        }
    }
}