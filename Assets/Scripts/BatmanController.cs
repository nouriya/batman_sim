using UnityEngine;
using UnityEngine.SceneManagement;

public class BatmanController : MonoBehaviour
{
    // Movement parameters
    public float turnSpeed = 100f;
    private float currentMoveSpeed = 10f;

    // Reference to the SceneManager to get current state -> Will implement later
    private SceneManager sceneManager;

    void Start()
    {
        // Find the SceneManager in the scene
        sceneManager = FindObjectOfType<SceneManager>();
        if (sceneManager == null)
        {
            Debug.LogError("SceneManager not found in the scene!");
        }
    //done

    }

    void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// basicallly collects input and handles movement and stuff
    /// </summary>
    void HandleMovement()
    {
        // Get movement input
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Get current speed from SceneManager based on state
        if (sceneManager != null)
        {
            currentMoveSpeed = sceneManager.GetCurrentMoveSpeed();
        }


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