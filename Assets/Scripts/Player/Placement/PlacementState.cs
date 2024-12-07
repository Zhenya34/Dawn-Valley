using UnityEngine;

namespace Player.Placement
{
    public class PlacementState : IBuildingState
    {
        private readonly int _selectedObjectIndex = -1;
        private readonly int _id;
        private readonly Grid _grid;
        private readonly PreviewSystem _previewSystem;
        private readonly ObjectsDatabaseSo _database;
        private readonly GridData _floorData;
        private readonly GridData _furnitureData;
        private readonly ObjectPlacer _objectPlacer;
        private readonly SoundFeedback _soundFeedback;

        public PlacementState(
            int iD,
            Grid grid,
            PreviewSystem previewSystem,
            ObjectsDatabaseSo database,
            GridData floorData,
            GridData furnitureData,
            ObjectPlacer objectPlacer,
            SoundFeedback soundFeedback)
        {
            _id = iD;
            _grid = grid;
            _previewSystem = previewSystem;
            _database = database;
            _floorData = floorData;
            _furnitureData = furnitureData;
            _objectPlacer = objectPlacer;
            _soundFeedback = soundFeedback;
            
            _selectedObjectIndex = _database.objectsData.FindIndex(data => data.ID == _id);
            if (_selectedObjectIndex > -1)
            {
                previewSystem.StartShowingPlacementPreview(
                    _database.objectsData[_selectedObjectIndex].Prefab,
                    _database.objectsData[_selectedObjectIndex].Size
                );
            }
            else
            {
                Debug.LogError($"No object with ID {_id} found in the database.");
            }
        }

        public void EndState()
        {
            _previewSystem.StopShowingPreview();
            _previewSystem.StopShowingRemovePreview();
        }

        public void OnAction(Vector2Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);

            if (!placementValidity)
            {
                _soundFeedback.PlaySound(SoundType.WrongPlacement);
                return;
            }

            _soundFeedback.PlaySound(SoundType.Place);
            
            Vector3 worldPosition = _grid.CellToWorld((Vector3Int)gridPosition) + 
                                    new Vector3(_grid.cellSize.x / 2, 0, 0);

            int index = _objectPlacer.PlaceObject(
                _database.objectsData[_selectedObjectIndex].Prefab,
                worldPosition
            );

            GridData selectedData = (_database.objectsData[_selectedObjectIndex].ID == 0)
                ? _floorData
                : _furnitureData;

            selectedData.AddObjectAt(
                gridPosition,
                _database.objectsData[_selectedObjectIndex].Size,
                _database.objectsData[_selectedObjectIndex].ID,
                index
            );

            _previewSystem.UpdatePosition(worldPosition, false);
        }

        private bool CheckPlacementValidity(Vector2Int gridPosition, int selectedObjectIndex)
        {
            GridData selectedData = _database.objectsData[selectedObjectIndex].ID == 0
                ? _floorData
                : _furnitureData;

            return selectedData.CanPlaceObjectAt(
                gridPosition,
                _database.objectsData[selectedObjectIndex].Size
            );
        }

        public void UpdateState(Vector2Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);

            Vector3 worldPosition = _grid.CellToWorld((Vector3Int)gridPosition);

            _previewSystem.UpdatePosition(worldPosition, placementValidity);
        }
    }
}