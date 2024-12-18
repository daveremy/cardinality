using UnityEngine;

namespace C11y.Navigation
{
    public class Waypoint : MonoBehaviour
    {
        [Header("Waypoint Settings")]
        [SerializeField] private string waypointName = "Waypoint";
        [SerializeField] private string description;
        [SerializeField] private Color waypointColor = Color.red;
        [SerializeField] private float radius = 1f;

        [Header("Visual Settings")]
        [SerializeField] private bool showLabel = true;
        [SerializeField] private GameObject visualMarker;

        private void OnDrawGizmos()
        {
            // Draw waypoint marker in scene view
            Gizmos.color = waypointColor;
            Gizmos.DrawWireSphere(transform.position, radius);

            if (showLabel)
            {
                // Draw waypoint name above the marker
                #if UNITY_EDITOR
                UnityEditor.Handles.Label(transform.position + Vector3.up * (radius + 0.5f), waypointName);
                #endif
            }
        }

        /// <summary>
        /// Gets the distance to another waypoint
        /// </summary>
        public float GetDistanceTo(Waypoint other)
        {
            return Vector3.Distance(transform.position, other.transform.position);
        }

        /// <summary>
        /// Gets the bearing to another waypoint
        /// </summary>
        public float GetBearingTo(Waypoint other)
        {
            Vector3 direction = other.transform.position - transform.position;
            float bearing = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            return (bearing + 360f) % 360f;
        }

        /// <summary>
        /// Sets the waypoint name
        /// </summary>
        public void SetName(string name)
        {
            waypointName = name;
        }

        /// <summary>
        /// Gets the waypoint name
        /// </summary>
        public string GetName()
        {
            return waypointName;
        }

        /// <summary>
        /// Sets the waypoint description
        /// </summary>
        public void SetDescription(string desc)
        {
            description = desc;
        }

        /// <summary>
        /// Gets the waypoint description
        /// </summary>
        public string GetDescription()
        {
            return description;
        }

        /// <summary>
        /// Sets the waypoint color
        /// </summary>
        public void SetColor(Color color)
        {
            waypointColor = color;
            if (visualMarker != null)
            {
                var renderer = visualMarker.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = color;
                }
            }
        }
    }
} 