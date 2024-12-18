using UnityEngine;
using UnityEngine.Events;

namespace C11y.Navigation
{
    public class MapController : MonoBehaviour
    {
        [Header("Map Settings")]
        [SerializeField] private float mapScale = 1f;              // Meters per unity unit
        [SerializeField] private Vector2 gridSize = new(1000, 1000); // Map dimensions in meters

        [Header("Grid Settings")]
        [SerializeField] private bool showGrid = true;
        [SerializeField] private float gridSpacing = 100f;         // Grid line spacing in meters
        [SerializeField] private Color gridColor = Color.gray;

        public UnityEvent<Vector2> onMapClick;                     // Event fired when map is clicked
        private Camera mainCamera;
        private Vector2 mapCenter;

        private void Start()
        {
            mainCamera = Camera.main;
            mapCenter = transform.position;
            onMapClick = new UnityEvent<Vector2>();
        }

        private void Update()
        {
            HandleMapInput();
        }

        private void OnDrawGizmos()
        {
            if (showGrid)
            {
                DrawGrid();
            }
        }

        private void HandleMapInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector2 worldPosition = new(hit.point.x, hit.point.z);
                    Vector2 mapPosition = WorldToMapCoordinates(worldPosition);
                    onMapClick.Invoke(mapPosition);
                }
            }
        }

        private void DrawGrid()
        {
            Gizmos.color = gridColor;

            // Draw vertical lines
            for (float x = -gridSize.x/2; x <= gridSize.x/2; x += gridSpacing)
            {
                Vector3 start = transform.position + new Vector3(x, 0, -gridSize.y/2);
                Vector3 end = transform.position + new Vector3(x, 0, gridSize.y/2);
                Gizmos.DrawLine(start, end);
            }

            // Draw horizontal lines
            for (float z = -gridSize.y/2; z <= gridSize.y/2; z += gridSpacing)
            {
                Vector3 start = transform.position + new Vector3(-gridSize.x/2, 0, z);
                Vector3 end = transform.position + new Vector3(gridSize.x/2, 0, z);
                Gizmos.DrawLine(start, end);
            }
        }

        /// <summary>
        /// Converts world coordinates to map coordinates
        /// </summary>
        public Vector2 WorldToMapCoordinates(Vector2 worldPos)
        {
            return (worldPos - mapCenter) * mapScale;
        }

        /// <summary>
        /// Converts map coordinates to world coordinates
        /// </summary>
        public Vector2 MapToWorldCoordinates(Vector2 mapPos)
        {
            return (mapPos / mapScale) + mapCenter;
        }

        /// <summary>
        /// Sets the map scale (meters per unity unit)
        /// </summary>
        public void SetMapScale(float scale)
        {
            mapScale = Mathf.Max(0.1f, scale);
        }

        /// <summary>
        /// Gets the current map scale
        /// </summary>
        public float GetMapScale()
        {
            return mapScale;
        }

        /// <summary>
        /// Toggles the grid visibility
        /// </summary>
        public void ToggleGrid(bool show)
        {
            showGrid = show;
        }
    }
} 