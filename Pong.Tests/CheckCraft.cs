using System;
using System.Collections.Generic;

public class Suite {
    public Action suite;
    public string description;

    public Suite(Action suite, string description) {
        this.suite = suite ?? throw new ArgumentNullException(nameof(suite));
        this.description = description;
    }
}

public static class Suites {
    private static List<Suite> suites = new List<Suite>();

    public static void Add(Suite suite) {
        suites.Add(suite);
    }

    public static void Run() {
        foreach(var suite in suites) {
            Console.WriteLine(suite.description);
            suite.suite();
        }
    }
}

public static class CheckCraft {
    public static void Describe(string description, Action suite) {
        Suites.Add(new Suite(suite, description));
    }

    public static void It(string description, Action test) {
        try {
            test();
            
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  O ");
            Console.ForegroundColor = oldColor;
            Console.WriteLine(description);
        } catch(Exception e) {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  X ");
            Console.ForegroundColor = oldColor;

            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }

    public static ComparisonObject<T> Expect<T>(T a) where T: IComparable<T> {
        return new ComparisonObject<T>(a);
    }

    public static void RunTests() {
        Suites.Run();
    }
}

public class ComparisonObject<T> where T: IComparable<T> {
    readonly T a;

    public ComparisonObject(T a) {
        this.a = a;
    }

    public void ToBe(T b) {
        bool? result = a?.Equals(b);

        if(result == null || result == false) {
            throw new Exception($"Expected {a} to equal {b}");
        }
    }

    public void ToBeCloseTo(T b, int numDigits) {
        var delta = Math.Pow(10, -numDigits);

        if(Math.Abs(Convert.ToDouble(a) - Convert.ToDouble(b)) > delta) {
            throw new Exception($"Expected {a} to be close to {b} with delta {delta}");
        }
    }

    public void ToBeGreaterThan(T b) {
        bool? result = a?.CompareTo(b) > 0;

        if(result == null || result == false) {
            throw new Exception($"Expected {a} to be greater than {b}");
        }
    }

    public void ToBeLessThan(T b) {
        bool? result = a?.CompareTo(b) < 0;

        if(result == null || result == false) {
            throw new Exception($"Expected {a} to be less than {b}");
        }
    }
}
