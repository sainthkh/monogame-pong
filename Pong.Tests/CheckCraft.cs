using System;
using System.Collections.Generic;

public class Suite {
    public Action suite;
    public string description;
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
        Suites.Add(new Suite() {
            description = description,
            suite = suite
        });
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

    public static ComparisonObject<T> Expect<T>(T a) {
        return new ComparisonObject<T>(a);
    }

    public static void RunTests() {
        Suites.Run();
    }
}

public class ComparisonObject<T> {
    T a;

    public ComparisonObject(T a) {
        this.a = a;
    }

    public void ToBe(T b) {
        if(!a.Equals(b)) {
            throw new Exception($"Expected {a} to equal {b}");
        }
    }

    public void ToBeCloseTo(T b, int numDigits) {
        var delta = Math.Pow(10, -numDigits);

        if(Math.Abs(Convert.ToDouble(a) - Convert.ToDouble(b)) > delta) {
            throw new Exception($"Expected {a} to be close to {b} with delta {delta}");
        }
    }
}

public class T {
    // public void X() {
    //     Describe("asdf", () => {
    //         It("should be true", () => {
    //             Expect(1).ToBe(1);
    //         });
    //     });
    // }

    // public List<AA> a = new List<AA>() {
    //     new AA() {
    //         Name = "asdf",
    //         ()
    //     }
    // }
}