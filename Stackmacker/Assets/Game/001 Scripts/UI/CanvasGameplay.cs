using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasGameplay : UICanvas
{
    public void SettingButton()
    {
        UIManager.Instance.OpenUI<CanvasSetting>();
    }
}
