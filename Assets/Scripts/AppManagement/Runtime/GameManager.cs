using Gameframe.ServiceProvider;
using GG.Entities.WordForming;
using GG.LevelSystem;
using GG.TileMapSystem;
using GG.UserInterface;
using UnityEngine;

namespace GG
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ActiveLevelData _activeLevelData;
        [SerializeField] private LevelProgress _levelProgress;

        [SerializeField] private OnTileMapCreated _onTileMapCreated;
        [SerializeField] private OnWordSubmitted _onWordSubmitted;
        [SerializeField] private OnLevelStarted _onLevelStarted;
        [SerializeField] private OnLevelCompleted _onLevelCompleted;
        [SerializeField] private OnLevelSelected _onLevelSelected;

        private void Awake()
        {
            ServiceCollection.Current.AddSingleton(this);
        }

        private void OnEnable()
        {
            _onWordSubmitted.RegisterListener(CheckLevelEnd);
            _onLevelSelected.RegisterListener<LevelDataHolder>(StartLevel);
        }

        private void OnDisable()
        {
            _onWordSubmitted.RemoveListener(CheckLevelEnd);
            _onLevelSelected.RemoveListener<LevelDataHolder>(StartLevel);
        }

        public void StartLevel(LevelDataHolder levelDataHolder)
        {
            _activeLevelData.Refresh(levelDataHolder);

            var mapGenerator = ServiceProvider.Current.GetService(typeof(TileMapGenerator)) as TileMapGenerator;

            _onTileMapCreated.RegisterListener(OnTileMapCreated);

            mapGenerator.GenerateMap(levelDataHolder);
        }

        private void OnTileMapCreated()
        {
            _onTileMapCreated.RemoveListener(OnTileMapCreated);

            Debug.Log("Level Started! level title: " + _activeLevelData.ActiveDataHolder.LevelData.title);

            _onLevelStarted.Broadcast(_activeLevelData.ActiveDataHolder.LevelData);
        }

        private async void CheckLevelEnd()
        {
            var mapGenerator = ServiceProvider.Current.GetService(typeof(TileMapGenerator)) as TileMapGenerator;
            var map = mapGenerator.CurrentTileMap;
            
            if(map.Letters.Count == 0)
            {
                CompleteLevel();
                return;
            }

            var searcher = new TileSearcher(map, _activeLevelData.LanguageModel);

            var searcherResult = await searcher.CanFormAnyWord();

            if (!searcherResult)
            {
                Debug.Log("Searcher did not found any word to be formed");
                CompleteLevel();
                return;
            }
        }

        private void CompleteLevel()
        {
            Debug.Log("Level Completed! level title: " + _activeLevelData.ActiveDataHolder.LevelData.title);

            _onLevelCompleted.Broadcast(_activeLevelData);

            _levelProgress.SaveLevelProgress(_activeLevelData);
        }
    }
}