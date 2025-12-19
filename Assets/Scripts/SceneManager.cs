using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    // ---------------------------------------- STATE SYSTEM ----------------------------------------
    public enum BatmanState { Normal, Stealth, Alert }
    public BatmanState currentState = BatmanState.Normal;

    // Speed values for each state
    public float normalSpeed = 10f;
    public float stealthSpeed = 4f;
    public float alertSpeed = 10f;

    // ---------------------------------------- LIGHT & SOUND REFERENCES ----------------------------------------
    [Header("Light Settings")]
    public Light environmentLight;
    private float defaultLightIntensity;

    [Header("Alert System")]
    public Light[] alertLights;
    public Color[] alertColors = { Color.red, Color.blue };
    private bool isBlinking = false;
    private float blinkInterval = 0.5f;

    [Header("Audio")]
    public AudioSource alarmAudioSource;
    public AudioClip alarmSound;

    [Header("Bat-Signal")]
    public Light batSignalLight;
    private bool isBatSignalOn = false;
    public float batSignalRotationSpeed = 10f;

    // ---------------------------------------- UNITY METHODS ----------------------------------------
    void Start()
    {
        // Store initial light intensity
        if (environmentLight != null)
        {
            defaultLightIntensity = environmentLight.intensity;
            Debug.Log($"Default light intensity: {defaultLightIntensity}");
        }
        else
        {
            Debug.LogError("Environment Light not assigned in Inspector!");
            defaultLightIntensity = 1.0f; // Safe default
        }

        // Initialize systems
        SetAlertLightsActive(false);

        // Apply initial state effects
        ApplyStateEffects();
    }

    void Update()
    {
        // Handle all input and state changes
        HandleStateInput();
        HandleBatSignal();
    }

    // ---------------------------------------- PUBLIC METHODS ----------------------------------------
    public float GetCurrentMoveSpeed()
    {
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
        ExitCurrentState();

        // Set new state
        currentState = newState;
        Debug.Log($"State changed to: {newState}");

        // Enter new state
        ApplyStateEffects();
    }


    //--------------------------------------check alert state and adjust stuff acordingly-------------------------------------
    void ExitCurrentState()
    {
        if (currentState == BatmanState.Alert)
        {
            if (isBlinking) StopAllCoroutines();
            isBlinking = false;
            SetAlertLightsActive(false);
            if (alarmAudioSource != null && alarmAudioSource.isPlaying)
                alarmAudioSource.Stop();
        }
    }

    //--------------------------------------change light intensity based on state------------------------------------------------
    void ApplyStateEffects()
    {
        switch (currentState)
        {
            case BatmanState.Normal:
                SetEnvironmentLightIntensity(defaultLightIntensity);
                break;

            case BatmanState.Stealth:
                SetEnvironmentLightIntensity(defaultLightIntensity * 0.3f);
                break;

            case BatmanState.Alert:
                SetEnvironmentLightIntensity(defaultLightIntensity * 1.5f);
                StartAlertEffects();
                break;
        }
    }

    // ---------------------------------------- LIGHT & SOUND EFFECTS ----------------------------------------
    void SetEnvironmentLightIntensity(float intensity)
    {
        if (environmentLight != null)
        {
            environmentLight.intensity = intensity;
            Debug.Log($"Light intensity set to: {intensity}");
        }
    }

    void StartAlertEffects()
    {
        // Start light blinking
        if (!isBlinking)
        {
            StartCoroutine(BlinkAlertLights());
            isBlinking = true;
        }

        // Play alarm sound
        if (alarmAudioSource != null && alarmSound != null && !alarmAudioSource.isPlaying)
        {
            alarmAudioSource.clip = alarmSound;
            alarmAudioSource.loop = true;
            alarmAudioSource.Play();
        }
    }

    IEnumerator BlinkAlertLights()
    {
        int colorIndex = 0;
        while (currentState == BatmanState.Alert)
        {
            // Set all alert lights to current color
            foreach (Light light in alertLights)
            {
                if (light != null)
                {
                    light.enabled = true;
                    light.color = alertColors[colorIndex];
                }
            }

            // Switch to next color
            colorIndex = (colorIndex + 1) % alertColors.Length;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    void SetAlertLightsActive(bool isActive)
    {
        foreach (Light light in alertLights)
        {
            if (light != null)
                light.enabled = isActive;
        }
    }

    // ---------------------------------------- BAT-SIGNAL CONTROL ----------------------------------------
    void HandleBatSignal()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBatSignal();
        }

        // Rotate Bat-Signal if active
        if (isBatSignalOn && batSignalLight != null)
        {
            batSignalLight.transform.Rotate(0, batSignalRotationSpeed * Time.deltaTime, 0);
        }
    }

    void ToggleBatSignal()
    {
        isBatSignalOn = !isBatSignalOn;

        if (batSignalLight != null)
            batSignalLight.enabled = isBatSignalOn;

        Debug.Log($"Bat-Signal: {(isBatSignalOn ? "ON" : "OFF")}");
    }
}