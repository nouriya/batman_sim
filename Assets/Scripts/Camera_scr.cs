using UnityEngine;

public class CameraOrbitFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0f, -2.5f, -10f);  // Camera offset from player

    [Header("Follow Settings")]
    public float followSpeed = 10f;         // Position follow speed
    public float rotationFollowSpeed = 5f;  // Rotation follow speed

    [Header("Orbit Controls")]
    public bool orbitEnabled = true;        // Can orbit around player
    public float orbitSpeed = 2f;           // Mouse orbit speed
    public float verticalAngleLimit = 80f;  // Up/down look limit

    // throws camera out of the walls
    [Header("Collision Settings")]
    public LayerMask collisionMask;
    public float collisionOffset = 0.3f;

    //camera rotation
    private Vector3 currentOffset;
    private float currentYaw = 0f; 
    private float currentPitch = 20f;   
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

    // orbit the camera pos by right click 
    void HandleOrbitInput()
    {
        if (!orbitEnabled) return;

   
        if (Input.GetMouseButton(1))
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
    /// <summary>
    /// cammera had to follow the car
    /// </summary>
    void UpdateCameraPosition()
    {
        // Calculate angles
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        // Calculate position 
        Vector3 desiredPosition = target.position + rotation * offset;

        // move to position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref smoothVelocity, 1f / followSpeed);

        // Make camera look at player
        Vector3 lookTarget = target.position + Vector3.up * 1f;
        transform.LookAt(lookTarget);
    }
    /// <summary>
    /// wrote this for collision control but didn't actually use it
    /// </summary>
    void HandleCameraCollision()
    {
        Vector3 cameraToTarget = target.position - transform.position;
        float distance = cameraToTarget.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, cameraToTarget.normalized, out hit, distance, collisionMask))
        {
            float newDistance = hit.distance - collisionOffset;

            Vector3 adjustedPosition = transform.position + cameraToTarget.normalized * (distance - newDistance);
            transform.position = adjustedPosition;
        }
    }

    /// <summary> 
    /// Public method to reset camera behind player
    /// </summary>
    public void ResetBehindPlayer()
    {
        currentYaw = target.eulerAngles.y;
        currentPitch = 20f;
    }
    /// <summary>
    // Toggle between orbit and follow modes
    /// </summary>
    /// <param name="enable"></param>
    public void ToggleOrbit(bool enable)
    {
        orbitEnabled = enable;
    }
}