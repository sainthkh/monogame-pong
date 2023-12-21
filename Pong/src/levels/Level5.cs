using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

public class Level5: World1 {
    protected override void AddMoreBlocks()
    {
        const int NUM_BRICK_LINES = 6;
        const int NUM_BRICKS_PER_LINE = 6;

        for (int i = 0; i < NUM_BRICK_LINES; i++)
        {
            for (int j = 0; j < NUM_BRICKS_PER_LINE; j++)
            {
                int offset = i % 2 == 0 ? 0 : 30;

                BrickOnRail brick = new BrickOnRail(j * 60 + offset, i * 100 + 150, 15, 15);
                brick.Color = Color.White;

                bricks.Add(brick);
            }
        }
    }

    public override void OnClickNextLevel()
    {
        SceneManager.LoadLevel(6);
    }
}
