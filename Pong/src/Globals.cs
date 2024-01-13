using System;
using System.Collections.Generic;
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

public enum Side {
    Top,
    Bottom,
    Left,
    Right
}

public static class MathUtil {
    // It's counterclockwise.
    public static float Degree(Vector2 a, Vector2 b) {
        var det = a.X * b.Y - a.Y * b.X;
        var dot = a.X * b.X + a.Y * b.Y;

        return (float)(Math.Atan2(det, dot) * (180 / Math.PI));
    }

    public static List<float> CornerDegrees(Rectangle rect) {
        var corners = new List<Vector2> {
            new Vector2(-rect.Width, -rect.Height),
            new Vector2(-rect.Width, rect.Height),
            new Vector2(rect.Width, rect.Height),
            new Vector2(rect.Width, -rect.Height),
        };

        return corners.ConvertAll(corner => {
            corner.Normalize();
            return Degree(corner, new Vector2(0, 1));
        });
    }

    public static Side GetSide(List<float> degrees, float degree) {
        if (degree >= degrees[0] && degree < degrees[1]) {
            return Side.Left;
        }
        else if (degree >= degrees[1] && degree < degrees[2]) {
            return Side.Bottom;
        }
        else if (degree >= degrees[2] && degree < degrees[3]) {
            return Side.Right;
        }
        else {
            return Side.Top;
        }
    }

    public static int Sign(float direction) {
        return direction < 0 ? -1 : 1;
    }
}
