using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GG.AppManagement.Editor
{
    public static class AppSettingsEditor
    {
        [MenuItem("Assets/Create/GG/AppSettings")]
        public static void CreateAppSettings()
        {
            var filter = "t:" + typeof(AppSettings).Name;
            var settingsAssets = AssetDatabase.FindAssets(filter);

            if (settingsAssets != null)
            {
                if(settingsAssets.Length > 0)
                {
                    var path = AssetDatabase.GUIDToAssetPath(settingsAssets[0]);
                    Debug.LogError("An App Settings asset is already exist at: " + path);
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath(path, typeof(AppSettings));
                    return;
                }
            }

            var targetPath = GetCurrentPath() + "/AppSettings.asset";
            var settings = ScriptableObject.CreateInstance<AppSettings>();
            AssetDatabase.CreateAsset(settings, targetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = settings;
        }

        private static string GetCurrentPath()
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            return obj.ToString();
        }
    }
}