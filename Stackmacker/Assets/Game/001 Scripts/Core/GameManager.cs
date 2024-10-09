using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public static UnityAction ActionGameStart, ActionLevelPassed;
    private int nextLevel;
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        nextLevel = PlayerPrefs.GetInt("LEVEL", 1);
        nextLevel++;
        PlayerPrefs.SetInt("LEVEL", nextLevel);
        SceneManager.LoadScene(nextLevel);
    }
}
