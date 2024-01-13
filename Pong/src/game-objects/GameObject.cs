using System;
using Microsoft.Xna.Framework;

namespace mg_pong;

public enum GameObjectType {
    Actor,
    Solid,
}

public class GameObject {
    private int id;
    private static int nextId = 0;
    private int x;
    private int y;
    private int width;
    private int height;

    private Rectangle bounds;
    
    public readonly GameObjectType Type;

    public int Id { get { return id; } }
    public int X { 
        get { return x; } 
        set { 
            x = value; 
            bounds.X = x - width / 2;
        } 
    }
    public int Y {
        get { return y; } 
        set { 
            y = value; 
            bounds.Y = y - height / 2;
        }
    }
    public int Width {
        get { return width; } 
        set { 
            width = value; 
            bounds.Width = width;
        }
    }
    public int Height {
        get { return height; } 
        set { 
            height = value; 
            bounds.Height = height;
        }
    }
    public Rectangle Bounds { 
        get { return bounds; } 
        set { 
            bounds = value; 
            x = bounds.X + bounds.Width / 2;
            y = bounds.Y + bounds.Height / 2;
            width = bounds.Width;
            height = bounds.Height;
        }
    }

    protected GameObject(GameObjectType type) {
        Type = type;
        id = nextId++;
    }

    public bool Collides(Rectangle other) {
        return Bounds.Intersects(other);
    }
}
