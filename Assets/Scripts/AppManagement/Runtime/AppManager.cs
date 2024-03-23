using UnityEngine;

namespace GG.AppManagement
{
    public class AppManager : MonoBehaviour
    {
        [SerializeField] private AppSettings _appSettings;

        private void Awake()
        {
            _appSettings.Apply();
        }
    }
}