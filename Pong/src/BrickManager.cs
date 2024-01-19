using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class BrickManager {
    public static List<Brick2> bricks = new List<Brick2>();

    public static void Generate() {
        List<TetrisBrickGroup> tetrisBrickGroups = new List<TetrisBrickGroup>();

        for(int i = 0; i < 5; i++) {
            while(true) {
                var tetris = new TetrisBrickGroup();
                tetris.Generate();

                var x = Xna.Rand.Next(0, GameBounds.X - tetris.Bounds.Width);
                var y = Xna.Rand.Next(100, GameBounds.Y - 100 - tetris.Bounds.Height);

                tetris.Left = x;
                tetris.Top = y;

                bool intersects = false;
                foreach(var t in tetrisBrickGroups) {
                    if (t.Bounds.Intersects(tetris.Bounds)) {
                        intersects = true;
                        break;
                    }
                }

                if (!intersects) {
                    tetrisBrickGroups.Add(tetris);
                    bricks.AddRange(tetris.Bricks);
                    break;
                }
            }
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
