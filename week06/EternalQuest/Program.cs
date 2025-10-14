/*
Author: Matthew Osaruyi Eruogun
Course: CSE210
Instructor: Alex Christensen
Added Features: 
                - Gamification: Eternal rank system (leveling), 
                - Tiered point/bonus management and 
                - Enhanced user experience
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{

    // ============================================
    // Base Class: Goal
    // ============================================
    public abstract class Goal
    {
        protected string _shortName;
        protected string _description;
        protected int _points;

        public string Name => _shortName;

        public Goal(string name, string description, int points)
        {
            _shortName = name;
            _description = description;
            _points = points;
        }

        public abstract int RecordEvent();
        public abstract bool IsComplete();

        public virtual string GetDetailsString()
        {
            return $"{_shortName} ({_description})";
        }

        public abstract string GetStringRepresentation();
    }

    // ============================================
    // Derived Class: SimpleGoal
    // ============================================
    public class SimpleGoal : Goal
    {
        private bool _isComplete;

        public SimpleGoal(string name, string description, int points) : base(name, description, points)
        {
            _isComplete = false;
        }

        public SimpleGoal(string name, string description, int points, bool isComplete) : base(name, description, points)
        {
            _isComplete = isComplete;
        }

        public override int RecordEvent()
        {
            if (!_isComplete)
            {
                _isComplete = true;
                return _points;
            }
            return 0;
        }

        public override bool IsComplete()
        {
            return _isComplete;
        }

        public override string GetDetailsString()
        {
            string status = IsComplete() ? "[X]" : "[ ]";
            return $"{status} {base.GetDetailsString()}";
        }

        public override string GetStringRepresentation()
        {
            return $"SimpleGoal|{_shortName}|{_description}|{_points}|{_isComplete}";
        }
    }

    // ============================================
    // Derived Class: EternalGoal
    // ============================================
    public class EternalGoal : Goal
    {
        public EternalGoal(string name, string description, int points) : base(name, description, points)
        {
        }

        public override int RecordEvent()
        {
            return _points;
        }

        public override bool IsComplete()
        {
            return false;
        }

        public override string GetDetailsString()
        {
            return $"[ ] {base.GetDetailsString()}";
        }

        public override string GetStringRepresentation()
        {
            return $"EternalGoal|{_shortName}|{_description}|{_points}";
        }
    }

    // ============================================
    // Derived Class: ChecklistGoal
    // ============================================
    public class ChecklistGoal : Goal
    {
        private int _amountCompleted;
        private int _target;
        private int _bonus;

        public ChecklistGoal(string name, string description, int points, int target, int bonus) : base(name, description, points)
        {
            _amountCompleted = 0;
            _target = target;
            _bonus = bonus;
        }

        public ChecklistGoal(string name, string description, int points, int target, int bonus, int amountCompleted) : base(name, description, points)
        {
            _target = target;
            _bonus = bonus;
            _amountCompleted = amountCompleted;
        }

        public override int RecordEvent()
        {
            if (IsComplete()) return 0;

            _amountCompleted++;
            int pointsGained = _points;

            if (IsComplete())
            {
                pointsGained += _bonus;
            }

            return pointsGained;
        }

        public override bool IsComplete()
        {
            return _amountCompleted >= _target;
        }

        public override string GetDetailsString()
        {
            string status = IsComplete() ? "[X]" : "[ ]";
            string baseDetails = base.GetDetailsString();
            return $"{status} {baseDetails} -- Currently completed {_amountCompleted}/{_target} times";
        }

        public override string GetStringRepresentation()
        {
            return $"ChecklistGoal|{_shortName}|{_description}|{_points}|{_target}|{_bonus}|{_amountCompleted}";
        }
    }

    // ============================================
    // Manager Class: GoalManager
    // ============================================
    public class GoalManager
    {
        private List<Goal> _goals;
        private int _score;

        private string GetEternalRank()
        {
            if (_score < 1000) return "Novice Seeker";
            if (_score < 5000) return "Apprentice Disciple";
            if (_score < 10000) return "Faithful Traveler";
            if (_score < 25000) return "Master Builder";
            return "Eternal Questor";
        }

        public GoalManager()
        {
            _goals = new List<Goal>();
            _score = 0;
        }

        public void Start()
        {
            int choice = 0;
            while (choice != 6)
            {
                DisplayPlayerInfo();
                Console.WriteLine("\nMenu Options:");
                Console.WriteLine("  1. Create New Goal");
                Console.WriteLine("  2. List Goals");
                Console.WriteLine("  3. Save Goals");
                Console.WriteLine("  4. Load Goals");
                Console.WriteLine("  5. Record Event");
                Console.WriteLine("  6. Quit");
                Console.Write("Select a choice from the menu: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1: CreateGoal(); break;
                        case 2: ListGoalDetails(); break;
                        case 3: SaveGoals(); break;
                        case 4: LoadGoals(); break;
                        case 5: RecordEvent(); break;
                        case 6: Console.WriteLine("Goodbye! May your Eternal Quest be successful! 🙏"); break;
                        default: Console.WriteLine("Invalid choice. Please enter a number between 1 and 6."); break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        public void DisplayPlayerInfo()
        {
            Console.WriteLine($"\nYou have {_score} points.");
            Console.WriteLine($"Your Eternal Rank: {GetEternalRank()} ⭐");
        }

        public void ListGoalDetails()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("\nYou have no goals set.");
                return;
            }

            Console.WriteLine("\nThe goals are:");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
            }
        }

        private void ListGoalNames()
        {
            Console.WriteLine("\nThe goals are:");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].Name}");
            }
        }

        public void CreateGoal()
        {
            Console.WriteLine("The types of Goals are:");
            Console.WriteLine("  1. Simple Goal");
            Console.WriteLine("  2. Eternal Goal");
            Console.WriteLine("  3. Checklist Goal");
            Console.Write("Which type of goal would you like to create? ");

            if (!int.TryParse(Console.ReadLine(), out int typeChoice) || typeChoice < 1 || typeChoice > 3)
            {
                Console.WriteLine("Invalid selection. Goal creation cancelled.");
                return;
            }

            Console.Write("What is the name of your goal? ");
            string name = Console.ReadLine();
            Console.Write("What is a short description of it? ");
            string description = Console.ReadLine();
            Console.Write("What is the base amount of points associated with this goal? ");
            if (!int.TryParse(Console.ReadLine(), out int points)) return;

            Goal newGoal = null;
            switch (typeChoice)
            {
                case 1: newGoal = new SimpleGoal(name, description, points); break;
                case 2: newGoal = new EternalGoal(name, description, points); break;
                case 3:
                    Console.Write("How many times does this goal need to be accomplished for a bonus? ");
                    if (!int.TryParse(Console.ReadLine(), out int target)) return;
                    Console.Write("What is the bonus for accomplishing it that many times? ");
                    if (!int.TryParse(Console.ReadLine(), out int bonus)) return;
                    newGoal = new ChecklistGoal(name, description, points, target, bonus);
                    break;
            }

            if (newGoal != null)
            {
                _goals.Add(newGoal);
                Console.WriteLine($"Goal '{name}' created successfully. 🎉");
            }
        }

        public void RecordEvent()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("No goals available to record an event.");
                return;
            }

            ListGoalNames();
            Console.Write("Which goal did you accomplish? ");

            if (!int.TryParse(Console.ReadLine(), out int goalIndex) || goalIndex < 1 || goalIndex > _goals.Count)
            {
                Console.WriteLine("Invalid goal number.");
                return;
            }

            Goal goal = _goals[goalIndex - 1];

            if (goal.IsComplete())
            {
                Console.WriteLine($"You have already completed the goal '{goal.Name}'. No points awarded.");
                return;
            }

            int pointsEarned = goal.RecordEvent();
            _score += pointsEarned;

            Console.WriteLine($"\nCongratulations! You have earned {pointsEarned} points!");
            Console.WriteLine($"You now have {_score} points.");

            if (goal.IsComplete())
            {
                Console.WriteLine($"\n*** You have completed the goal: {goal.Name} ***\n");
            }
        }

        public void SaveGoals()
        {
            Console.Write("What is the filename for the goal file? ");
            string filename = Console.ReadLine();

            try
            {
                using (StreamWriter outputFile = new StreamWriter(filename))
                {
                    outputFile.WriteLine(_score);

                    foreach (Goal goal in _goals)
                    {
                        outputFile.WriteLine(goal.GetStringRepresentation());
                    }
                }
                Console.WriteLine($"Goals saved successfully to {filename} ✅");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving: {ex.Message}");
            }
        }

        // UPDATED: Added robust error handling and validation for reliable loading
        public void LoadGoals()
        {
            Console.Write("What is the filename for the goal file? ");
            string filename = Console.ReadLine();

            if (!File.Exists(filename))
            {
                Console.WriteLine($"File not found: {filename}");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(filename);

                if (lines.Length == 0)
                {
                    Console.WriteLine("File is empty. No goals loaded.");
                    return;
                }

                // Load score from the first line
                if (int.TryParse(lines[0], out int loadedScore))
                {
                    _score = loadedScore;
                }
                else
                {
                    Console.WriteLine("Warning: Could not load score from file. Starting with 0.");
                    _score = 0;
                }

                _goals.Clear(); // Clear existing goals
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] parts = line.Split('|');
                    string goalType = parts[0];

                    Goal loadedGoal = null;

                    // Ensure basic properties are available (GoalType, Name, Description, Points)
                    if (parts.Length < 4 || !int.TryParse(parts[3], out int points))
                    {
                        Console.WriteLine($"Warning: Skipping corrupted goal line {i} (Invalid basic data).");
                        continue;
                    }

                    string name = parts[1];
                    string description = parts[2];

                    switch (goalType)
                    {
                        case "SimpleGoal":
                            if (parts.Length < 5 || !bool.TryParse(parts[4], out bool isComplete))
                            {
                                Console.WriteLine($"Warning: Skipping corrupted SimpleGoal line {i} (Invalid completion status).");
                                continue;
                            }
                            loadedGoal = new SimpleGoal(name, description, points, isComplete);
                            break;

                        case "EternalGoal":
                            loadedGoal = new EternalGoal(name, description, points);
                            break;

                        case "ChecklistGoal":
                            if (parts.Length < 7 ||
                                !int.TryParse(parts[4], out int target) ||
                                !int.TryParse(parts[5], out int bonus) ||
                                !int.TryParse(parts[6], out int amountCompleted))
                            {
                                Console.WriteLine($"Warning: Skipping corrupted ChecklistGoal line {i} (Invalid extended data).");
                                continue;
                            }
                            loadedGoal = new ChecklistGoal(name, description, points, target, bonus, amountCompleted);
                            break;
                    }

                    if (loadedGoal != null)
                    {
                        _goals.Add(loadedGoal);
                    }
                }
                Console.WriteLine($"Goals loaded successfully from {filename} 💾");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"A critical error occurred while loading: {ex.Message}");
            }
        }
    }

    // ============================================
    // Main Program Entry Point
    // ============================================
    class Program
    {
        static void Main(string[] args)
        {
            GoalManager manager = new GoalManager();
            manager.Start();
        }
    }
}