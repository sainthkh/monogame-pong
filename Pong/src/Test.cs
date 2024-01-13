using System;
using Microsoft.Xna.Framework;

namespace mg_pong;

public static class Test {
    public static void TestGetSide() {
        var ds = MathUtil.CornerDegrees(new Rectangle(0, 0, 10, 10));

        foreach(var d in ds) {
            Console.WriteLine(d);
        }

        Vector2 p = new Vector2(-4, -5);

        var deg = MathUtil.Degree(p, new Vector2(0, 1));

        Console.WriteLine(deg);

        var side = MathUtil.GetSide(ds, deg);

        Console.WriteLine(side); // Top
    }

    public static void TestDegree() {
        for(int i = 10; i >= 0; i--) {
            Vector2 vec = new Vector2(1f - 0.1f * i, 1);
            vec.Normalize();

            var deg = MathUtil.Degree(vec, new Vector2(0, 1));

            Console.WriteLine(deg); // 0-45
        }

        for(int i = 0; i <= 10; i++) {
            Vector2 vec = new Vector2(1f, 1f - 0.2f * i);
            vec.Normalize();

            var deg = MathUtil.Degree(vec, new Vector2(0, 1));

            Console.WriteLine(deg); // 45-135
        }
    }
}
