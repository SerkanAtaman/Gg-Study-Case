using UnityEngine;

namespace GG.InputManagement
{
    public abstract class InputModule : MonoBehaviour
    {
        [SerializeField] protected InputModule[] conflictedModules;

        public InputModule[] ConflictedModules => conflictedModules;

        public virtual void Enable()
        {
            enabled = true;
        }

        public virtual void Disable()
        {
            enabled = false;
        }
    }
}