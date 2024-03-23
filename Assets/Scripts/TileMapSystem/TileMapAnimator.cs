using DG.Tweening;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GG.TileMapSystem
{
    public class TileMapAnimator
    {
        public async Task PlayCreationAnim(TileMap tileMap)
        {
            int count = tileMap.Tiles.Count;

            for(int i = 0; i < count; i++)
            {
                var tile = tileMap.Tiles.ElementAt(i).Value;
                var tilePos = tile.transform.position;
                tile.transform.position = new Vector3(0, 100, tilePos.z);
                tile.transform.DOMove(tilePos, 0.5f).SetEase(Ease.InOutQuad).SetDelay(i * 0.05f);
            }

            await Task.Delay(500 + 5 * count);
        }

        public async Task PlayDestortionAnim(TileMap tileMap)
        {
            int count = tileMap.Tiles.Count;

            for (int i = 0; i < count; i++)
            {
                var tile = tileMap.Tiles.ElementAt(i).Value;
                var tilePos = tile.transform.position;
                tile.transform.DOMove(tilePos + new Vector3(0, 100, 0), 0.5f).SetEase(Ease.InOutQuad).SetDelay(i * 0.05f);
            }

            await Task.Delay(500 + 5 * count);
        }
    }
}