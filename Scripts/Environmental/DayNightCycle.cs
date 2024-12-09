/*
    This script is here to add fun and randomness to the game
*/

using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Settings")]
    public float dayDurationInMinutes = 10f; // Length of the day phase in real-time minutes
    public float nightDurationInMinutes = 10f; // Length of the night phase in real-time minutes
    public Light sunLight; // Reference to the directional light acting as the sun

    private float totalCycleDuration; // Total duration for day + night cycle
    private float currentTime; // Elapsed time within the current cycle

    void Start()
    {
        totalCycleDuration = (dayDurationInMinutes + nightDurationInMinutes) * 60f; // Convert to seconds
        currentTime = Random.Range(0f, totalCycleDuration); // Random start time within the cycle
        UpdateSunPosition();
    }

    void Update()
    {
        // Increment the current time within the cycle
        currentTime += Time.deltaTime;
        currentTime %= totalCycleDuration; // Loop back to 0 after a full cycle
        UpdateSunPosition();
    }

    private void UpdateSunPosition()
    {
        float sunAngle = (currentTime / totalCycleDuration) * 360f - 90f;

        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 0, 0);

        // Adjust light intensity based on the angle (day: brighter, night: darker)
        sunLight.intensity = Mathf.Clamp01(Mathf.Sin(sunAngle * Mathf.Deg2Rad));
    }
}
