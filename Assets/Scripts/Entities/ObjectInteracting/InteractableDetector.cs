using GG.InputManagement;
using UnityEngine;

namespace GG.ObjectInteracting
{
    public class InteractableDetector : MonoBehaviour
    {
        [SerializeField] private Camera _mainCam;
        [SerializeField] private OnClickInputReceived _onClickInputReceived;

        private void OnEnable()
        {
            _onClickInputReceived.RegisterListener<ClickInputData>(DetectInteractable);
        }

        private void OnDisable()
        {
            _onClickInputReceived.RemoveListener<ClickInputData>(DetectInteractable);
        }

        private void DetectInteractable(ClickInputData clickData)
        {
            var ray = _mainCam.ScreenPointToRay(clickData.ClickScreenPosition);

            if(Physics.Raycast(ray, out var hit))
            {
                if(hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }
}