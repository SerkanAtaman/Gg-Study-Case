using GG.LevelSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GG.TileMapSystem
{
    public static class TileMapUtilities
    {
        public static Vector3 ToVector3(this TilePosition tilePosition)
        {
            return new Vector3(tilePosition.x, tilePosition.y, tilePosition.z);
        }

        public static TileRect GetTileMapRect(TileMap tileMap)
        {
            var tiles = tileMap.Tiles;
            int tileCount = tiles.Count;
            float minPosZ = int.MaxValue;
            Vector3 bottomLeftPos = new Vector3(float.MaxValue, float.MaxValue, 0f);
            Vector3 topRightPos = new Vector3(float.MinValue, float.MinValue, 0f);

            for (int i = 0; i < tileCount; i++)
            {
                var tile = tiles.Values.ElementAt(i);
                if (tile.LevelTile.position.z <= minPosZ) minPosZ = tile.LevelTile.position.z;
                if (tile.LevelTile.position.x <= bottomLeftPos.x) bottomLeftPos.x = tile.LevelTile.position.x;
                if (tile.LevelTile.position.y <= bottomLeftPos.y) bottomLeftPos.y = tile.LevelTile.position.y;
                if (tile.LevelTile.position.x >= topRightPos.x) topRightPos.x = tile.LevelTile.position.x;
                if (tile.LevelTile.position.y >= topRightPos.y) topRightPos.y = tile.LevelTile.position.y;
            }

            var extend = (topRightPos - bottomLeftPos) * 0.5f;
            var center = bottomLeftPos + extend;
            center.z = minPosZ;

            return new TileRect(center, extend);
        }

        public static void SetTileParents(this TileMap tileMap)
        {
            int count = tileMap.Tiles.Count;

            for (int i = 0; i < count; i++)
            {
                tileMap.Tiles.Values.ElementAt(i).ParentTiles.Clear();
            }

            for (int i = 0; i < count; i++)
            {
                var tile = tileMap.Tiles.Values.ElementAt(i);

                if(tile.LevelTile.children is null) continue;

                foreach(var child in tile.LevelTile.children)
                {
                    var childTile = tileMap.Tiles[child];
                    childTile.ParentTiles.Add(tile);
                }
            }
        }

        public static List<BaseTile> GetInteractableTiles(this TileMap tileMap)
        {
            return tileMap.Tiles.Values.ToList().FindAll(x => x.IsInteractable == true);
        }

        public static List<BaseTile> GetTilesWillBeUnlocked(this TileMap tileMap, BaseTile baseTile)
        {
            List<BaseTile> result = new();

            foreach (var childID in baseTile.LevelTile.children)
            {
                var childTile = tileMap.Tiles[childID];
                result.Add(childTile);
            }

            return result;
        }
    }
}