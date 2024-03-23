using UnityEngine;
using UnityEngine.EventSystems;

namespace GG.InputManagement
{
    public class ClickInputModule : InputModule, IPointerClickHandler
    {
        [SerializeField] private OnClickInputReceived _onClickInputReceived;

        public override void Enable()
        {
            base.Enable();

            gameObject.SetActive(true);
        }

        public override void Disable()
        {
            base.Disable();

            gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var clickData = new ClickInputData(eventData, eventData.position);
            _onClickInputReceived.Broadcast(clickData);
        }
    }
}