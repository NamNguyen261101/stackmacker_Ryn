using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] bool isDestroyOnClose = false;

    // xu ly tai tho cho cac con game

    // Goi truoc khi duoc active
    public virtual void Setup()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float ratio = (float) Screen.width / (float) Screen.height;

        if (ratio > 2.1f)
        {
            Vector2 leftBottom = rect.offsetMin;
            Vector2 rightTop = rect.offsetMax;

            leftBottom.y = 0f;
            rightTop.y = -100f;

            rect.offsetMin = leftBottom;
            rect.offsetMax = rightTop;
        }
    }

    // Goi sau khi dc active
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    // tat canvas sau time
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseDirectly), time);
    }

    // tat canvas truc tiep 
    public virtual void CloseDirectly()
    {
        if (isDestroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
