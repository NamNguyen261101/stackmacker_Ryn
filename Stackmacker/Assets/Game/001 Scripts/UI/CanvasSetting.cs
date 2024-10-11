using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasSetting : UICanvas
{
    [SerializeField] private GameObject[] _btn;

    public void SetState(UICanvas canvas)
    {
        for (int i = 0; i < _btn.Length; i++)
        {
            _btn[i].gameObject.SetActive(false);
        }

        if (canvas is CanvasMainMenu)
        {
            _btn[2].gameObject.SetActive(true);
        }
        else if (canvas is CanvasGameplay)
        {
            _btn[0].gameObject.SetActive(true);
            // _btn[1].gameObject.SetActive(true);
        }
    }

    public void MainMenuButton()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    public void RetryButton()
    {
        Close(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ContinueButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasGameplay>(); //CanvasGameplay
    }
}
