using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectScaler : MonoBehaviour
{
    private void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();

        float xScaleFactor = Screen.width / canvasScaler.referenceResolution.x;
        float yScaleFactor = Screen.height / canvasScaler.referenceResolution.y;

        Vector2 localScale = rectTransform.localScale;
        localScale.x *= xScaleFactor;
        localScale.y *= yScaleFactor;
        rectTransform.localScale = localScale;
    }
}
