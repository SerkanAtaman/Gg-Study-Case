using UnityEngine;

namespace GG.AppManagement
{
    public class AppSettings : ScriptableObject
    {
        public int TargetFrameRate = 60;

        public void Apply()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = TargetFrameRate;
        }
    }
}