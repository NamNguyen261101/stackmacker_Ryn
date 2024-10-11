using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Show();

    }
    [ContextMenu("Do ")]
    private void Show()
    {

        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }
}
