using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

public class Level4: World1 {
    protected override void AddMoreBlocks() {
        List<Point> points = new List<Point>();

        const int NUM_BRICKS = 12;

        for(int i = 0; i < NUM_BRICKS; i++) {
            RotatingBrick brick = new RotatingBrick(0, 0, 15, 15);
            brick.Pivot = new Point(SharedResource.GameBounds.X / 2, SharedResource.GameBounds.Y / 2);
            brick.Radius = 150;
            brick.Rotation = 360 / NUM_BRICKS * i;
            brick.Color = Color.White;

            bricks.Add(brick);
        }
    }
}