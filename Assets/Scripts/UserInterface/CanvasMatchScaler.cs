using UnityEngine;
using UnityEngine.UI;

namespace GG.UserInterface
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasMatchScaler : MonoBehaviour
    {
        private void Start()
        {
            SetScale();
        }

        private void SetScale()
        {
            var canvasScaler = GetComponent<CanvasScaler>();
            var screenSize = new Vector2(Screen.width, Screen.height);
            
            var referenceAspectRatio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;
            var currentAspectRatio = screenSize.x / screenSize.y;

            if (currentAspectRatio <= referenceAspectRatio)
                canvasScaler.matchWidthOrHeight = 0f;
            else
                canvasScaler.matchWidthOrHeight = 1f;
        }
    }
}