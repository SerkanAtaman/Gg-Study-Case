using GG.LevelSystem;
using UnityEngine;
using GG.ObjectPooling;
using System.Collections.Generic;
using Gameframe.ServiceProvider;
using System.Threading.Tasks;

namespace GG.TileMapSystem
{
    public class TileMapGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _tilePref;
        [SerializeField] private OnTileMapCreated _onTileMapCreated;
        [SerializeField] private OnLevelCompleted _onLevelCompleted;

        public TileMap CurrentTileMap { get; private set; }

        private ObjectPool<BaseTile> _tilePool;
        private TileMapAnimator _animator;

        private void Awake()
        {
            CurrentTileMap = null;
            _animator = new TileMapAnimator();
            _tilePool = new ObjectPool<BaseTile>(_tilePref, transform, 40);

            ServiceCollection.Current.AddSingleton(this);
        }

        private void OnEnable()
        {
            _onLevelCompleted.RegisterListener(DestroyMapOnLevelEnd);
        }

        private void OnDisable()
        {
            _onLevelCompleted.RemoveListener(DestroyMapOnLevelEnd);
        }

        public async void GenerateMap(LevelDataHolder levelDataHolder)
        {
            if (CurrentTileMap is not null) await DestroyMap();

            Dictionary<int, BaseTile> tiles = new();

            foreach (var tile in levelDataHolder.LevelData.tiles)
            {
                var baseTile = _tilePool.Pull();
                baseTile.Init(tile);
                baseTile.gameObject.SetActive(true);
                tiles.Add(tile.id, baseTile);
            }

            CurrentTileMap = new TileMap(tiles, levelDataHolder.LanguageModel);
            CurrentTileMap.SetTileParents();

            _onTileMapCreated.Broadcast(CurrentTileMap);

            await _animator.PlayCreationAnim(CurrentTileMap);
        }

        public async Task DestroyMap()
        {
            await _animator.PlayDestortionAnim(CurrentTileMap);
            
            if(CurrentTileMap is null)
            {
                Debug.LogError("Can not destroy tile map because CurrentTileMap is null");
                return;
            }

            foreach(var tile in CurrentTileMap.Tiles.Values)
            {
                _tilePool.PushItem(tile.gameObject);
            }

            CurrentTileMap.Dispose();
            CurrentTileMap = null;
        }

        private void DestroyMapOnLevelEnd()
        {
            DestroyMap();
        }
    }
}