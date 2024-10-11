using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasVictory : UICanvas
{
    [SerializeField] private TextMeshProUGUI countText;
    public void RetryButton()
    {
        Close(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextButton()
    {
        Close(0);
        LevelManager.currentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetBestScore(int bestScore)
    {
        countText.text = bestScore.ToString(); 
    }

    public void MainMenuButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }
}
