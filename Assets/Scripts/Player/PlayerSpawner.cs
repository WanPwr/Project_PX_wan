using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public CameraFollow cameraScript;
    public LevelGoal levelGoalScript;

    private GameObject currentPlayer;

    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        // 1. If a player already exists, destroy them before respawning
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        int selectedID = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject prefab = Resources.Load<GameObject>($"Characters/Player{selectedID}");

        if (prefab != null)
        {
            currentPlayer = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            // Assign to Camera
            if (cameraScript != null) cameraScript.target = currentPlayer.transform;

            // Assign to Level Goal
            if (levelGoalScript != null) levelGoalScript.AssignPlayer(currentPlayer);

            Debug.Log("Player Respawned!");
        }
    }
}