
/*Name: Matthew Osaruyi Eruogun
//Course: CSE210
- Added a 'mood' field to each journal entry to save additional information on how the user is feeling.
- Changed saving / loading logic to use JSON format for better structure, human readability, and compatibility with external tools.
- Added a simple reminder feature that checks if the last journal entry was entered today.
- If not, reminds the user to keep journaling regularly.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Entry class represents a journal entry with prompt, response, date, and mood
class Entry
{
    private string _prompt;
    private string _response;
    private string _date;
    private string _mood; // New field to save mood information

    public Entry(string prompt, string response, string date, string mood)
    {
        _prompt = prompt;
        _response = response;
        _date = date;
        _mood = mood;
    }

    // Properties for JSON serialization (public getters and setters)
    public string Prompt { get => _prompt; set => _prompt = value; }
    public string Response { get => _response; set => _response = value; }
    public string Date { get => _date; set => _date = value; }
    public string Mood { get => _mood; set => _mood = value; }

    // Override ToString to display full info
    public override string ToString()
    {
        return $"Date: {_date}\nPrompt: {_prompt}\nMood: {_mood}\nResponse: {_response}\n";
    }
}

// Journal class manages a list of entries and supports adding, displaying, saving, and loading
class Journal
{
    private List<Entry> _entries;

    public Journal()
    {
        _entries = new List<Entry>();
    }

    public void AddEntry(Entry entry)
    {
        _entries.Add(entry);
    }

    public void DisplayEntries()
    {
        if (_entries.Count == 0)
        {
            Console.WriteLine("The journal is empty.");
            return;
        }
        foreach (Entry entry in _entries)
        {
            Console.WriteLine(entry);
        }
    }

    // Save journal as JSON to file for better structure and compatibility
    public void SaveToFile(string filename)
    {
        string jsonString = JsonSerializer.Serialize(_entries, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filename, jsonString);
        Console.WriteLine($"Journal saved to {filename} in JSON format.");
    }

    // Load journal from JSON file, replacing current entries
    public void LoadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.");
            return;
        }

        try
        {
            string jsonString = File.ReadAllText(filename);
            _entries = JsonSerializer.Deserialize<List<Entry>>(jsonString);
            Console.WriteLine($"Journal loaded from {filename}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading journal: {ex.Message}");
        }
    }

    // Returns the Date string of the last entry, or null if no entries
    public string GetLastEntryDate()
    {
        if (_entries.Count == 0)
            return null;
        return _entries[_entries.Count - 1].Date;
    }
}

class Program
{
    static void Main(string[] args)
    {

        Journal journal = new Journal();
        string[] prompts = new string[]
        {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did you see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?"
        };

        Random random = new Random();

        while (true)
        {
            // Check for journaling reminder
            string lastDate = journal.GetLastEntryDate();
            string today = DateTime.Now.ToShortDateString();
            if (lastDate == null || lastDate != today)
            {
                Console.WriteLine("Reminder: You have not made an entry today. Try to write something!");
            }

            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Write");
            Console.WriteLine("2. Display");
            Console.WriteLine("3. Save");
            Console.WriteLine("4. Load");
            Console.WriteLine("5. Quit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    int index = random.Next(prompts.Length);
                    string prompt = prompts[index];
                    Console.WriteLine($"\nPrompt: {prompt}");
                    Console.Write("Your response: ");
                    string response = Console.ReadLine();

                    Console.Write("How are you feeling today? (Enter mood): ");
                    string mood = Console.ReadLine();

                    string date = DateTime.Now.ToShortDateString();
                    Entry entry = new Entry(prompt, response, date, mood);
                    journal.AddEntry(entry);
                    Console.WriteLine("Entry added.");
                    break;

                case "2":
                    Console.WriteLine("\nJournal entries:");
                    journal.DisplayEntries();
                    break;

                case "3":
                    Console.Write("Enter filename to save (with .json extension recommended): ");
                    string saveFilename = Console.ReadLine();
                    journal.SaveToFile(saveFilename);
                    break;

                case "4":
                    Console.Write("Enter filename to load: ");
                    string loadFilename = Console.ReadLine();
                    journal.LoadFromFile(loadFilename);
                    break;

                case "5":
                    Console.WriteLine("Goodbye for now!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please choose between 1 and 5.");
                    break;
            }
        }
    }
}
