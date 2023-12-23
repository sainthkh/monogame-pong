using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

public class Level6: World1 {
    protected override void AddMoreBlocks()
    {
        var points = new List<Point>() {
            new Point(80, 100), new Point(180, 100), new Point(280, 100), new Point(380, 100), new Point(480, 100), new Point(580, 100),
            new Point(80, 650), new Point(80, 550), new Point(80, 450), new Point(80, 350), new Point(80, 250), new Point(80, 150),
            new Point(210, 390), new Point(50, 390), new Point(160, 390), new Point(270, 390), new Point(380, 390), new Point(490, 390),
            new Point(350, 650), new Point(350, 550), new Point(350, 450), new Point(350, 350), new Point(350, 250), new Point(350, 150),
            new Point(350, 100), new Point(300, 100), new Point(250, 100), new Point(200, 100), new Point(150, 100), new Point(100, 100),
        };

        GeneratorBrick generatorBrick = null;
        for(int i = 0; i < points.Count; i++) {
            Point point = points[i];

            if (i % 6 == 0) {
                generatorBrick = new GeneratorBrick(point.X, point.Y, 45, 45);
                generatorBrick.Color = Color.Purple;

                bricks.Add(generatorBrick);
            }
            else
            {
                Brick brick = new Brick(point.X, point.Y, 15, 15);
                brick.Color = Color.White;
                brick.IsAlive = false;

                generatorBrick.ToBeGenerated.Add(brick);

                bricks.Add(brick);
            }
        }
    }

    public override void OnClickNextLevel()
    {
        SceneManager.LoadLevel(7);
    }
}
