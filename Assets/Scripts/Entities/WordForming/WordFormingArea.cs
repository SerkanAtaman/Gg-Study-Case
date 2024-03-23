using DG.Tweening;
using Gameframe.ServiceProvider;
using GG.CommandSystem;
using GG.LevelSystem;
using GG.TileMapSystem;
using System.Globalization;
using UnityEngine;

namespace GG.Entities.WordForming
{
    public class WordFormingArea : MonoBehaviour
    {
        [SerializeField] private Vector3 _distanceFromTileMap = new Vector3(0, 10, 0);

        [SerializeField] private ActiveLevelData _activeLevelData;
        [SerializeField] private OnTileMapCreated _onTileMapCreated;
        [SerializeField] private OnWordFormAreaUpdated _onWordFormAreaUpdated;
        [SerializeField] private OnWordSubmitted _onWordSubmitted;
        [SerializeField] private OnLevelCompleted _onLevelCompleted;

        [SerializeField] private LetterHolder[] _letterHolders;

        public LetterHolder[] LetterHolders => _letterHolders;

        private FormAreaAnimator _formAreaAnimator;

        private void Awake()
        {
            _formAreaAnimator = new FormAreaAnimator(this);

            ServiceCollection.Current.AddSingleton(this);
        }

        private void OnEnable()
        {
            _onTileMapCreated.RegisterListener<TileMap>(SetFormArea);
            _onLevelCompleted.RegisterListener(HideArea);
        }

        private void OnDisable()
        {
            _onTileMapCreated.RemoveListener<TileMap>(SetFormArea);
            _onLevelCompleted.RemoveListener(HideArea);
        }

        private void SetFormArea(TileMap tileMap)
        {
            var targetPos = tileMap.MapRect.GetTopCenterPosition() + _distanceFromTileMap;
            transform.DOMove(targetPos, 0.5f).SetDelay(0.8f);
            ClearFormArea(false);
            _onWordFormAreaUpdated.Broadcast(this);
        }

        private void HideArea()
        {
            transform.DOMove(transform.position + new Vector3(-100, 0, 0), 0.5f);
        }

        private bool AnyLetterHolderAvailable()
        {
            bool result = false;
            foreach(var letterHolder in _letterHolders)
            {
                if (!letterHolder.IsOccupied)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private LetterHolder GetFirstAvailableHolder()
        {
            int count = _letterHolders.Length;
            LetterHolder result = null;

            for(int i = 0; i < count; i++)
            {
                if (!_letterHolders[i].IsOccupied)
                {
                    result= _letterHolders[i];
                    break;
                }
            }

            return result;
        }

        private LetterHolder GetHolderWithBaseTile(BaseTile baseTile)
        {
            LetterHolder result = null;

            foreach(var letterHolder in _letterHolders)
            {
                if (letterHolder.CurrentTile == null) continue;
                if(letterHolder.CurrentTile == baseTile)
                {
                    result = letterHolder;
                    break;
                }
            }

            return result;
        }

        private void ClearFormArea(bool animate)
        {
            if (!animate)
            {
                foreach(var letterHolder in _letterHolders)
                {
                    if (letterHolder.IsOccupied) letterHolder.CurrentTile.PushBackToPool();

                    letterHolder.ReleaseTile();
                }
                return;
            }

            _formAreaAnimator.PlayClearAreaAnimation();
        }

        public string GetCurrentWord()
        {
            var result = "";

            int count = _letterHolders.Length;
            for (int i = 0; i < count; i++)
            {
                if (_letterHolders[i].IsOccupied)
                {
                    result += _letterHolders[i].CurrentTile.LevelTile.character;
                }
            }

            return result.ToLower(new CultureInfo(_activeLevelData.LanguageModel.LanguageCode));
        }

        public bool IsCurrentWordValid()
        {
            var word = GetCurrentWord();

            return _activeLevelData.LanguageModel.WordExist(word) && !_activeLevelData.SubmittedWords.Contains(word);
        }

        public bool CanHoldMoreLetter()
        {
            return AnyLetterHolderAvailable();
        }

        public void ReceiveTile(BaseTile baseTile)
        {
            if (!AnyLetterHolderAvailable())
            {
                Debug.LogWarning("Failed to receive tile to form word because there is no letter holder available");
                return;
            }

            var holder = GetFirstAvailableHolder();
            holder.HoldTile(baseTile);

            var tween = baseTile.transform.DOMove(holder.transform.position, 0.3f).SetEase(Ease.OutBack);
            baseTile.DoTween(tween);

            _onWordFormAreaUpdated.Broadcast(this);
        }

        public void RemoveTile(BaseTile baseTile)
        {
            var holder = GetHolderWithBaseTile(baseTile);
            holder.ReleaseTile();

            var tileMapPos = baseTile.LevelTile.position.ToVector3();

            var tween = baseTile.transform.DOMove(tileMapPos, 0.3f).SetEase(Ease.OutBack);
            baseTile.DoTween(tween);

            _onWordFormAreaUpdated.Broadcast(this);
        }

        public void SubmitCurrentWord()
        {
            CommandCenter.ClearStack();

            var word = GetCurrentWord();

            _activeLevelData.ReceiveSubmittedWord(word);

            _onWordSubmitted.Broadcast(this, GetCurrentWord());

            ClearFormArea(true);

            _onWordFormAreaUpdated.Broadcast(this);
        }
    }
}