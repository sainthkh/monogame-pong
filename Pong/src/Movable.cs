using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace mg_pong;

public class Movable: Actor {
    private Vector2 direction;

    public Vector2 Direction { 
        get
        {
            return direction;
        }

        set 
        {
            direction = value;
            direction.Normalize();
        }
    }

    public float DirectionX {
        get { return direction.X; } 
        set { 
            direction.X = value; 
            direction.Normalize();
        } 
    }

    public float DirectionY {
        get { return direction.Y; } 
        set { 
            direction.Y = value; 
            direction.Normalize();
        }
    }

    public float Speed { get; set; }
    public float MaxSpeed { get; set; }
}
