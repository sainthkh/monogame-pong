using Microsoft.Xna.Framework;


namespace mg_pong;

public class Ball {
    private Rectangle rec;
    public Rectangle Rec { get { return rec; } }

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

    public float X { get { return rec.X; } set { rec.X = (int)value; } }
    public float Y { get { return rec.Y; } set { rec.Y = (int)value; } }

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

    public delegate void OnHitTopWall(Ball ball);
    public delegate void OnHitBottomWall(Ball ball);
    public event OnHitTopWall HitTopWall;
    public event OnHitBottomWall HitBottomWall;


    public Ball() {
        rec = new Rectangle(0, 0, 10, 10);
        Direction = new Vector2(0.1f, 1f);
        Speed = 10.0f;
        MaxSpeed = 15f;
    }

    public void Move() {
        rec.X += (int)(Direction.X * Speed);
        rec.Y += (int)(Direction.Y * Speed);
    }

    public void CheckWallCollision() {
        var GameBounds = SharedResource.GameBounds;
        var Rand = SharedResource.Rand;

        if (Y < 0) 
        {
            Y = 1;
            DirectionY *= -1;
            HitTopWall(this);
        }
        else if (Y > GameBounds.Y)
        {
            Y = GameBounds.Y - 1;
            DirectionY *= -1;
            HitBottomWall(this);
        }

        if (X < 0 + 10)
        {
            X = 10 + 1;
            DirectionX *= -(1 + Rand.Next(-100, 101) * 0.005f);
        }
        else if (X > GameBounds.X - 10)
        {
            X = GameBounds.X - 11;
            DirectionX *= -(1 + Rand.Next(-100, 101) * 0.005f);
        }
    }
}
