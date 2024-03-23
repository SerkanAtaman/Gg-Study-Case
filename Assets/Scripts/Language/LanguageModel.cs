using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GG.Language
{
    [CreateAssetMenu(menuName = "GG/Language/LanguageModel")]
    public class LanguageModel : ScriptableObject
    {
        public string LanguageCode = "en-US";

        [SerializeField] private TextAsset _dictionary;
        [SerializeField] private LanguageValueTable _valueTable;

        public LanguageValueTable ValueTable => _valueTable;

        private List<string> _words;

        private int _totalWordCount;

        private void OnEnable()
        {
            if (_dictionary == null) return;
            var words = _dictionary.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            _totalWordCount = words.Length;
            _words = words.ToList();

            _valueTable.LanguageCode = LanguageCode;
        }

        public bool WordExist(string word)
        {
            return _words.Contains(word);
        }

        public bool CanLettersFormAnyWord(List<char> letters, out string word)
        {
            bool result = false;
            word = null;

            for(int i = 0; i < _totalWordCount; i++)
            {
                var tempWord = _words[i];
                if(CanLettersFormWord(letters, tempWord))
                {
                    result = true;
                    word = tempWord;
                    break;
                }
            }

            return result;
        }

        public bool CanLettersFormWord(List<char> letters, string targetWord)
        {
            int targetMatchedLetters = targetWord.Length;
            int matchedLetters = 0;

            var tempLetters = new List<char>();
            foreach(char letter in letters)
            {
                tempLetters.Add(letter);
            }

            for(int i = 0; i < targetMatchedLetters; i++)
            {
                if (tempLetters.Contains(targetWord[i]))
                {
                    matchedLetters++;
                    tempLetters.Remove(targetWord[i]);
                }
            }

            return matchedLetters == targetMatchedLetters;
        }
    }
}