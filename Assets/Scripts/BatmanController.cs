using UnityEngine;

public class BatmanController : MonoBehaviour
{
    // Movement parameters
    public float turnSpeed = 100f;
    private float currentMoveSpeed = 10f;

    // Reference to the SceneManager to get current state -> Will implement later
    //private SceneManager sceneManager;

    void Start()
    {
        // Find the SceneManager in the scene
            /*
             if couldn't find it debug error
             */
        
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Get movement input
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        
        // Apply movement + current speed
        transform.Translate(0, 0, moveInput * currentMoveSpeed * Time.deltaTime);
        // Apply rotation
        transform.Rotate(0, turnInput * turnSpeed * Time.deltaTime, 0);
    }

    public void SetMoveSpeed(float speed)
    {
        currentMoveSpeed = speed;
    }
}