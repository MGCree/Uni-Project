using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMovement : MonoBehaviour
{
    private NavMeshAgent agent; // NavMeshAgent for navigation
    private Transform nearestTarget; // Stores the nearest target

    public Transform shootingPoint; // Position where the gun shoots from
    [SerializeField] private float shootingRange = 100f; // Range to shoot at targets
    [SerializeField] private float fireRate = 1f; // Time between shots
    private int shotsFired = 0; // Counter for shots fired
    private bool isReloading = false;

    public int damage = 10; // Damage dealt per hit

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }

        if (shootingPoint == null)
        {
            Debug.LogError("Shooting point not set on " + gameObject.name);
        }

        StartCoroutine(ShootingLogic());
    }

    void Update()
    {
        FindAndMoveToNearestTarget();
    }

    void FindAndMoveToNearestTarget()
    {
        // Find all objects with "Player" and "Bot" tags
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] bots = GameObject.FindGameObjectsWithTag("Bot");

        float shortestDistance = Mathf.Infinity;
        nearestTarget = null;

        // Combine all targets (players + bots) into one list
        List<GameObject> allTargets = new List<GameObject>();
        allTargets.AddRange(players);
        allTargets.AddRange(bots);

        foreach (GameObject target in allTargets)
        {
            // Ignore itself
            if (target != this.gameObject)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestTarget = target.transform; // Set the nearest target
                }
            }
        }

        // Move towards the nearest target
        if (nearestTarget != null)
        {
            agent.SetDestination(nearestTarget.position);
        }
    }

    IEnumerator ShootingLogic()
    {
        while (true)
        {
            if (isReloading)
            {
                yield return null;
                continue;
            }

            if (nearestTarget != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, nearestTarget.position);

                // Check if target is within range and visible
                if (distanceToTarget <= shootingRange && IsTargetVisible(nearestTarget))
                {
                    PerformHitscanShot(nearestTarget);

                    shotsFired++;
                    if (shotsFired >= 10)
                    {
                        StartCoroutine(Reload());
                    }
                }
            }

            yield return new WaitForSeconds(fireRate);
        }
    }

    bool IsTargetVisible(Transform target)
    {
        RaycastHit hit;
        Vector3 directionToTarget = (target.position - shootingPoint.position).normalized;

        if (Physics.Raycast(shootingPoint.position, directionToTarget, out hit, shootingRange))
        {
            if (hit.transform == target)
            {
                return true; // Target is visible
            }
        }
        return false;
    }

    void PerformHitscanShot(Transform target)
    {
        bool willHit = Random.value <= 0.7f; // 70% chance to hit so that they dont have total aimbot
        Vector3 directionToTarget = (target.position - shootingPoint.position).normalized;

        RaycastHit hit;

        // Perform raycast
        if (Physics.Raycast(shootingPoint.position, directionToTarget, out hit, shootingRange))
        {
            if (hit.transform == target && willHit)
            {
                Debug.Log("Shot hit: " + hit.transform.name);

                // Apply damage if the target has a health script
                Health targetHealth = hit.transform.GetComponent<Health>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(damage);
                }
            }
            else if (willHit == false)
            {
                Debug.Log("Shot missed!");
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(5f); // 5-second reload time
        shotsFired = 0; // Reset shot count
        isReloading = false;
        Debug.Log("Reload complete!");
    }
}
