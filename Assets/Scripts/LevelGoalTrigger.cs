using UnityEngine;

public class LevelGoalTrigger : MonoBehaviour
{
    // Drag your GameManager (which has the LevelController script) here
    public LevelController levelController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the thing that entered the trigger is the Player
        if (other.CompareTag("Player"))
        {
            // Call the function to show the panel and freeze time
            levelController.ShowEndScreen();

            Debug.Log("Goal Reached!");
        }
    }

    // Use this version if you are making a 3D game
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelController.ShowEndScreen();
        }
    }
    */
}