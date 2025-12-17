using UnityEngine;

public class CameraOrbitFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;                // Drag Batman here
    public Vector3 offset = new Vector3(0f, 2f, -5f);  // Camera offset from player

    [Header("Follow Settings")]
    public float followSpeed = 10f;         // Position follow speed
    public float rotationFollowSpeed = 5f;  // Rotation follow speed

    [Header("Orbit Controls")]
    public bool orbitEnabled = true;        // Can orbit around player
    public float orbitSpeed = 2f;           // Mouse orbit speed
    public float verticalAngleLimit = 80f;  // Up/down look limit

    [Header("Collision Settings")]
    public LayerMask collisionMask;         // Set to "Default"
    public float collisionOffset = 0.3f;    // Push camera out from walls

    private Vector3 currentOffset;
    private float currentYaw = 0f;          // Horizontal rotation
    private float currentPitch = 20f;       // Vertical rotation (start looking slightly down)
    private Vector3 smoothVelocity;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }

        // Initialize offset
        currentOffset = offset;

        // Start with camera behind player
        currentYaw = target.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleOrbitInput();
        UpdateCameraPosition();
        HandleCameraCollision();
    }

    void HandleOrbitInput()
    {
        if (!orbitEnabled) return;

        // Get mouse input for orbiting
        if (Input.GetMouseButton(1))  // Right mouse button
        {
            currentYaw += Input.GetAxis("Mouse X") * orbitSpeed;
            currentPitch -= Input.GetAxis("Mouse Y") * orbitSpeed;
        }
        else
        {
            // If not manually orbiting, follow player's rotation
            currentYaw = Mathf.LerpAngle(currentYaw, target.eulerAngles.y, rotationFollowSpeed * Time.deltaTime);
        }

        // Clamp vertical angle
        currentPitch = Mathf.Clamp(currentPitch, -verticalAngleLimit, verticalAngleLimit);
    }

    void UpdateCameraPosition()
    {
        // Calculate rotation based on angles
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        // Calculate desired position (rotated offset behind player)
        Vector3 desiredPosition = target.position + rotation * offset;

        // Smoothly move to desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref smoothVelocity, 1f / followSpeed);

        // Make camera look at player (slightly above center)
        Vector3 lookTarget = target.position + Vector3.up * 1f;
        transform.LookAt(lookTarget);
    }

    void HandleCameraCollision()
    {
        Vector3 cameraToTarget = target.position - transform.position;
        float distance = cameraToTarget.magnitude;

        // Raycast from camera to player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cameraToTarget.normalized, out hit, distance, collisionMask))
        {
            // If something is between camera and player
            float newDistance = hit.distance - collisionOffset;

            // Move camera closer to player
            Vector3 adjustedPosition = transform.position + cameraToTarget.normalized * (distance - newDistance);
            transform.position = adjustedPosition;
        }
    }

    // Public method to reset camera behind player
    public void ResetBehindPlayer()
    {
        currentYaw = target.eulerAngles.y;
        currentPitch = 20f;
    }

    // Toggle between orbit and follow modes
    public void ToggleOrbit(bool enable)
    {
        orbitEnabled = enable;
    }
}