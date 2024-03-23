using UnityEngine;

namespace GG.UserInterface
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] private Canvas _rootCanvas;

        [SerializeField][Range(0f, 1f)] private float _areaMultiplier = 1f;

        private Vector3 CanvasScale
        {
            get
            {
                if (_rootCanvas == null)
                    _rootCanvas = transform.GetComponentInParent<Canvas>();

                return _rootCanvas.transform.localScale;
            }
        }

        private void Start()
        {
            AdjustRect();
        }

        [ContextMenu("Adjust")]
        public void AdjustRect()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            Rect safeAreaRect = Screen.safeArea;

            var xMinMoveAmount = safeAreaRect.xMin * _areaMultiplier;
            var yMinMoveAmount = safeAreaRect.yMin * _areaMultiplier;
            var xMaxMoveAmount = (Screen.width - safeAreaRect.xMax) * _areaMultiplier;
            var yMaxMoveAmount = (Screen.height - safeAreaRect.yMax) * _areaMultiplier;


            var xminOffset = xMinMoveAmount / CanvasScale.x;
            var yminOffset = yMinMoveAmount / CanvasScale.y;
            var xmaxOffset = xMaxMoveAmount / CanvasScale.x;
            var ymaxOffset = yMaxMoveAmount / CanvasScale.y;

            rectTransform.offsetMin = new Vector2(xminOffset, yminOffset);
            rectTransform.offsetMax = new Vector2(-xmaxOffset, -ymaxOffset);
        }
    }
}