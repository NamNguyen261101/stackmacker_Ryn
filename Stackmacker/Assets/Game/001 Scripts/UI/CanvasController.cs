using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;

public class CanvasController : Singleton<CanvasController>
{
    [SerializeField] private GameObject _panelMenu, _panelInGame, _panelEndGame;
    [SerializeField] private TextMeshProUGUI _textStackIndicator;
    [SerializeField] private TextMeshProUGUI _textUISwipe;

    /// <summary>
    /// Enable get a game start
    /// </summary>
    private void OnEnable()
    {
        GameManager.ActionGameStart += SetInGameUI;
        GameManager.ActionLevelPassed += SetEndGameUI;
    }

    /// <summary>
    /// Set In Game After Player Swipe
    /// </summary>
    private void SetInGameUI()
    {
        _textUISwipe.enabled = false;
        
        _panelMenu.SetActive(false);
        _panelInGame.SetActive(true);
        
    }

    /// <summary>
    /// Set End Game UI
    /// </summary>
    private void SetEndGameUI()
    {
        _panelEndGame.SetActive(true);
    }

    #region UI Buttons' methods
    public void ButtonRestartPressed()
    {
        GameManager.Instance.RestartLevel();
    }

    public void ButtonStartPressed()
    {
        GameManager.ActionGameStart.Invoke();
    }

    public void ButtonNextLevelPressed()
    {
        GameManager.Instance.LoadNextLevel();
    }
    #endregion

    /// <summary>
    /// Update Text when player get a brick
    /// </summary>
    /// <param name="stackBrickSize"></param>
    public void UpdateStackIndicatorText(int stackBrickSize)
    {
        _textStackIndicator.text = stackBrickSize.ToString();
    }
    /// <summary>
    /// Disable when call it
    /// </summary>
    private void OnDisable()
    {
        GameManager.ActionGameStart -= SetInGameUI;
        GameManager.ActionLevelPassed -= SetEndGameUI;
    }
}
