using System;
using System.Collections.Generic;

namespace FitnessTracker
{
    // ============================================
    // Base Class: Activity
    // Contains shared attributes and defines the core interface (polymorphic methods).
    // ============================================
    public class Activity
    {
        // Encapsulated Member Variables
        private string _date;
        private int _minutes;

    
        protected string _activityType = "Generic Activity";

        // Public Property for Type (Reads the protected field)
        

        // Constructor
        public Activity(string date, int minutes)
        {
            _date = date;
            _minutes = minutes;
        }

        // Public Accessors
        public int Minutes => _minutes;

        // Abstract/Virtual Methods (Polymorphism)
        public virtual double GetDistance()
        {
            return 0; // Default: 0 km
        }

        public virtual double GetSpeed()
        {
            // Speed (kph) = (distance / minutes) * 60
            if (_minutes == 0) return 0;
            return (GetDistance() / _minutes) * 60;
        }

        public virtual double GetPace()
        {
            // Pace (min per km) = minutes / distance
            double distance = GetDistance();
            if (distance == 0) return 0;
            return _minutes / distance;
        }

        // GetSummary Method
        public string GetSummary()
        {
            // Example format: 03 Nov 2022 Running (30 min): Distance 4.8 km, Speed: 9.7 kph, Pace: 6.25 min per km
            double distance = GetDistance();
            double speed = GetSpeed();
            double pace = GetPace();

            return $"{_date} {_activityType} ({_minutes} min): " +
                   $"Distance {distance:F2} km, " +
                   $"Speed: {speed:F2} kph, " +
                   $"Pace: {pace:F2} min per km";
        }
    }

    // ============================================
    // Derived Class: Running
    // ============================================
    public class Running : Activity
    {
        private double _distance; // Stored in kilometers

        public Running(string date, int minutes, double distance)
            : base(date, minutes)
        {
            _distance = distance;
            // Accessing the protected field:
            _activityType = "Running";
        }

        public override double GetDistance()
        {
            return _distance;
        }
    }

    // ============================================
    // Derived Class: StationaryBicycle (Cycling)
    // ============================================
    public class Cycling : Activity
    {
        private double _speed; // Stored in kilometers per hour (kph)

        public Cycling(string date, int minutes, double speed)
            : base(date, minutes)
        {
            _speed = speed;
            // Accessing the protected field:
            _activityType = "Stationary Bicycles";
        }

        public override double GetSpeed()
        {
            return _speed;
        }

        public override double GetDistance()
        {
            return _speed * (Minutes / 60.0);
        }

        public override double GetPace()
        {
            if (_speed == 0) return 0;
            return 60.0 / _speed;
        }
    }

    // ============================================
    // Derived Class: Swimming
    // ============================================
    public class Swimming : Activity
    {
        private int _laps;
        private const double LapLengthMeters = 50.0;

        public Swimming(string date, int minutes, int laps)
            : base(date, minutes)
        {
            _laps = laps;
            // Accessing the protected field:
            _activityType = "Swimming";
        }

        public override double GetDistance()
        {
            return (_laps * LapLengthMeters) / 1000.0;
        }
    }

    // ============================================
    // Program Entry Point
    // ============================================
    class Program
    {
        static void Main(string[] args)
        {
            List<Activity> activities = new List<Activity>();

            activities.Add(new Running("03 Nov 2022", 30, 4.8));
            activities.Add(new Cycling("04 Nov 2022", 45, 20.0));
            activities.Add(new Swimming("05 Nov 2022", 60, 40));

            Console.WriteLine("--- Fitness Activity Summary (Metric/Kilometers) ---\n");

            foreach (Activity activity in activities)
            {
                Console.WriteLine(activity.GetSummary());
            }

            Console.WriteLine("\n--- End of Report ---");
        }
    }
}