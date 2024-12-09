using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        } else
        {
            return;
        }
    }

    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y; // Keep height fixed
        transform.position = newPosition;
    }
}
