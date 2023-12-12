using System;
using Microsoft.Xna.Framework;


namespace mg_pong;

public class Ball {
    private Rectangle rec;
    public Rectangle Rec { get { return rec; } }
    const int BALL_SIZE = 10;

    private Vector2 direction;
    public Vector2 Direction { 
        get
        {
            return direction;
        }

        set 
        {
            direction = value;
            LimitDirection();
        }
    }

    public float X { get { return rec.X; } set { rec.X = (int)value; } }
    public float Y { get { return rec.Y; } set { rec.Y = (int)value; } }

    public float DirectionX {
        get { return direction.X; } 
        set { 
            direction.X = value; 
            LimitDirection();
        } 
    }
    public float DirectionY {
        get { return direction.Y; } 
        set { 
            direction.Y = value; 
            LimitDirection();
        }
    }

    public float Speed { get; set; }
    public float MaxSpeed { get; set; }

    public delegate void OnHitTopWall(Ball ball);
    public delegate void OnHitBottomWall(Ball ball);
    public delegate void OnHitPlayer(Ball ball, Paddle player);
    public event OnHitTopWall HitTopWall;
    public event OnHitBottomWall HitBottomWall;
    public event OnHitPlayer HitPlayer;


    public Ball() {
        rec = new Rectangle(0, 0, BALL_SIZE, BALL_SIZE);
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

    public void CheckPlayerCollision(Paddle player) {
        if (player.Collides(this))
        {
            if (Y > player.Y + player.Height * 0.5 &&
                X > player.X + player.Width * 0.05 && X < player.X + player.Width * 0.95 ) // Ball hits bottom
            {
                DirectionY *= -1;
                Y = player.Y + player.Height + 1;
            }
            else // Ball hits other 3 sides
            {
                float y = -1;

                float diff = ((X + BALL_SIZE / 2) - (player.X + player.Width / 2)) / (player.Width / 2);
                diff = MathF.Abs(diff);
                float dir = DirectionX < 0 ? -1 : 1;

                float x = diff < 0.15
                    ? 0
                    : diff * 2f * dir;
                
                if ((X > player.X + player.Width * 0.9 && x < 0) || // Hit right
                    (X < player.X + player.Width * 0.1 && x > 0) // Hit left
                )
                {
                    x *= -1;
                }

                Direction = new Vector2(x, y);
                Y = player.Y - BALL_SIZE - 1;
            }

            HitPlayer(this, player);
        }
    }

    private void LimitDirection() {
        if (MathF.Abs(DirectionY) * 2 < MathF.Abs(DirectionX)) {
            int dirX = DirectionX < 0 ? -1 : 1;
            int dirY = DirectionY < 0 ? -1 : 1;

            var dir = new Vector2(dirX * 1.95f, dirY);
            dir.Normalize();

            direction = dir;
        }
    }
}
