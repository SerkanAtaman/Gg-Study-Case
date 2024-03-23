using GG.Language;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace GG.TileMapSystem
{
    public class TileMap : DisposableObject
    {
        public Dictionary<int, BaseTile> Tiles { get; private set; }
        public List<char> Letters { get; private set; }

        public TileRect MapRect { get; private set; }

        private LanguageModel _languageModel;

        public TileMap(Dictionary<int, BaseTile> tiles, LanguageModel languageModel)
        {
            _languageModel = languageModel;

            Tiles = tiles;
            Letters = new();
            foreach (var tile in Tiles.Values)
            {
                var letter = tile.LevelTile.character.ToLower(new CultureInfo(_languageModel.LanguageCode));
                Letters.Add(letter[0]);
            }

            MapRect = TileMapUtilities.GetTileMapRect(this);
            RefreshInteractables();
        }

        public void ReceiveTile(BaseTile baseTile)
        {
            try
            {
                Tiles.Add(baseTile.LevelTile.id, baseTile);
                var letter = baseTile.LevelTile.character.ToLower(new CultureInfo(_languageModel.LanguageCode));
                Letters.Add(letter[0]);
                foreach (var child in baseTile.LevelTile.children)
                {
                    Tiles[child].SetInteractable(false);
                }
            }
            catch (System.Exception exp)
            {
                Debug.LogError("Failed to receive tile " + exp.Message);
            }
        }

        public void RemoveTile(BaseTile baseTile)
        {
            try
            {
                Tiles.Remove(baseTile.LevelTile.id);
                var letter = baseTile.LevelTile.character.ToLower(new CultureInfo(_languageModel.LanguageCode));
                Letters.Remove(letter[0]);

                foreach (var childID in baseTile.LevelTile.children)
                {
                    var childTile = Tiles[childID];
                    childTile.SetInteractable(true);
                    foreach(var parent in childTile.ParentTiles)
                    {
                        if (Tiles.ContainsKey(parent.LevelTile.id))
                        {
                            childTile.SetInteractable(false);
                            break;
                        }
                    }
                }
            }
            catch(System.Exception exp)
            {
                Debug.LogError("Failed to remove tile " + exp.Message);
            }
        }

        public void RefreshInteractables()
        {
            foreach (var tile in Tiles.Values)
            {
                tile.SetInteractable(true);
            }

            foreach (var tile in Tiles.Values)
            {
                foreach(var id in tile.LevelTile.children)
                {
                    Tiles[id].SetInteractable(false);
                }
            }
        }

        public override void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                // Dispose managed resources.
                Tiles.Clear();
                Tiles = null;
                Letters.Clear();
                Letters = null;
                _languageModel = null;
            }

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.

            Disposed = true;
        }
    }
}