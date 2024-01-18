using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class BrickManager {
    public static List<Brick2> bricks = new List<Brick2>();

    public static void Generate() {
        for(int i = 0; i < 5; i++) {
            var x = Xna.Rand.Next(0, GameBounds.X - 100);
            var y = Xna.Rand.Next(100, GameBounds.Y - 100 - 100);

            var tetris = new TetrisBrickGroup();

            tetris.Generate();
            tetris.Top = x;
            tetris.Left = y;

            bricks.AddRange(tetris.Bricks);
        }
    }

    public static void GenerateRandomBrick() {
        for(int i = 0; i < 20; i++) {
            var width = Xna.Rand.Next(10, 30);
            var height = Xna.Rand.Next(10, 30);
            var x = Xna.Rand.Next(0, GameBounds.X - width);
            var y = Xna.Rand.Next(100, GameBounds.Y - 100 - height);
            var brick = GenerateBrick(x, y, width, height, Color.White, BrickMoveType.None, BrickOnHitType.Break);

            bricks.Add(brick);
        }
    }

    public static void CheckCollision(Ball2 ball) {
        foreach(var brick in bricks) {
            if (brick.IsAlive && brick.Collides(ball)) {
                CollisionManager.AddCollision(ball, brick);
            }
        }
    }

    public static void Render() {
        foreach(var brick in bricks) {
            brick.Render();
        }
    }

    public static Brick2 GenerateBrick(int x, int y, int width, int height, Color color, BrickMoveType moveType, BrickOnHitType onHitType) {
        var brick = new Brick2();
        brick.X = x;
        brick.Y = y;
        brick.Width = width;
        brick.Height = height;
        brick.Color = color;
        brick.MoveType = moveType;
        brick.OnHitType = onHitType;

        return brick;
    }
}
