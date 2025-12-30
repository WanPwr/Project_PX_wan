using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject winScreen;  // UI Panel for Winning
    public GameObject loseScreen; // UI Panel for Game Over
    private bool isGameOver = false;

    public void LevelSolved()
    {
        if (isGameOver) return; // Prevent double triggers

        isGameOver = true;
        winScreen.SetActive(true); // Show the "Victory" UI
        Time.timeScale = 0f;       // Optional: Freeze the game world
        Debug.Log("Level Complete!");
    }

    public void GameOver()
    {
        isGameOver = true;
        loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1f; // Always reset time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}