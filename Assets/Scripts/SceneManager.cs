using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    // ---------------------------------------- STATE SYSTEM ----------------------------------------
    public enum BatmanState { Normal, Stealth, Alert }
    public BatmanState currentState = BatmanState.Normal;

    // Speed values for each state Note: will fix the damn access later
    public float normalSpeed = 10f;
    public float stealthSpeed = 4f;
    public float alertSpeed = 10f;

   

    // ---------------------------------------- UNITY METHODS ----------------------------------------
    void Start()
    {
       

        // Initialize systems
        //SetAlertLightsActive(false);

        // Apply initial state effects
        ApplyStateEffects();
    }

    void Update()
    {
        // Handle all input and state changes
        HandleStateInput();
        //HandleBatSignal(); implement later
    }

    // ---------------------------------------- PUBLIC METHODS (For BatmanController) ----------------------------------------
    public float GetCurrentMoveSpeed()
    {
        // Return speed based on current state
        switch (currentState)
        {
            case BatmanState.Normal:
                return normalSpeed;
            case BatmanState.Stealth:
                return stealthSpeed;
            case BatmanState.Alert:
                return alertSpeed;
            default:
                return normalSpeed;
        }
    }

    // ---------------------------------------- INPUT HANDLING ----------------------------------------
    void HandleStateInput()
    {
        if (Input.GetKeyDown(KeyCode.N) && currentState != BatmanState.Normal)
        {
            ChangeState(BatmanState.Normal);
        }
        else if (Input.GetKeyDown(KeyCode.C) && currentState != BatmanState.Stealth)
        {
            ChangeState(BatmanState.Stealth);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && currentState != BatmanState.Alert)
        {
            ChangeState(BatmanState.Alert);
        }
    }

    // ---------------------------------------- STATE MANAGEMENT ----------------------------------------
    void ChangeState(BatmanState newState)
    {
        // Exit current state
        //ExitCurrentState(); #TODO :-)

        // Set new state
        currentState = newState;
        Debug.Log($"State changed to: {newState}");

        // Enter new state
        ApplyStateEffects();
    }

  

    void ApplyStateEffects()
    {
        switch (currentState)
        {
            case BatmanState.Normal:
                //SetEnvironmentLightIntensity(defaultLightIntensity); -> for changing the damn lights will add onto it later
                break;

            case BatmanState.Stealth:
                //SetEnvironmentLightIntensity(defaultLightIntensity * 0.3f); -> same thing
                break;

            case BatmanState.Alert:
                //SetEnvironmentLightIntensity(defaultLightIntensity * 1.5f); -> same thing
                StartAlertEffects();// -> will implement later
                break;
        }
    }

    void StartAlertEffects() { }

}