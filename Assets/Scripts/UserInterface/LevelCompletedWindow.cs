using Gameframe.ServiceProvider;
using GG.Entities.ParticleSystem;
using GG.LevelSystem;
using SeroJob.UiSystem;
using System.Threading.Tasks;
using UnityEngine;

namespace GG.UserInterface
{
    public class LevelCompletedWindow : UIWindow
    {
        [SerializeField] private UICommander _openLevelEndCommand;
        [SerializeField] private UICommander _openLevelSelectionCommand;
        [SerializeField] private OnLevelCompleted _onLevelCompleted;

        [SerializeField] private GameObject _completedText;
        [SerializeField] private GameObject _highScoreText;

        private ActiveLevelData _activeLevelData;

        private void Start()
        {
            _onLevelCompleted.RegisterListener<ActiveLevelData>(PlayLevelEndSequence);
        }

        private void OnDestroy()
        {
            _onLevelCompleted.RemoveListener<ActiveLevelData>(PlayLevelEndSequence);
        }

        protected override void WindowOpenStarted()
        {
            _completedText.SetActive(!_activeLevelData.IsHighestScore);
            _highScoreText.SetActive(_activeLevelData.IsHighestScore);

            base.WindowOpenStarted();
        }

        private async void PlayLevelEndSequence(ActiveLevelData activeLevelData)
        {
            _activeLevelData = activeLevelData;

            await Task.Delay(100);

            var particleSpawner = ServiceProvider.Current.GetService(typeof(ParticleSpawner)) as ParticleSpawner;

            if (_activeLevelData.IsHighestScore)
                particleSpawner.PlayParticle(1, OnParticlesPlayed);
            else
                particleSpawner.PlayParticle(0, OnParticlesPlayed);
        }

        private void OnParticlesPlayed()
        {
            _openLevelEndCommand.GiveCommand();
        }

        public void ButtonContinue()
        {
            _openLevelSelectionCommand.GiveCommand();
        }
    }
}