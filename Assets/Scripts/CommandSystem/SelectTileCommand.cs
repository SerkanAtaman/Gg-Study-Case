using GG.Entities.WordForming;
using GG.TileMapSystem;

namespace GG.CommandSystem
{
    public class SelectTileCommand : Command
    {
        private BaseTile _selectedTile;
        private WordFormingArea _wordFormingArea;
        private TileMap _tileMap;

        public SelectTileCommand(BaseTile selectedTile, WordFormingArea wordFormingArea, TileMap tileMap)
        {
            _selectedTile = selectedTile;
            _wordFormingArea = wordFormingArea;
            _tileMap = tileMap;
        }

        internal override void Execute()
        {
            _selectedTile.SetInteractable(false, false);
            _tileMap.RemoveTile(_selectedTile);
            _wordFormingArea.ReceiveTile(_selectedTile);
        }

        internal override void Undo()
        {
            _selectedTile.SetInteractable(true);
            _tileMap.ReceiveTile(_selectedTile);
            _wordFormingArea.RemoveTile(_selectedTile);
        }

        public override bool IsExecutable()
        {
            return _wordFormingArea.CanHoldMoreLetter();
        }

        public override void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                // Dispose managed resources.
                _selectedTile = null;
                _wordFormingArea = null;
                _tileMap = null;
            }

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.

            Disposed = true;
        }
    }
}