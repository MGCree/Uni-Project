using UnityEngine;

public class Pistol : Weapon
{
    [Header("Pistol Specific Settings")]
    public float range = 50f; // The shooting range of the pistol
    public float fireRate = 0.5f; // Fire rate in seconds

    [Header("Damage Multipliers")]
    public float headshotMultiplier = 2.5f;
    public float bodyMultiplier = 1.0f;

    [Header("References")]
    public Transform shootPoint;
    public Camera playerCamera;
    public AudioSource gunshotAudioSource; // Reference to AudioSource
    public AudioClip gunshotSound; // Reference to the gunshot sound

    private float nextTimeToFire = 0f;

    void Start()
    {
        // Set initial ammo
        currentAmmo = maxAmmo;
        if (gunshotAudioSource == null)
        {
            gunshotAudioSource = GetComponent<AudioSource>(); // Try to get AudioSource if not assigned
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            UseWeapon();
        }
    }

    public override void UseWeapon()
    {
        if (currentAmmo <= 0) return; // No ammo to shoot

        currentAmmo--;

        // Play the gunshot sound
        PlayGunshotSound();

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Hit object: " + hit.collider.name);

            // Check if the hit object or its parent has the Enemy tag
            Transform enemyTransform = hit.transform;
            while (enemyTransform != null && !enemyTransform.CompareTag("Enemy"))
            {
                enemyTransform = enemyTransform.parent;
            }

            if (enemyTransform != null && enemyTransform.CompareTag("Enemy"))
            {
                ApplyDamage(hit, enemyTransform);
            }
        }
    }

    void PlayGunshotSound()
    {
        if (gunshotAudioSource != null && gunshotSound != null)
        {
            gunshotAudioSource.PlayOneShot(gunshotSound);
        }
        else
        {
            Debug.LogWarning("Gunshot audio source or sound clip not assigned!");
        }
    }

    void ApplyDamage(RaycastHit hit, Transform enemy)
    {
        float finalDamage = damage;

        // Determine the body part hit based on the object's name
        if (hit.collider.name == "Head")
        {
            finalDamage *= headshotMultiplier;
        }
        else if (hit.collider.name == "Body")
        {
            finalDamage *= bodyMultiplier;
        }

        // Get the Health component from the parent GameObject
        Transform parentTransform = hit.collider.transform.root; // Get the top-level parent

        Health enemyHealth = parentTransform.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(finalDamage);
            Debug.Log($"Applied {finalDamage} damage to {parentTransform.name}");
        }
        else
        {
            Debug.LogWarning($"Health component not found on {parentTransform.name}");
        }
    }

    public void ReloadPistol()
    {
        Reload();
    }
}
