using GG.Entities.WordForming;
using GG.LevelSystem;
using TMPro;
using UnityEngine;

namespace GG.UserInterface
{
    public class WordScoreDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _wordScoreTmp;
        [SerializeField] private OnWordFormAreaUpdated _onWordFormAreaUpdated;
        [SerializeField] private ActiveLevelData _activeLevelData;

        private void OnEnable()
        {
            _wordScoreTmp.text = "WORD SCORE: 0";

            _onWordFormAreaUpdated.RegisterListener<WordFormingArea>(SetWordScore);
        }

        private void OnDisable()
        {
            _onWordFormAreaUpdated.RemoveListener<WordFormingArea>(SetWordScore);
        }

        private void SetWordScore(WordFormingArea formingArea)
        {
            if (!formingArea.IsCurrentWordValid())
            {
                _wordScoreTmp.text = "WORD SCORE: 0";
                return;
            }

            var wordScore = formingArea.CalculateCurrentWordScore(_activeLevelData.LanguageModel.ValueTable);

            _wordScoreTmp.text = "WORD SCORE: " + wordScore;
        }
    }
}