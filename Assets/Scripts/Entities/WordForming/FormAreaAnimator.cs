using DG.Tweening;
using UnityEngine;

namespace GG.Entities.WordForming
{
    public class FormAreaAnimator
    {
        private WordFormingArea _formingArea;

        public FormAreaAnimator(WordFormingArea formingArea)
        {
            _formingArea = formingArea;
        }

        public void PlayClearAreaAnimation()
        {
            int count = _formingArea.LetterHolders.Length;

            for(int i = 0; i < count; i++)
            {
                var holder = _formingArea.LetterHolders[i];

                if (!holder.IsOccupied) continue;

                var tile = holder.CurrentTile;
                holder.ReleaseTile();

                var tileEndPos = tile.transform.position + new Vector3(150, 50, 0);
                var tween = tile.transform.DOMove(tileEndPos, 0.25f).SetEase(Ease.InOutQuad);
                tween.SetDelay(i * 0.1f);
                tween.onComplete += () =>
                {
                    tile.PushBackToPool();
                };

                tween.Play();
            }
        }
    }
}