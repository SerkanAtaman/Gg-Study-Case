using NaughtyAttributes;
using UnityEngine;
using Newtonsoft.Json;
using GG.Language;

namespace GG.LevelSystem
{
    [CreateAssetMenu(menuName = "GG/LevelSystem/LevelDataHolder")]
    public class LevelDataHolder : ScriptableObject
    {
        [SerializeField] private int _levelID;
        [SerializeField] private LevelData _levelData;
        [SerializeField] private LanguageModel _languageModel;

        public int LevelID => _levelID;
        public LevelData LevelData => _levelData;
        public LanguageModel LanguageModel => _languageModel;

        [Space(20f)]
        [SerializeField] private TextAsset _jsonToImport;

#if UNITY_EDITOR

        [Button("Import")]
        public void ImportJson()
        {
            if (_jsonToImport == null) return;

            LevelData data;
            try
            {
                data = JsonConvert.DeserializeObject<LevelData>(_jsonToImport.text);
            }
            catch
            {
                data = null;
            }

            if (_jsonToImport == null) return;

            _levelData = data;

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
        }
    }

#endif
}