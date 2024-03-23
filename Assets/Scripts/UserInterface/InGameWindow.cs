using Gameframe.ServiceProvider;
using GG.CommandSystem;
using GG.Entities.WordForming;
using GG.LevelSystem;
using SeroJob.UiSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GG.UserInterface
{
    public class InGameWindow : UIWindow
    {
        [SerializeField] private OnWordFormAreaUpdated _onWordFormAreaUpdated;
        [SerializeField] private OnLevelCompleted _onLevelCompleted;
        [SerializeField] private UICommander _closeWindowCommand;

        public Button SubmitWordButton;
        public Button UndoButton;

        private void OnEnable()
        {
            CheckUndoButton();

            _onWordFormAreaUpdated.RegisterListener<WordFormingArea>(CheckSubmitButton);
            _onLevelCompleted.RegisterListener(CloseWindow);
        }

        private void OnDisable()
        {
            _onWordFormAreaUpdated.RemoveListener<WordFormingArea>(CheckSubmitButton);
            _onLevelCompleted.RemoveListener(CloseWindow);
        }

        private void CloseWindow()
        {
            _closeWindowCommand.GiveCommand();
        }

        public void ButtonUndo()
        {
            CommandCenter.Undo();
        }

        public void ButtonUndoAll()
        {
            StartCoroutine(UndoAll());
        }

        private IEnumerator UndoAll()
        {
            while (CommandCenter.IsUndoAvailable)
            {
                CommandCenter.Undo();

                yield return new WaitForSeconds(0.1f);
            }
        }

        public void ButtonSubmit()
        {
            var formArea = ServiceProvider.Current.GetService(typeof(WordFormingArea)) as WordFormingArea;

            formArea.SubmitCurrentWord();
        }

        private void CheckSubmitButton(WordFormingArea wordFormingArea)
        {
            SubmitWordButton.interactable = wordFormingArea.IsCurrentWordValid();
            CheckUndoButton();
        }

        private void CheckUndoButton()
        {
            UndoButton.interactable = CommandCenter.IsUndoAvailable;
        }
    }
}