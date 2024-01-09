using System;
using Microsoft.Xna.Framework;

namespace mg_pong;

public enum GameObjectType {
    Actor,
    Solid,
}

public class GameObject {
    public readonly GameObjectType Type;
    public int X;
    public int Y;
    public Rectangle Bounds;

    protected GameObject(GameObjectType type) {
        Type = type;
    }

    public bool Collides(Rectangle other) {
        return Bounds.Intersects(other);
    }
}
