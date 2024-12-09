/*
    This script is for the tutorial and is not finished
    The tutorial script will get bigger with more complex features but due to time constraints will stay like this for now.
    Left to add:
        1. Timing mechanism
        2. Voice actor recordings
        3. Ability tutorial
        4. Item tutorial
        5. Inventory tutorial
        6. Teammate Revive tutorial
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string instructionText; // Text for instructions
    }

    public TutorialStep[] tutorialSteps; // Array of tutorial steps
    public TextMeshProUGUI tutorialTextUI; // UI Text for instructions

    [SerializeField] private int currentStepIndex = 0; // Track current tutorial step
    private Coroutine currentCoroutine = null; // Store the current coroutine

    // Input flags for WASD keys, Advanced Movement and Shooting
    private bool pressedW = false;
    private bool pressedA = false;
    private bool pressedS = false;
    private bool pressedD = false;

    private bool pressedJump = false;
    private bool pressedCrouch = false;
    private bool pressedSprint = false;

    private bool shootPressed = false;
    private bool reloadPressed = false;

    public GameObject weapon; // Reference to the weapon GameObject to enable during the shoot step

    void Start()
    {
        StartTutorial();
    }

    void StartTutorial()
    {
        currentStepIndex = 0;
        ShowStep(currentStepIndex);
    }

    void ShowStep(int stepIndex)
    {
        if (stepIndex >= tutorialSteps.Length)
        {
            Debug.Log("Tutorial Complete!");
            tutorialTextUI.text = "Tutorial Complete!";
            return;
        }

        // Show the current step's text
        tutorialTextUI.text = tutorialSteps[stepIndex].instructionText;

        // Stop any previous coroutine
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Reset flags and start the appropriate step coroutine
        switch (stepIndex)
        {
            case 0: // WASD Movement Step
                ResetWASDFlags();
                currentCoroutine = StartCoroutine(WaitForMovementInput());
                break;
            case 1: // Advanced Movement Step
                ResetAdvancedMovementFlags();
                currentCoroutine = StartCoroutine(WaitForAdvancedMovementInput());
                break;
            case 2: // Shoot and Reload Step
                ResetShootReloadFlags();
                MakeWeaponActive();
                currentCoroutine = StartCoroutine(WaitForShootAndReload());
                break;
        }
    }

    void ResetWASDFlags()
    {
        pressedW = pressedA = pressedS = pressedD = false;
    }

    void ResetAdvancedMovementFlags()
    {
        pressedJump = pressedCrouch = pressedSprint = false;
    }

    void ResetShootReloadFlags()
    {
        shootPressed = reloadPressed = false;
    }

    void MakeWeaponActive()
    {
        if (weapon != null)
        {
            weapon.SetActive(true); // Enable the weapon GameObject
            Debug.Log("Weapon activated for shoot tutorial.");
        }
        else
        {
            Debug.LogWarning("Weapon GameObject is not assigned!");
        }
    }

    IEnumerator WaitForMovementInput()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.W)) pressedW = true;
            if (Input.GetKeyDown(KeyCode.A)) pressedA = true;
            if (Input.GetKeyDown(KeyCode.S)) pressedS = true;
            if (Input.GetKeyDown(KeyCode.D)) pressedD = true;

            // Check if all WASD keys have been pressed
            if (pressedW && pressedA && pressedS && pressedD)
            {
                Debug.Log("WASD movement completed!");
                currentStepIndex++;
                ShowStep(currentStepIndex);
                yield break; // Exit coroutine
            }

            yield return null;
        }
    }

    IEnumerator WaitForAdvancedMovementInput()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space)) pressedJump = true;
            if (Input.GetKeyDown(KeyCode.LeftControl)) pressedCrouch = true;
            if (Input.GetKeyDown(KeyCode.LeftShift)) pressedSprint = true;

            // Check if all advanced keys have been pressed
            if (pressedJump && pressedCrouch && pressedSprint)
            {
                Debug.Log("Advanced movement completed!");
                currentStepIndex++;
                ShowStep(currentStepIndex);
                yield break; // Exit coroutine
            }

            yield return null;
        }
    }

    IEnumerator WaitForShootAndReload()
    {
        while (true)
        {
            if (Input.GetButtonDown("Fire1")) shootPressed = true; // "Fire1" is mapped to the left mouse button by default
            if (Input.GetKeyDown(KeyCode.R)) reloadPressed = true; // "R" for reload

            // Check if both shooting and reloading actions have been performed
            if (shootPressed && reloadPressed)
            {
                Debug.Log("Shooting and Reloading completed!");
                currentStepIndex++;
                ShowStep(currentStepIndex);
                yield break; // Exit coroutine
            }

            yield return null;
        }
    }
}
