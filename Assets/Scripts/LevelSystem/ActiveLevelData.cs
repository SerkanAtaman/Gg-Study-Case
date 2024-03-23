using GG.Language;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace GG.LevelSystem
{
    [CreateAssetMenu(menuName = "GG/LevelSystem/ActiveLevelData")]
    public class ActiveLevelData : ScriptableObject
    {
        [SerializeField] private LevelProgress _levelProgress;

        [SerializeField][ReadOnly] private LevelDataHolder _activeDataHolder;
        [SerializeField][ReadOnly] private LanguageModel _languageModel;

        [SerializeField][ReadOnly] private List<string> _submittedWords;

        [SerializeField][ReadOnly] private int _currentScore;
        [SerializeField][ReadOnly] private bool _isHighestScore = false;

        public bool IsHighestScore => _isHighestScore;

        public LevelDataHolder ActiveDataHolder => _activeDataHolder;
        public LanguageModel LanguageModel => _languageModel;
        public List<string> SubmittedWords => _submittedWords;
        public int CurrentScore => _currentScore;

        private void OnEnable()
        {
            Reset();
        }

        public void Reset()
        {
            _isHighestScore = false;
            _activeDataHolder = null;
            _submittedWords = new();
            _currentScore = 0;
        }

        public void Refresh(LevelDataHolder dataHolder)
        {
            _languageModel = dataHolder.LanguageModel;
            _activeDataHolder = dataHolder;
            _submittedWords.Clear();
            _currentScore = 0;
            _isHighestScore = false;
        }

        public void ReceiveSubmittedWord(string word)
        {
            _submittedWords.Add(word);
        }

        public void AddScore(int amount)
        {
            _currentScore += amount;

            var previousScore = _levelProgress.GetLevelScore(_activeDataHolder.LevelID);

            _isHighestScore = _currentScore > previousScore;
        }
    }
}