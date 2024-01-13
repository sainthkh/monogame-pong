using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace mg_pong;

public class Ball2: Movable {
    public const int BALL_SIZE = 10;

    public delegate void OnHitTopWall(Ball2 ball);
    public delegate void OnHitBottomWall(Ball2 ball);
    public event OnHitTopWall HitTopWall;
    public event OnHitBottomWall HitBottomWall;

    public Ball2() {
        Direction = new Vector2(0.1f, 1f);
        Bounds = new Rectangle(0, 0, BALL_SIZE, BALL_SIZE);
        Speed = 400.0f;
    }

    public void Move(float deltaTime) {
        MoveX(DirectionX * Speed * deltaTime, OnCollideSolid);
        MoveY(DirectionY * Speed * deltaTime, OnCollideSolid);
    }

    public void Render() {
        Draw.Rectangle(Bounds, Color.YellowGreen);
    }

    public void OnCollideSolid(GameObject paddle, Solid solid) {
        if (solid is Wall) {
            var wall = (Wall)solid;

            if (wall.WallType == WallType.Top) {
                DirectionY = -DirectionY;
                HitTopWall?.Invoke(this);
            }
            else if (wall.WallType == WallType.Bottom) {
                DirectionY = -DirectionY;
                HitBottomWall?.Invoke(this);
            }
            else if (wall.WallType == WallType.Left) {
                DirectionX = -DirectionX;
            }
            else if (wall.WallType == WallType.Right) {
                DirectionX = -DirectionX;
            }
        }
    }

    public override void OnCollideActor(Snapshot other, float deltaTime)
    {
        Console.WriteLine("Ball collided with actor");
    }
}
