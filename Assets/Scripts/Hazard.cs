using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Settings")]
    public PlayerSpawner spawner; // Changed from SimpleSpawner to PlayerSpawner

    private void Start()
    {
        // Automatically find the spawner in the scene if not assigned
        if (spawner == null)
        {
            spawner = Object.FindFirstObjectByType<PlayerSpawner>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (spawner != null)
            {
                spawner.SpawnPlayer();
            }
            else
            {
                Debug.LogWarning("Hazard: No PlayerSpawner found in scene!");
            }
        }
    }
}