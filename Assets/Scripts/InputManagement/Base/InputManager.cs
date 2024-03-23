using GG.LevelSystem;
using UnityEngine;

namespace GG.InputManagement
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputModule[] _modules;

        [SerializeField] private OnLevelStarted _onLevelStarted;
        [SerializeField] private OnLevelCompleted _onLevelCompleted;

        private void Awake()
        {
            foreach (var module in _modules)
            {
                module.Disable();
            }
        }

        private void OnEnable()
        {
            _onLevelStarted.RegisterListener(EnableClickModule);
            _onLevelCompleted.RegisterListener(DisableClickModule);
        }

        private void OnDisable()
        {
            _onLevelStarted.RemoveListener(EnableClickModule);
            _onLevelCompleted.RemoveListener(DisableClickModule);
        }

        private void EnableClickModule()
        {
            _modules[0].Enable();
        }

        private void DisableClickModule()
        {
            _modules[0].Disable();
        }
    }
}