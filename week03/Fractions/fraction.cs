// Fraction.cs
using System;

public class Fraction
{
    private int numerator;
    private int denominator;

    // Constructor with no parameters, initializes to 1/1
    public Fraction()
    {
        numerator = 1;
        denominator = 1;
    }

    // Constructor with one parameter, initializes denominator to 1
    public Fraction(int top)
    {
        numerator = top;
        denominator = 1;
    }

    // Constructor with two parameters, one for numerator and one for denominator
    public Fraction(int top, int bottom)
    {
        numerator = top;
        denominator = bottom;
        if (denominator == 0)
        {
            throw new ArgumentException("Denominator cannot be zero.");
        }
    }

    // Getter and Setter for numerator
    public int GetNumerator()
    {
        return numerator;
    }
    public void SetNumerator(int value)
    {
        numerator = value;
    }

    // Getter and Setter for denominator
    public int GetDenominator()
    {
        return denominator;
    }
    public void SetDenominator(int value)
    {
        if (value == 0)
        {
            throw new ArgumentException("Denominator cannot be zero.");
        }
        denominator = value;
    }

    // Returns the fraction as a string representation like "3/4"
    public string GetFractionString()
    {
        return $"{numerator}/{denominator}";
    }

    // Returns decimal value of fraction as double
    public double GetDecimalValue()
    {
        return (double)numerator / denominator;
    }
}
