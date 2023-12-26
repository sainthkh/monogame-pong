using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

public class Level7: World1 {
    protected override void AddMoreBlocks()
    {
        Point pivot = new Point(
            SharedResource.GameBounds.X / 2,
            SharedResource.GameBounds.Y / 2
        );
        int[] vars = new int[] { 10, 40 };
        int[] vars2 = new int[] { 40, 140 };

        for(int i = 0; i < 10; i++) {
            SpreadBrick brick = new SpreadBrick(
                15, 15,
                vars[0], vars[1],
                pivot, 36 * i
            );
            brick.Color = Color.Indigo;

            bricks.Add(brick);
        }

        for(int i = 0; i < 10; i++) {
            SpreadBrick brick = new SpreadBrick(
                15, 15,
                vars2[0], vars2[1],
                pivot, 36 * i
            );
            brick.Color = Color.Indigo;

            bricks.Add(brick);
        }
    }

    public override void OnClickNextLevel()
    {
        SceneManager.LoadLevel(8);
    }
}
