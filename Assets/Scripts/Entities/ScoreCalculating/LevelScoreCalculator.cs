using Gameframe.ServiceProvider;
using GG.Entities.WordForming;
using GG.LevelSystem;
using GG.TileMapSystem;
using UnityEngine;

namespace GG.Entities.ScoreCalculating
{
    public class LevelScoreCalculator : MonoBehaviour
    {
        [SerializeField] private ActiveLevelData _activeLevelData;
        [SerializeField] private OnWordSubmitted _onWordSubmitted;
        [SerializeField] private OnLevelCompleted _onLevelCompleted;

        private void OnEnable()
        {
            _onWordSubmitted.RegisterListener<WordFormingArea, string>(CalculateWordScore);
            _onLevelCompleted.RegisterListener(CheckLettersLeft);
        }

        private void OnDisable()
        {
            _onWordSubmitted.RemoveListener<WordFormingArea, string>(CalculateWordScore);
            _onLevelCompleted.RemoveListener(CheckLettersLeft);
        }

        private void CalculateWordScore(WordFormingArea formingArea, string word)
        {
            var wordScore = formingArea.CalculateCurrentWordScore(_activeLevelData.LanguageModel.ValueTable);

            Debug.Log($"Calculated score for word: {word} is {wordScore}");

            _activeLevelData.AddScore(wordScore);
        }

        private void CheckLettersLeft()
        {
            var mapGenerator = ServiceProvider.Current.GetService(typeof(TileMapGenerator)) as TileMapGenerator;
            var map = mapGenerator.CurrentTileMap;

            var remainingLetterCount = map.Letters.Count;

            _activeLevelData.AddScore(-100 * remainingLetterCount);
        }
    }
}