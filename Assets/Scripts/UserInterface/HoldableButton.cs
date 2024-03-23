using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GG.UserInterface
{
    [RequireComponent(typeof(Button))]
    public class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _holdTime = 1f;
        [SerializeField] private UnityEvent _onClick;
        [SerializeField] private UnityEvent _onHold;

        private IEnumerator _activeCoroutine = null;
        private Button _button;

        public UnityEvent OnHold => _onHold;
        public UnityEvent OnClick => _onClick;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _activeCoroutine = WaitForHold(OnHolded);
            StartCoroutine(_activeCoroutine);

            _button.OnSelect(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_activeCoroutine != null)
            {
                StopCoroutine(_activeCoroutine);
                OnClicked();
            }

            _activeCoroutine = null;
            _button.OnDeselect(eventData);
        }

        private IEnumerator WaitForHold(Action onEnd)
        {
            yield return new WaitForSeconds(_holdTime);

            _button.OnDeselect(null);
            _activeCoroutine = null;
            onEnd();
        }

        private void OnHolded()
        {
            _onHold?.Invoke();
        }

        private void OnClicked()
        {
            _onClick?.Invoke();
        }
    }
}