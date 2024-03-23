using System.Collections.Generic;
using UnityEngine;

namespace GG.LevelSystem
{
    [CreateAssetMenu(menuName = "GG/LevelSystem/LevelProgress")]
    public class LevelProgress : ScriptableObject
    {
        [SerializeField] private List<SingleLevelProgress> _progress;

        public int LastPlayedLevelID
        {
            get
            {
                if (_progress.Count == 0) return 1;

                return _progress[^1].LevelID;
            }
        }

        private void OnEnable()
        {
            // TODO LOAD
            if(_progress == null) _progress = new List<SingleLevelProgress>();
        }

        public int GetLevelScore(int levelID)
        {
            var result = -1;

            foreach (var progress in _progress)
            {
                if(progress.LevelID == levelID)
                {
                    result = progress.LevelScore;
                    break;
                }
            }

            return result;
        }

        public bool IsLevelPlayed(int levelID)
        {
            var result = false;
            foreach (var progress in _progress)
            {
                if(progress.LevelID == levelID)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public void SaveLevelProgress(ActiveLevelData activeLevelData)
        {
            var levelId = activeLevelData.ActiveDataHolder.LevelID;

            bool isNewLevel = true;

            foreach (var progress in _progress)
            {
                if(progress.LevelID == levelId)
                {
                    isNewLevel = false;

                    if(activeLevelData.CurrentScore > progress.LevelScore)
                        progress.LevelScore = activeLevelData.CurrentScore;

                    break;
                }
            }

            if (isNewLevel)
            {
                var progress = new SingleLevelProgress()
                {
                    LevelID = activeLevelData.ActiveDataHolder.LevelID,
                    LevelScore = activeLevelData.CurrentScore
                };
                _progress.Add(progress);
            }

            // TODO SAVE
        }
    }

    [System.Serializable]
    public class SingleLevelProgress
    {
        public int LevelID;
        public int LevelScore;
    }
}