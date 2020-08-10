using System;

public enum HardLevel
{
    LittleEasy,
    Easy,
    LittleMedium,
    Medium,
    LittleHard,
    Hard,
    VeryHard,
    SuperHard,
    ImpossibleHard
}

public class Glossary
{
    private const int minWordLength = 5;
    public int MaxWordLength
    {
        get
        {
            return 24;
        }
    }

    private readonly string[] words;
    private readonly Random random;
    private int wordLength;

    public Glossary(string glossary)
    {
        words = glossary.Split('\n');
        random = new Random();
    }

    public void SetHardLevel(HardLevel hardLevel)
    {       
        int minLength = (int)hardLevel + minWordLength;
        int maxAddValue = MaxWordLength - minLength;
        wordLength = (int)hardLevel + random.Next((int)hardLevel, MaxWordLength - maxAddValue + 3);
    }    

    public string GetWord()
    {
        if (wordLength == 0 || wordLength > MaxWordLength || wordLength < minWordLength)
            return GetWordWithLength(minWordLength);

        return GetWordWithLength(wordLength);
    }

    private string GetWordWithLength(int wordLength)
    {
        string word = "";

        while (word.Length != wordLength + 1)
            word = words[random.Next(minWordLength, words.Length)];

        return word;
    }
}
