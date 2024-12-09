using UnityEngine;

public class MapBoundary : MonoBehaviour
{
    [Header("Boundary Settings")]
    public float initialRadius = 50f;  // Initial size of the safe zone
    public float finalRadius = 10f;    // Final size of the safe zone when it finishes shrinking
    public float shrinkDuration = 300f;  // Time in seconds it takes to shrink to final radius
    public float damagePerSecond = 5f;  // Damage taken per second outside the zone

    [Header("Visual Settings")]
    public GameObject boundaryMarker; // Visual marker for the boundary
    public float boundaryHeight = 5f; // Adjustable height for the boundary marker

    private float currentRadius;
    private float timeElapsed = 0f;

    void Start()
    {
        currentRadius = initialRadius;
    }

    void Update()
    {
        // Shrink the safe zone over time
        ShrinkSafeZone();

        if (boundaryMarker != null)
        {
            boundaryMarker.transform.localScale = new Vector3(currentRadius * 2, boundaryHeight, currentRadius * 2);
        }
    }

    void ShrinkSafeZone()
    {
        // Shrink the zone over time
        timeElapsed += Time.deltaTime;
        currentRadius = Mathf.Lerp(initialRadius, finalRadius, timeElapsed / shrinkDuration);

        // Ensure it doesn't shrink beyond the final radius
        if (currentRadius <= finalRadius)
        {
            currentRadius = finalRadius;
        }
    }

    // Check if the player is outside the safe zone and apply damage
    public void ApplyDamageOutsideZone(Transform playerTransform)
    {
        float distanceFromCenter = Vector3.Distance(playerTransform.position, transform.position);

        // If player is outside the zone, apply damage
        if (distanceFromCenter > currentRadius)
        {
            float distanceFactor = (distanceFromCenter - currentRadius) / 10f;
            float damage = damagePerSecond * distanceFactor;

            // Apply damage to player (use your health script to handle this)
            Health playerHealth = playerTransform.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage * Time.deltaTime);
            }
        }
    }
}
