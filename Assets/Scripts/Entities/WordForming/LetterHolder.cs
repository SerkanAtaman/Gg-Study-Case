using GG.TileMapSystem;
using UnityEngine;

namespace GG.Entities.WordForming
{
    public class LetterHolder : MonoBehaviour
    {
        public BaseTile CurrentTile { get; private set; }

        public bool IsOccupied => CurrentTile != null;

        private void OnEnable()
        {
            CurrentTile = null;
        }

        private void OnDisable()
        {
            CurrentTile = null;
        }

        public void HoldTile(BaseTile baseTile)
        {
            CurrentTile = baseTile;
        }

        public void ReleaseTile()
        {
            CurrentTile = null;
        }
    }
}