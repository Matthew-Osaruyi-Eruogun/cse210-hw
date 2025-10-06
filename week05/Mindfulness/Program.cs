/*
Author: Matthew Osaruyi Eruogun
Course: CSE 210
Added Features: - Gratitude activity, 
                - activity log, unique session prompt/question usage and 
                - improved breathing animation

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Activity
{
    protected string _name;
    protected string _description;
    protected int _duration;

    public Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void DisplayStartingMessage()
    {
        Console.WriteLine($"\nStarting {_name} Activity");
        Console.WriteLine(_description);
        Console.Write("Enter duration in seconds: ");
        while (!int.TryParse(Console.ReadLine(), out _duration) || _duration <= 0)
        {
            Console.Write("Please enter a positive integer for duration: ");
        }
        Console.WriteLine("Get ready...");
        ShowSpinner(3);
    }

    public void DisplayEndingMessage()
    {
        Console.WriteLine("\nWell done!");
        ShowSpinner(3);
        Console.WriteLine($"You have completed the {_name} Activity for {_duration} seconds.");
        ShowSpinner(3);
    }

    public void ShowSpinner(int seconds)
    {
        string[] spinnerChars = { "|", "/", "-", "\\" };
        int i = 0;
        int delay = 250;
        int total = (seconds * 1000) / delay;
        for (int count = 0; count < total; count++)
        {
            Console.Write(spinnerChars[i]);
            Thread.Sleep(delay);
            Console.Write("\b");
            i = (i + 1) % spinnerChars.Length;
        }
    }

    public void ShowCountDown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i + " ");
            Thread.Sleep(1000);
            Console.Write("\b\b");
        }
        Console.WriteLine();
    }
}

class BreathingActivity : Activity
{
    public BreathingActivity() : base("Breathing", "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
    { }

    private void BreathingAnimation(string message, int seconds, bool expanding)
    {
        Console.WriteLine();
        for (int i = 1; i <= seconds; i++)
        {
            int count = expanding ? i : (seconds - i + 1);
            Console.Write("\r" + message.PadRight(14) + new string('*', count));
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }

    public void Run()
    {
        DisplayStartingMessage();
        int timePassed = 0;
        while (timePassed < _duration)
        {
            int breathIn = Math.Min(4, _duration - timePassed);
            Console.Write("\nBreathe in...");
            BreathingAnimation("Inhale", breathIn, true);
            timePassed += breathIn;
            if (timePassed >= _duration) break;

            int breathOut = Math.Min(6, _duration - timePassed);
            Console.Write("\nBreathe out...");
            BreathingAnimation("Exhale", breathOut, false);
            timePassed += breathOut;
        }
        DisplayEndingMessage();
    }
}

class ListingActivity : Activity
{
    private int _count;
    private List<string> _sessionPrompts;
    private static readonly List<string> AllPrompts = new()
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };
    public ListingActivity() : base("Listing", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    {
        _sessionPrompts = AllPrompts.OrderBy(x => Guid.NewGuid()).ToList(); // Shuffle for session
    }

    private string GetUniquePrompt()
    {
        if (_sessionPrompts.Count == 0)
            _sessionPrompts = AllPrompts.OrderBy(x => Guid.NewGuid()).ToList();
        string prompt = _sessionPrompts[0];
        _sessionPrompts.RemoveAt(0);
        return prompt;
    }

    private List<string> GetListFromUser()
    {
        List<string> responses = new List<string>();
        DateTime endTime = DateTime.Now.AddSeconds(_duration);
        Console.WriteLine("Start listing items (press Enter after each):");
        while (DateTime.Now < endTime)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                responses.Add(input);
        }
        return responses;
    }

    public void Run()
    {
        DisplayStartingMessage();
        string prompt = GetUniquePrompt();
        Console.WriteLine("\nList as many responses as you can to the following prompt:");
        Console.WriteLine($"--- {prompt} ---");
        Console.WriteLine("You have a few seconds to prepare.");
        ShowCountDown(5);

        var responses = GetListFromUser();
        _count = responses.Count;
        Console.WriteLine($"\nYou listed {_count} items.");
        DisplayEndingMessage();
    }
}

class ReflectingActivity : Activity
{
    private List<string> _sessionPrompts;
    private List<string> _sessionQuestions;
    private static readonly List<string> AllPrompts = new()
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };
    private static readonly List<string> AllQuestions = new()
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };
    public ReflectingActivity() : base("Reflecting", "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.")
    {
        _sessionPrompts = AllPrompts.OrderBy(x => Guid.NewGuid()).ToList();
        _sessionQuestions = AllQuestions.OrderBy(x => Guid.NewGuid()).ToList();
    }

    private string GetUniquePrompt()
    {
        if (_sessionPrompts.Count == 0)
            _sessionPrompts = AllPrompts.OrderBy(x => Guid.NewGuid()).ToList();
        string prompt = _sessionPrompts[0];
        _sessionPrompts.RemoveAt(0);
        return prompt;
    }
    private string GetUniqueQuestion()
    {
        if (_sessionQuestions.Count == 0)
            _sessionQuestions = AllQuestions.OrderBy(x => Guid.NewGuid()).ToList();
        string q = _sessionQuestions[0];
        _sessionQuestions.RemoveAt(0);
        return q;
    }

    public void DisplayPrompt()
    {
        string prompt = GetUniquePrompt();
        Console.WriteLine($"\n{prompt}");
    }

    public void DisplayQuestions()
    {
        int timePassed = 0;
        while (timePassed + 5 <= _duration)
        {
            string question = GetUniqueQuestion();
            Console.Write($"> {question} ");
            ShowSpinner(5);
            timePassed += 5;
            Console.WriteLine();
        }
    }

    public void Run()
    {
        DisplayStartingMessage();
        DisplayPrompt();
        DisplayQuestions();
        DisplayEndingMessage();
    }
}

// New: GratitudeActivity (Sample creative addition)
class GratitudeActivity : Activity
{
    public GratitudeActivity() : base("Gratitude", "This activity lets you write down things, moments, or people you are grateful for. This supports a positive mindset and wellbeing.") { }

    public void Run()
    {
        DisplayStartingMessage();
        Console.WriteLine("\nList things, people, or moments you are grateful for:");
        List<string> responses = new List<string>();
        DateTime endTime = DateTime.Now.AddSeconds(_duration);
        while (DateTime.Now < endTime)
        {
            Console.Write("Gratitude: ");
            string response = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(response))
                responses.Add(response);
        }
        Console.WriteLine($"\nAwesome! You listed {responses.Count} things you are grateful for.");
        Console.WriteLine();
        DisplayEndingMessage();
    }
}

class Program
{
    // Log for session
    static Dictionary<string, int> activityLog = new()
    {
        {"Breathing", 0},
        {"Reflecting", 0},
        {"Listing", 0},
        {"Gratitude", 0}
    };

    static void Main()
    {
        bool exitRequested = false;
        while (!exitRequested)
        {
            Console.Clear();
            Console.WriteLine("Mindfulness App");
            Console.WriteLine("Select an activity:");
            Console.WriteLine("1) Breathing Activity");
            Console.WriteLine("2) Reflecting Activity");
            Console.WriteLine("3) Listing Activity");
            Console.WriteLine("4) Gratitude Activity (New!)");
            Console.WriteLine("5) Show Session Log and Quit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    activityLog["Breathing"]++;
                    new BreathingActivity().Run();
                    break;
                case "2":
                    activityLog["Reflecting"]++;
                    new ReflectingActivity().Run();
                    break;
                case "3":
                    activityLog["Listing"]++;
                    new ListingActivity().Run();
                    break;
                case "4":
                    activityLog["Gratitude"]++;
                    new GratitudeActivity().Run();
                    break;
                case "5":
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
        // Show log
        Console.WriteLine("\nSession Activity Log:");
        foreach (var pair in activityLog)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value} time(s)");
        }
        Console.WriteLine("\nThank you for using the Mindfulness App. Goodbye For Now!!!");
    }
}
