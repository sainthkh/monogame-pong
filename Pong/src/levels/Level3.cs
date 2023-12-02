using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

public class Level3: World1 {
    protected override void AddMoreBlocks() {
        List<Point> startingPoints = new List<Point>() {
            new Point(80, 100),
            new Point(150, 250),
            new Point(80, 400),
            new Point(150, 550),
        };

        List<Point> points = new List<Point>();

        foreach(Point startingPoint in startingPoints) {
            int x = startingPoint.X;
            int y = startingPoint.Y;

            points.Add(new Point(x, y));

            for(int i = 0; i < 6; i++) {
                points.Add(new Point(x + 45 * (i + 1), y));
            }
        }

        int brickIndex = 0;

        foreach(Point point in points) {
            List<Point> p = new List<Point>();

            Brick brick;
            
            if (brickIndex % 3 == 0) {
                brick = new RegeneratableBrick(point.X, point.Y, 15, 15);
                brick.Color = Color.Yellow;
            }
            else {
                brick = new Brick(point.X, point.Y, 15, 15);
                brick.Color = Color.White;
            }

            bricks.Add(brick);

            brickIndex++;
        }
    }

    public override void OnClickNextLevel()
    {
        SceneManager.LoadLevel(4);
    }
}
