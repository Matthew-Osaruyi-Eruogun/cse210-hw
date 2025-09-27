/*
 Name: Matthew Osaruyi Eruogun
 Course: CSE210
 Added Features: 
-Multiple scriptures which appears randomly each session,
-hides words not yet hidden to make way for memorization,
-tracks user's progress and the remaining words to be memorized, 
-Added descriptions of the codes. 
*/

using System;
using System.Collections.Generic;

class Reference
{
    private string book;
    private int startChapter;
    private int startVerse;
    private int endVerse;

    public Reference(string book, int chapter, int verse)
    {
        this.book = book;
        this.startChapter = chapter;
        this.startVerse = verse;
        this.endVerse = verse;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        this.book = book;
        this.startChapter = chapter;
        this.startVerse = startVerse;
        this.endVerse = endVerse;
    }

    public string GetDisplayText()
    {
        if (startVerse == endVerse)
            return $"{book} {startChapter}:{startVerse}";
        else
            return $"{book} {startChapter}:{startVerse}-{endVerse}";
    }
}

class Word
{
    private string text;
    private bool hidden;

    public Word(string text)
    {
        this.text = text;
        hidden = false;
    }

    public void Hide()
    {
        hidden = true;
    }

    public bool IsHidden()
    {
        return hidden;
    }

    public string GetDisplayText()
    {
        if (hidden)
            return new string('_', text.Length);
        else
            return text;
    }
}

class Scripture
{
    private Reference reference;
    private List<Word> words;

    public Scripture(Reference reference, string scriptureText)
    {
        this.reference = reference;
        words = new List<Word>();
        string[] splitWords = scriptureText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string w in splitWords)
        {
            words.Add(new Word(w));
        }
    }

    // Hide random words that are not yet hidden
    public void HideRandomWords(int count = 3)
    {
        Random random = new Random();
        List<int> notHiddenIndices = new List<int>();
        for (int i = 0; i < words.Count; i++)
        {
            if (!words[i].IsHidden())
                notHiddenIndices.Add(i);
        }

        if (notHiddenIndices.Count == 0) return;

        int hides = Math.Min(count, notHiddenIndices.Count);

        for (int i = 0; i < hides; i++)
        {
            int randIndex = random.Next(notHiddenIndices.Count);
            words[notHiddenIndices[randIndex]].Hide();
            notHiddenIndices.RemoveAt(randIndex);
        }
    }

    public bool AllWordsHidden()
    {
        foreach (var word in words)
        {
            if (!word.IsHidden())
                return false;
        }
        return true;
    }

    public int CountHiddenWords()
    {
        int count = 0;
        foreach (var word in words)
        {
            if (word.IsHidden()) count++;
        }
        return count;
    }

    public string GetDisplayText()
    {
        string wordsText = string.Join(' ', words.ConvertAll(w => w.GetDisplayText()));
        return $"{reference.GetDisplayText()} - {wordsText}";
    }
}

class Program
{
    // Scripture library: list tuples of Reference and scripture text
    static List<(Reference reference, string text)> scriptureLibrary = new List<(Reference, string)>
    {
        (new Reference("John", 3, 16),
         "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life."),

        (new Reference("Proverbs", 3, 5, 6),
         "Trust in the Lord with all your heart and lean not on your own understanding; in all your ways submit to him, and he will make your paths straight."),

        (new Reference("Psalm", 23, 1),
         "The Lord is my shepherd, I lack nothing.")
    };

    static void Main(string[] args)
    {
        // Select random scripture from library
        Random random = new Random();
        var chosen = scriptureLibrary[random.Next(scriptureLibrary.Count)];

        Scripture scripture = new Scripture(chosen.reference, chosen.text);

        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine($"\nWords hidden: {scripture.CountHiddenWords()} out of {chosen.text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length}");

            if (scripture.AllWordsHidden())
            {
                Console.WriteLine("\nAll words are hidden. Memorization complete!");
                break;
            }

            Console.WriteLine("\nPress Enter to hide more words or type 'quit' to exit:");
            string input = Console.ReadLine();
            if (input.Trim().ToLower() == "quit")
                break;

            scripture.HideRandomWords();
        }
    }
}

