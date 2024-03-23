using UnityEngine;
using UnityEngine.EventSystems;

namespace GG.InputManagement
{
    public class ClickInputData
    {
        public PointerEventData PointerEventData { get; private set; }
        public Vector2 ClickScreenPosition { get; private set; }

        public ClickInputData(PointerEventData eventData, Vector2 screenPos)
        {
            PointerEventData = eventData;
            ClickScreenPosition = screenPos;
        }
    }
}