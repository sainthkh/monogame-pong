using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

public class Level2: World1 {
    protected override void AddMoreBlocks() {
        List<Point> pivots = new List<Point>() {
            new Point(80, 100),
            new Point(80, 650),
            new Point(350, 650),
            new Point(350, 100),
        };

        List<Point> direction = new List<Point> {
            new Point(1, 1),
            new Point(1, -1),
            new Point(-1, -1),
            new Point(-1, 1),
        };

        List<Point> points = new List<Point>();

        for(int i = 0; i < pivots.Count; i++) {
            Point pivot = pivots[i];
            int x = pivot.X;
            int y = pivot.Y;

            points.Add(new Point(x, y));

            for (int j = 0; j < 4; j++) {
                points.Add(new Point(x + 15 * (j + 1) * direction[i].X , y));
            }

            for (int j = 0; j < 4; j++) {
                points.Add(new Point(x, y + 15 * (j + 1) * direction[i].Y));
            }
        }

        foreach (Point point in points)
        {
            List<Point> p = new List<Point>();

            Brick brick = new Brick(point.X, point.Y, 15, 15);
            brick.Color = Color.White;
            bricks.Add(brick);
        }
    }

    public override void OnClickNextLevel()
    {
        SceneManager.LoadLevel(3);
    }
}
