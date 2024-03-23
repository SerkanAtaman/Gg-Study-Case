using DG.Tweening;
using GG.LevelSystem;
using GG.ObjectInteracting;
using GG.ObjectPooling;
using System.Collections.Generic;
using UnityEngine;

namespace GG.TileMapSystem
{
    public abstract class BaseTile : MonoBehaviour, IInteractable, IPoolItem<BaseTile>
    {
        public LevelTile LevelTile { get; protected set; }
        public List<BaseTile> ParentTiles { get; protected set; }

        public bool IsInteractable { get; protected set; }

        private ObjectPool<BaseTile> _dependentPool;

        private Tween _activeTween;

        public BaseTile()
        {
            IsInteractable = false;
            ParentTiles = new();
            _activeTween = null;
        }

        public abstract void Init(LevelTile levelTile);

        public virtual void SetInteractable(bool interactable, bool setColor = true)
        {
            IsInteractable = interactable;
        }

        public virtual void Interact()
        {
            
        }

        public void OnPoolInitialized(ObjectPool<BaseTile> pool)
        {
            _dependentPool = pool;
        }

        public void PushBackToPool()
        {
            _activeTween?.Kill(false);
            _activeTween = null;
            _dependentPool.PushItem(gameObject);
        }

        public void DoTween(Tween tween)
        {
            _activeTween?.Kill(false);

            _activeTween = tween;

            _activeTween.onComplete += () => _activeTween = null;
            
            _activeTween.Play();
        }
    }
}