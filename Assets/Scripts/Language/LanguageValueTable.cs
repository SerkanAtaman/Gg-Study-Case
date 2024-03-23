using UnityEngine;
using System.Globalization;

namespace GG.Language
{
    [CreateAssetMenu(menuName = "GG/Language/LanguageValueTable")]
    public class LanguageValueTable : ScriptableObject
    {
        [SerializeField] private LetterValue[] _letterValues;

        [HideInInspector] public string LanguageCode;

        public int GetLetterValue(char letter)
        {
            string letterStr = letter.ToString().ToLower(new CultureInfo(LanguageCode));
            int result = 0;

            foreach (LetterValue value in _letterValues)
            {
                if (value.ContainsLetter(letterStr))
                {
                    result = value.Value;
                    break;
                }
            }

            return result;
        }
    }
}