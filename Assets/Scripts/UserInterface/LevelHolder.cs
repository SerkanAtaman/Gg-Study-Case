using GG.LevelSystem;
using TMPro;
using UnityEngine;

namespace GG.UserInterface
{
    public class LevelHolder : MonoBehaviour
    {
        [SerializeField] private LevelDataHolder _levelDataHolder;
        [SerializeField] private LevelProgress _levelProgress;
        [SerializeField] private OnLevelSelected _onLevelSelected;

        [SerializeField] private GameObject _playButtonHolder;
        [SerializeField] private GameObject _lockHolder;

        [SerializeField] private TextMeshProUGUI _levelTitleTmp;
        [SerializeField] private TextMeshProUGUI _levelDescpTmp;

        private void OnEnable()
        {
            Init();
        }

        public void ButtonPlay()
        {
            _onLevelSelected.Broadcast(_levelDataHolder);
        }

        public void Init()
        {
            bool isLocked = _levelProgress.LastPlayedLevelID + 1 < _levelDataHolder.LevelID;

            _playButtonHolder.SetActive(!isLocked);
            _lockHolder.SetActive(isLocked);

            _levelTitleTmp.text = $"Level {_levelDataHolder.LevelID} - {_levelDataHolder.LevelData.title}";

            if (isLocked)
            {
                _levelDescpTmp.text = "Locked Level";
            }
            else
            {
                var levelScore = _levelProgress.GetLevelScore(_levelDataHolder.LevelID);
                if(levelScore > 0)
                {
                    _levelDescpTmp.text = $"Hihg Score: {levelScore}";
                }
                else
                {
                    _levelDescpTmp.text = "No Score";
                }
            }
        }
    }
}