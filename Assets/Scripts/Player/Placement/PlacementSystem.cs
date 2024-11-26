using UnityEngine;

namespace Player.Placement
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Grid grid;
        [SerializeField] private ObjectsDatabaseSo database;
        [SerializeField] private GameObject gridVisualization;
        [SerializeField] private AudioClip correctPlacementClip;
        [SerializeField] private AudioClip wrongPlacementClip;
        [SerializeField] private AudioSource source;
        [SerializeField] private PreviewSystem preview;
        [SerializeField] private ObjectPlacer objectPlacer;
        [SerializeField] private SoundFeedback soundFeedback;

        private GridData _floorData, _furnitureData;
        private Vector3Int _lastDetectedPosition = Vector3Int.zero;
        private IBuildingState _buildingState;
        private bool _isPlacementInitialized;
        private bool _isRemovingInitialized;

        private void Start()
        {
            inputManager.OnClickedRightButton += PerformAction;
            inputManager.OnClickedLeftButton += PerformAction;
            inputManager.OnExit += StopPlacement;

            gridVisualization.SetActive(false);
            _floorData = new();
            _furnitureData = new();
        }
    
        private void Update()
        {
            if (_buildingState == null)
            {
                return;
            }
            Vector3 mousePosition = inputManager.GetSelectedTileMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            if (_lastDetectedPosition != gridPosition)
            {
                _buildingState.UpdateState(gridPosition);
                _lastDetectedPosition = gridPosition;
            }
        }
        
        public void StartPlacement(int id)
        {
            StopRemoving();
            _isPlacementInitialized = true;
            gridVisualization.SetActive(true);
            _buildingState = new PlacementState(id, grid, preview,
                database, _floorData, _furnitureData,
                objectPlacer, soundFeedback);
            inputManager.OnClickedRightButton += PerformAction;
            inputManager.OnExit += StopPlacement;
        }
        
        public void StartRemoving()
        {
            StopPlacement();
            _isRemovingInitialized = true;
            gridVisualization.SetActive(true);
            _buildingState = new RemovingState(grid, preview, _floorData, _furnitureData, objectPlacer, soundFeedback);
            inputManager.OnClickedLeftButton += PerformAction;
        }
        
        private void PerformAction()
        {
            if (_buildingState == null)
            {
                return;
            }
            
            if (inputManager.IsPointerOverUI())
            {
                return;
            }

            Vector3 mousePosition = inputManager.GetSelectedTileMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            _buildingState.OnAction(gridPosition);
        }
        
        public void StopPlacement()
        {
            if (!_isPlacementInitialized)
            {
                return;
            }
            _isPlacementInitialized = false;
            soundFeedback.PlaySound(SoundType.Click);
            if (_buildingState == null)
            {
                return;
            }

            gridVisualization.SetActive(false);
            _buildingState.EndState();
            inputManager.OnClickedRightButton -= PerformAction;
            inputManager.OnExit -= StopPlacement;
            _lastDetectedPosition = Vector3Int.zero;
            _buildingState = null;
        }
        
        public void StopRemoving()
        {
            if (!_isRemovingInitialized)
            {
                return;
            }

            _isRemovingInitialized = false;

            if (_buildingState == null)
            {
                return;
            }

            gridVisualization.SetActive(false);
            _buildingState.EndState();
            inputManager.OnClickedLeftButton -= PerformAction;
            _lastDetectedPosition = Vector3Int.zero;
            _buildingState = null;
        }
    }
}