using GG.Entities.WordForming;
using GG.Language;

namespace GG
{
    public static class Utils
    {
        public static int CalculateCurrentWordScore(this WordFormingArea formingArea, LanguageValueTable valueTable)
        {
            var word = formingArea.GetCurrentWord();
            int wordLength = word.Length;
            int wordLettersValue = 0;

            for (int i = 0; i < wordLength; i++)
            {
                var letterValue = valueTable.GetLetterValue(word[i]);
                wordLettersValue += letterValue;
            }

            var wordScore = wordLettersValue * wordLength * 10;

            return wordScore;
        }
    }
}