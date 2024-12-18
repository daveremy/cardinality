using UnityEngine;

namespace C11y.Navigation
{
    public class CompassController : MonoBehaviour
    {
        [Header("Compass Settings")]
        [SerializeField] private float magneticDeclination = 0f;  // Local magnetic declination
        [SerializeField] private float smoothing = 5f;            // Smoothing factor for compass movement

        [Header("References")]
        [SerializeField] private Transform compassRose;           // Reference to the compass rose visual
        [SerializeField] private Transform needleTransform;       // Reference to the compass needle

        private float currentHeading;                             // Current compass heading in degrees
        private float targetHeading;                              // Target heading for smooth rotation

        private void Start()
        {
            // Initialize compass heading
            currentHeading = transform.eulerAngles.y;
            targetHeading = currentHeading;

            // Enable device compass if available
            Input.compass.enabled = true;
        }

        private void Update()
        {
            UpdateCompassOrientation();
        }

        private void UpdateCompassOrientation()
        {
            // Get device compass heading if available
            if (SystemInfo.supportsGyroscope)
            {
                float trueHeading = Input.compass.trueHeading;
                targetHeading = trueHeading + magneticDeclination;
            }

            // Smoothly rotate the compass
            currentHeading = Mathf.LerpAngle(currentHeading, targetHeading, Time.deltaTime * smoothing);

            // Update visual components
            if (compassRose != null)
            {
                compassRose.rotation = Quaternion.Euler(0, currentHeading, 0);
            }

            if (needleTransform != null)
            {
                needleTransform.rotation = Quaternion.Euler(0, currentHeading, 0);
            }
        }

        /// <summary>
        /// Sets the magnetic declination for the current location
        /// </summary>
        public void SetMagneticDeclination(float declination)
        {
            magneticDeclination = declination;
        }

        /// <summary>
        /// Gets the current compass heading in degrees
        /// </summary>
        public float GetCurrentHeading()
        {
            return currentHeading;
        }

        /// <summary>
        /// Takes a bearing on a target position
        /// </summary>
        public float TakeBearing(Vector3 targetPosition)
        {
            Vector3 directionToTarget = targetPosition - transform.position;
            float bearing = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
            return (bearing + 360f) % 360f;
        }
    }
} 