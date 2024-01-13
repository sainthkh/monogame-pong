using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mg_pong;

public static class GameBounds {
    public static int X;
    public static int Y;
}

public static class WindowBounds {
    public static int X;
    public static int Y;
}

public static class Xna {
    public static GraphicsDevice GraphicsDevice;
    public static SpriteBatch SpriteBatch;
}

public static class Util {
    // It's counterclockwise.
    public static float Degree(Vector2 a, Vector2 b) {
        var det = a.X * b.Y - a.Y * b.X;
        var dot = a.X * b.X + a.Y * b.Y;

        return (float)(Math.Atan2(det, dot) * (180 / Math.PI));
    }
}
