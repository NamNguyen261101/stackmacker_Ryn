using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasGameplay : UICanvas
{
    //[SerializeField] private TextMeshProUGUI countText;

    public override void Setup()
    {
        base.Setup();
        //  UpdateCountText(0);
    }
    /*public void UpdateCountText(int count)
    {
        countText.text = count.ToString();
    }*/
    public void SettingButton()
    {
        UIManager.Instance.OpenUI<CanvasSetting>().SetState(this);
    }
}
