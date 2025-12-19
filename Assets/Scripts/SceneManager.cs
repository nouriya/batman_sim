using UnityEngine;
using System.Collections;
using System;

public class SceneManager : MonoBehaviour
{
    // ---------------------------------------- STATE SYSTEM ----------------------------------------
    public enum BatmanState { Normal, Stealth, Alert }
    public BatmanState currentState = BatmanState.Normal;

    // Speed values for each state
    private float normalSpeed = 7f;
    private float stealthSpeed = 4f;
    private float alertSpeed = 10f;

    //adding the damn shift speed
    private float Shift_speed = 5f;

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
    public GameObject batSignalObject;
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
        }
        else
        {

            defaultLightIntensity = 1.0f; 
        }

        SetAlertLightsActive(false);

        ApplyStateEffects();
    }

    void Update()
    {

        // Handle all input and state changes
        HandleStateInput();
        HandleBatSignal();


        // handlign speed change through shift press
        if (Input.GetKey(KeyCode.LeftShift))
        {

            Shift_speed = 5;
        }
        else
        {

            Shift_speed = 0;
        }

    }

    // ---------------------------------------- PUBLIC METHODS ----------------------------------------
    
    /// <summary>
    /// return current speed or what it shoube based on the state we are 
    /// as well if shift is pressed or not
    /// </summary>
    /// <returns>calculated speed</returns>
    public float GetCurrentMoveSpeed()
    {
        switch (currentState)
        {
            case BatmanState.Normal:
                return normalSpeed + Shift_speed;
            case BatmanState.Stealth:
                return stealthSpeed + Shift_speed;
            case BatmanState.Alert:
                return alertSpeed + Shift_speed;
            default:
                return normalSpeed + Shift_speed;
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
    /// <summary>
    /// changes light intensity based on state
    /// </summary>
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
    /// <summary>
    /// change light intensity based on the current state
    /// </summary>
    /// <param name="intensity"></param>
    
    void SetEnvironmentLightIntensity(float intensity)
    {
        if (environmentLight != null)
        {
            environmentLight.intensity = intensity;
        }
    }
    /// <summary>
    /// play the alarm soundeffect
    /// </summary>
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


    /// <summary>
    ///  basically switches colors for the alarm
    /// </summary>
    /// <returns></returns>

    IEnumerator BlinkAlertLights()
    {
        int colorIndex = 0;
        while (currentState == BatmanState.Alert)
        {

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

    /// <summary>
    // you can have multiple light to switch from but I only placed 2 here
    // enables them on active state
    /// </summary>
    void SetAlertLightsActive(bool isActive)
    {
        foreach (Light light in alertLights)
        {
            if (light != null)
                light.enabled = isActive;
        }
    }

    // ---------------------------------------- BAT-SIGNAL CONTROL ----------------------------------------
    
    /// <summary>
    /// chacks if bat signal is active enables and rotates the light that has the bat logo sprite on it
    /// </summary>
    
    void HandleBatSignal()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBatSignal();
        }

        // Rotate the damn Bat-Signal if active
        if (isBatSignalOn && batSignalObject != null)
        {
            batSignalObject.transform.Rotate(0, batSignalRotationSpeed * Time.deltaTime, 0);
        }
    }
    /// <summary>
    /// handles enabling and disabling the batsignal
    /// also used inside HandleBatSignal
    /// </summary>
    void ToggleBatSignal()
    {
        isBatSignalOn = !isBatSignalOn;

        if (batSignalObject != null)
        {
            batSignalObject.SetActive(isBatSignalOn);
        }
        else if (batSignalLight != null)
        {
            batSignalLight.enabled = isBatSignalOn;
        }

    }
}