namespace GG.Language
{
    [System.Serializable]
    public class LetterValue
    {
        public int Value;
        public string[] Letters;

        public bool ContainsLetter(string letter)
        {
            bool result = false;

            foreach (string l in Letters)
            {
                if (letter == l)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}