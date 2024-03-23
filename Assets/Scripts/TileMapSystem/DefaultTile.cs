using Gameframe.ServiceProvider;
using GG.CommandSystem;
using GG.Entities.WordForming;
using GG.LevelSystem;
using TMPro;
using UnityEngine;

namespace GG.TileMapSystem
{
    public class DefaultTile : BaseTile
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMeshPro _characterTmp;

        public override void Init(LevelTile levelTile)
        {
            LevelTile = levelTile;

            _characterTmp.text = levelTile.character;

            transform.SetLocalPositionAndRotation(LevelTile.position.ToVector3(), Quaternion.identity);
        }

        public override void SetInteractable(bool interactable, bool setColor = true)
        {
            base.SetInteractable(interactable);

            if(setColor) _spriteRenderer.color = interactable ? Color.white : Color.gray;
        }

        public override void Interact()
        {
            base.Interact();

            if (!IsInteractable) return;

            var generator = ServiceProvider.Current.GetService(typeof(TileMapGenerator)) as TileMapGenerator;
            var tileMap = generator.CurrentTileMap;
            var formArea = ServiceProvider.Current.GetService(typeof(WordFormingArea)) as WordFormingArea;

            var command = new SelectTileCommand(this, formArea, tileMap);
            CommandCenter.ExecuteCommand(command);
        }
    }
}