using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace mg_pong;

public class BallSnapshot: Snapshot {
    public Vector2 Direction;
    public float Speed;

    public BallSnapshot(Ball2 ball): base(ball) {
        Direction = ball.Direction;
        Speed = ball.Speed;
    }
}

public class Ball2: Movable {
    public const int BALL_SIZE = 10;

    public delegate void OnHitTopWall(Ball2 ball);
    public delegate void OnHitBottomWall(Ball2 ball);
    public event OnHitTopWall HitTopWall;
    public event OnHitBottomWall HitBottomWall;

    private List<float> cornerDegrees;

    public Ball2() {
        Direction = new Vector2(0.1f, 1f);
        Bounds = new Rectangle(0, 0, BALL_SIZE, BALL_SIZE);
        Speed = 550.0f;
        actorType = ActorType.Ball;

        cornerDegrees = MathUtil.CornerDegrees(Bounds);
    }

    public override Snapshot Snapshot()
    {
        return new BallSnapshot(this);
    }

    public void Move(float deltaTime) {
        MoveX(DirectionX * Speed * deltaTime, OnCollideSolid);
        MoveY(DirectionY * Speed * deltaTime, OnCollideSolid);
    }

    public void Render() {
        mg_pong.Render.Rectangle(Bounds, Color.YellowGreen);
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
        if (other.Type == ActorType.Player) {
            // Roll back movement
            X -= (int) (DirectionX * Speed * deltaTime * .6f);
            Y -= (int) (DirectionY * Speed * deltaTime * .6f);

            var player = (PlayerSnapshot)other;

            Vector2 vec = new Vector2(X - player.X, Y - player.Y);
            vec.Normalize();
            float degree = MathUtil.Degree(vec, new Vector2(0, 1));
            Side side = MathUtil.GetSide(player.CornerDegrees, degree);

            if (side == Side.Top) {
                var diff = Math.Abs(X - player.X) / (player.Width / 2);

                if ((diff) < 0.1f) {
                    Direction = new Vector2(0, -1);
                }
                else {
                    int dir = MathUtil.Sign(DirectionX);

                    Direction = new Vector2(diff * dir, -1);
                }
            }
            else if (side == Side.Bottom) {
                DirectionY = -DirectionY;
            }
            else if (side == Side.Left || side == Side.Right) {
                Direction = -Direction;
            }
        }
        else if (other.Type == ActorType.Enemy) {
            // Roll back movement
            X -= (int) (DirectionX * Speed * deltaTime);
            Y -= (int) (DirectionY * Speed * deltaTime);

            var enemy = (EnemySnapshot)other;

            Vector2 vec = new Vector2(X - enemy.X, Y - enemy.Y);
            vec.Normalize();
            float degree = MathUtil.Degree(vec, new Vector2(0, 1));
            Side side = MathUtil.GetSide(enemy.CornerDegrees, degree);

            if (side == Side.Top) {
                DirectionY = -DirectionY;
            }
            else if (side == Side.Bottom) {
                var xs = Xna.Rand.RandomFloat(.5f, 1.5f);
                var ys = Xna.Rand.RandomFloat(.5f, 1.5f);
                var xd = Xna.Rand.RandomSign();

                Direction = new Vector2(xs * xd, ys);
            }
            else if (side == Side.Left || side == Side.Right) {
                Direction = -Direction;
            }
        }
        else if (other.Type == ActorType.Brick) {
            // Roll back movement
            X -= (int) (DirectionX * Speed * deltaTime * .6f);
            Y -= (int) (DirectionY * Speed * deltaTime * .6f);

            var brick = (BrickSnapshot)other;

            Vector2 vec = new Vector2(X - brick.X, Y - brick.Y);
            vec.Normalize();
            float degree = MathUtil.Degree(vec, new Vector2(0, 1));
            Side side = MathUtil.GetSide(brick.CornerDegrees, degree);

            if (side == Side.Top || side == Side.Bottom) {
                DirectionY = -DirectionY;
            }
            else if (side == Side.Left || side == Side.Right) {
                DirectionX = -DirectionX;
            }
        }
    }
}
