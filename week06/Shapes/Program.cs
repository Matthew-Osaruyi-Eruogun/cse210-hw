using System;
using System.Collections.Generic;

namespace Shapes
{
    // ============================================
    // Base Class: Shape
    // ============================================
    public class Shape
    {
        // Private member variable
        private string _color;

        // Public property (getter and setter)
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        // Constructor
        public Shape(string color)
        {
            _color = color;
        }

        // Virtual method: Allows derived classes to override the area calculation
        public virtual double GetArea()
        {
            return 0; // Default implementation
        }
    }

    // ============================================
    // Derived Class: Square
    // ============================================
    public class Square : Shape
    {
        private double _side;

        // Constructor: Accepts color and side, calls base constructor for color
        public Square(string color, double side) : base(color)
        {
            _side = side;
        }

        // Override GetArea() for a square (side * side)
        public override double GetArea()
        {
            return _side * _side;
        }
    }

    // ============================================
    // Derived Class: Rectangle
    // ============================================
    public class Rectangle : Shape
    {
        private double _length;
        private double _width;

        // Constructor: Accepts color, length, and width, calls base constructor for color
        public Rectangle(string color, double length, double width) : base(color)
        {
            _length = length;
            _width = width;
        }

        // Override GetArea() for a rectangle (length * width)
        public override double GetArea()
        {
            return _length * _width;
        }
    }

    // ============================================
    // Derived Class: Circle
    // ============================================
    public class Circle : Shape
    {
        private double _radius;

        // Constructor: Accepts color and radius, calls base constructor for color
        public Circle(string color, double radius) : base(color)
        {
            _radius = radius;
        }

        // Override GetArea() for a circle (PI * r * r)
        public override double GetArea()
        {
            return Math.PI * _radius * _radius;
        }
    }

    // ============================================
    // Main Program Entry Point
    // ============================================
    class Program
    {
        static void Main(string[] args)
        {
            // --- Testing Individual Square ---
            Console.WriteLine("--- Testing Individual Square ---");
            Square mySquare = new Square("Red", 5);
            Console.WriteLine($"Square Color: {mySquare.Color}");
            Console.WriteLine($"Square Area: {mySquare.GetArea()}");
            Console.WriteLine();

            // --- Building and Iterating through a List<Shape> (Polymorphism) ---
            Console.WriteLine("--- Testing Polymorphic List ---");

            // Create a list to hold shapes (List<Shape>)
            List<Shape> shapes = new List<Shape>();

            // Add instances of derived classes to the List<Shape>
            shapes.Add(new Square("Blue", 4.5));
            shapes.Add(new Rectangle("Green", 10, 5.5));
            shapes.Add(new Circle("Yellow", 3));
            shapes.Add(mySquare); 

            // Iterate through the list. The GetArea() call is correctly resolved
            // to the specific method for Square, Rectangle, or Circle.
            foreach (Shape shape in shapes)
            {
                // :F2 formats the area to 2 decimal places
                Console.WriteLine($"Shape: {shape.GetType().Name}, Color: {shape.Color}, Area: {shape.GetArea():F2}");
            }
        }
    }
}